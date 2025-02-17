import { defineFeature, loadFeature } from 'jest-cucumber'
import { test, expect } from 'vitest'
import { simStore } from '../../src/lib/stores/simulation'
import { TaskStatus } from '../../src/lib/types/constants'

const feature = loadFeature('./tests/features/reset.feature')

defineFeature(feature, test => {
    test('Reset from running state', ({ given, when, then, and }) => {
        let state: any

        given('a simulation with multiple tasks and workers', () => {
            simStore.update(s => ({
                ...s,
                startingTasks: 5,
                workers: [...s.workers, { id: 'ENG-2', name: 'Test 2'}]
            }))
            simStore.start()
            // Let some time pass
            state = { simTicks: 10 }
            simStore.update(s => ({ ...s, ...state }))
        })

        when('tasks are in progress and I reset', () => {
            simStore.reset()
            simStore.subscribe(s => { state = s })()
        })

        then('all tasks should be cleared', () => {
            expect(state.tasks.length).toBe(1)
            expect(state.workers.length).toBe(1)
            expect(state.tasks[0].status).toBe(TaskStatus.BACKLOG)
        })

        and('time should be 00:00', () => {
            expect(state.time).toBe('00:00')
            expect(state.simTicks).toBe(0)
        })

        and('sim should be stopped', () => {
            expect(state.running).toBe(false)
        })
    })

    test('Reset settings preserved', ({ given, when, then }) => {
        let state: any

        given('I change task arrival rate to 3', () => {
            simStore.update(s => ({
                ...s,
                arrivalRate: 3,
                maxWip: 5
            }))
        })

        when('I reset the simulation', () => {
            simStore.reset()
            simStore.subscribe(s => { state = s })()
        })

        then('arrival rate should still be 3', () => {
            // Settings that should persist
            expect(state.arrivalRate).toBe(3)
            expect(state.maxWip).toBe(5)
            // But time-based stuff resets
            expect(state.simTicks).toBe(0)
            expect(state.time).toBe('00:00')
        })
    })
})