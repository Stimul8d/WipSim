import { defineFeature, loadFeature } from 'jest-cucumber'
import { test, expect, vi } from 'vitest'
import { simStore } from '../../src/lib/stores/simulation'
import type { SimState } from '../../src/lib/stores/simulation/store'

const feature = loadFeature('./tests/features/timer.feature')

defineFeature(feature, test => {
    test('Starting the timer', ({ given, when, then, and }) => {
        let time = 'D0:00'
        
        given('I load the simulation', () => {
            vi.useFakeTimers()
            simStore.reset()
        })
        
        when('I click start', () => {
            simStore.start()
        })
        
        then('the timer should start from D0:00', () => {
            const unsubscribe = simStore.subscribe((s: SimState) => time = s.time)
            expect(time).toBe('D0:00')
            unsubscribe()
        })
        
        and('after 1 second it should show D0:04', () => {
            const unsubscribe = simStore.subscribe((s: SimState) => time = s.time)
            vi.advanceTimersByTime(1000)
            expect(time).toBe('D0:04')
            unsubscribe()
            vi.useRealTimers()
        })
    })
})