import { defineFeature, loadFeature } from 'jest-cucumber'
import { test, expect, beforeEach } from 'vitest'
import { TaskStatus } from '../../src/lib/types/constants'
import { createStore } from '../../src/lib/stores/simulation/store'
import { runAllocation } from '../utils'

const feature = loadFeature('./tests/features/parallel.feature')

defineFeature(feature, test => {
    let store: ReturnType<typeof createStore>
    
    beforeEach(() => {
        store = createStore()
    })

    test('Four workers and four tasks', ({ given, when, then }) => {
        let state: any
        
        given('4 workers and 4 tasks in backlog', () => {
            store.update(s => ({ ...s, startingTasks: 4 }))
            store.update(s => ({
                ...s,
                workers: Array.from({ length: 4 }, (_, i) => ({
                    id: `ENG-${i+1}`,
                    name: `Engineer ${i+1}`,
                    currentTasks: [],
                    efficiency: 1,
                    maxTasks: 1,
                    skills: []
                }))
            }))
            store.subscribe(s => { state = s })()
        })

        when('the simulation starts', () => {
            store.start()
            store.update(runAllocation)
            store.subscribe(s => { state = s })()
        })

        then('all tasks should be allocated in the first tick', () => {
            expect(state.tasks.filter(t => t.status === TaskStatus.IN_PROGRESS).length).toBe(4)
            expect(state.workers.every(w => w.currentTasks.length === 1)).toBe(true)
        })
    })

    test('Respecting WIP limits', ({ given, and, when, then }) => {
        let state: any
        
        given('4 workers with max WIP of 2 each', () => {
            store.update(s => ({
                ...s,
                workers: Array.from({ length: 4 }, (_, i) => ({
                    id: `ENG-${i+1}`,
                    name: `Engineer ${i+1}`,
                    currentTasks: [],
                    efficiency: 1,
                    maxTasks: 2,
                    skills: []
                }))
            }))
        })

        and('8 tasks in backlog', () => {
            store.update(s => ({ ...s, startingTasks: 8 }))
            store.subscribe(s => { state = s })()
        })

        when('the simulation starts', () => {
            store.start()
            store.update(runAllocation)
            store.subscribe(s => { state = s })()
        })

        then('all workers should have 2 tasks each', () => {
            expect(state.tasks.filter(t => t.status === TaskStatus.IN_PROGRESS).length).toBe(8)
            expect(state.workers.every(w => w.currentTasks.length === 2)).toBe(true)
        })
    })
})