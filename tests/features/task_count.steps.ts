import { test, expect } from 'vitest'
import { loadFeature, defineFeature } from 'jest-cucumber'
import { simStore } from '../../src/lib/stores/simulation'
import type { Task } from '../../src/lib/types/core'

const feature = loadFeature('tests/features/task_count.feature')

defineFeature(feature, test => {
    test('Changing starting task count', ({ given, when, then }) => {
        let tasks: Task[] = []
        const unsubscribe = simStore.subscribe(s => tasks = s.tasks)

        given('I load the simulation', () => {
            simStore.reset()
        })

        when('I change starting tasks to 5', () => {
            simStore.update(s => ({ ...s, startingTasks: 5 }))
        })

        then('there should be exactly 5 tasks in the grid', () => {
            expect(tasks.length).toBe(5)
            unsubscribe()
        })
    })
})