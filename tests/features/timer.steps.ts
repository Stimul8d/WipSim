import { test, expect, vi } from 'vitest'
import { loadFeature, defineFeature } from 'jest-cucumber'
import { simStore } from '../../src/lib/stores/simulation'

const feature = loadFeature('tests/features/timer.feature')

defineFeature(feature, test => {
    test('Starting the timer', ({ given, when, then }) => {
        // Set up fake timers
        vi.useFakeTimers()
        let time = '00:00'
        const unsubscribe = simStore.subscribe(s => time = s.time)

        given('I load the simulation', () => {
            simStore.reset()
        })

        when('I click start', () => {
            simStore.start()
        })

        then('the timer should start from 00:00', () => {
            expect(time).toBe('00:00')
        })

        then('after 1 second it should show 00:01', () => {
            vi.advanceTimersByTime(1000)
            expect(time).toBe('00:01')
            unsubscribe()
            vi.useRealTimers()
        })
    })
})