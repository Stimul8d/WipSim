import { test, expect, vi } from 'vitest'
import { loadFeature, defineFeature } from 'jest-cucumber'
import { simStore } from '../../src/lib/stores/simulation'
import type { Task, Worker } from '../../src/lib/types/core'

const feature = loadFeature('tests/features/allocation.feature')

defineFeature(feature, test => {
    test('Engineer grabs next task', ({ given, when, then }) => {
        vi.useFakeTimers()
        let state: { tasks: Task[], workers: Worker[] }
        const unsubscribe = simStore.subscribe(s => state = s)

        given('I load the simulation', () => {
            simStore.reset()
        })

        given('there is one engineer', () => {
            // Our default state should handle this
            expect(state.workers.length).toBe(1)
        })

        when('I start the simulation', () => {
            simStore.start()
            vi.advanceTimersByTime(1000)
        })

        then('they should pick up the first task', () => {
            expect(state.tasks[0].assignedTo).toBe(state.workers[0].id)
            unsubscribe()
            vi.useRealTimers()
        })
    })
})