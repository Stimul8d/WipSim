import { test, expect, vi } from 'vitest'
import { loadFeature, defineFeature } from 'jest-cucumber'
import { simStore } from '../../src/lib/stores/simulation'
import type { Task } from '../../src/lib/types/core'
import { SIM_SPEED } from '../../src/lib/stores/simulation/constants'

const feature = loadFeature('tests/features/progress.feature')

defineFeature(feature, test => {
    test('Task progresses after allocation', ({ given, when, then }) => {
        vi.useFakeTimers()
        let tasks: Task[] = []
        const unsubscribe = simStore.subscribe(s => tasks = s.tasks)

        given('I load the simulation', () => {
            simStore.reset()
        })

        when('I start the simulation', () => {
            simStore.start()
            // One tick for allocation
            vi.advanceTimersByTime(SIM_SPEED.TICK_MS)
        })

        when('wait 2 seconds', () => {
            vi.advanceTimersByTime(SIM_SPEED.TICK_MS)
        })

        then('the first task should have 20% progress', () => {
            expect(tasks[0].progress).toBe(SIM_SPEED.PROGRESS_PCT)
            unsubscribe()
            vi.useRealTimers()
        })
    })
})