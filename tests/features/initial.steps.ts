import { test, expect } from 'vitest'
import { loadFeature, defineFeature } from 'jest-cucumber'
import { simStore } from '../../src/lib/stores/simulation'
import { TaskType, TaskStatus } from '../../src/lib/types/constants'

const feature = loadFeature('tests/features/initial.feature')

defineFeature(feature, test => {
    test('Default task state', ({ given, then }) => {
        given('I load the simulation', () => {
            simStore.reset()
        })

        then('there should be exactly one task', () => {
            let tasks
            simStore.subscribe(state => {
                tasks = state.tasks
            })
            expect(tasks.length).toBe(1)
        })

        then('it should be in the first column', () => {
            let tasks
            simStore.subscribe(state => {
                tasks = state.tasks
            })
            expect(tasks[0].progress).toBe(0)
            expect(tasks[0].status).toBe(TaskStatus.BACKLOG)
        })

        then('it should have default properties', () => {
            let tasks
            simStore.subscribe(state => {
                tasks = state.tasks
            })
            expect(tasks[0]).toMatchObject({
                id: 'TASK-1',
                type: TaskType.FRONTEND,
                complexity: 1
            })
        })
    })
})