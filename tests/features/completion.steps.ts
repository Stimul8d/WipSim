import { test, expect, vi } from 'vitest'
import { loadFeature, defineFeature } from 'jest-cucumber'
import { simStore } from '../../src/lib/stores/simulation'
import type { Task } from '../../src/lib/types/core'
import { SIM_SPEED } from '../../src/lib/stores/simulation/constants'

const feature = loadFeature('tests/features/completion.feature')

defineFeature(feature, test => {
    test('Worker finishes task and moves on', ({ given, when, then }) => {
        vi.useFakeTimers()
        let tasks: Task[] = []
        const unsubscribe = simStore.subscribe(s => tasks = s.tasks)

        given('I load the simulation with 2 tasks', () => {
            simStore.reset()
            simStore.update(s => ({ ...s, startingTasks: 2 }))
        })

        when('I start the simulation', () => {
            simStore.start()
        })

        when('wait 10 seconds', () => {
            // 6 ticks for first task (1 for allocation + 5 for progress)
            // 6 ticks for second task
            vi.advanceTimersByTime(SIM_SPEED.TICK_MS * 12)
        })

        then('both tasks should be complete', () => {
            expect(tasks.every(t => t.progress === 100)).toBe(true)
            unsubscribe()
            vi.useRealTimers()
        })
    })
})