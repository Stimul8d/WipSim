import { test, expect } from 'vitest'
import { loadFeature, defineFeature } from 'jest-cucumber'
import { simStore } from '../../src/lib/stores/simulation'

const feature = loadFeature('tests/features/team.feature')

defineFeature(feature, test => {
    let state: any

    test('Default parameter values', ({ given, then }) => {
        given('I load the simulation', () => {
            simStore.subscribe(s => state = s)
        })

        then('there should be one worker', () => {
            expect(state.workers.length).toBe(1)
        })

        then('max WIP should be set to 1', () => {
            expect(state.maxWip).toBe(1)
        })

        then('arrival rate should be set to 1', () => {
            expect(state.arrivalRate).toBe(1)
        })

        then('task size should be set to 1', () => {
            expect(state.taskSize).toBe(1)
        })

        then('starting tasks should be set to 1', () => {
            expect(state.startingTasks).toBe(1)
        })

        then('all work types should be enabled', () => {
            expect(state.workTypes).toEqual({
                frontend: true,
                backend: true,
                devops: true,
                testing: true
            })
        })
    })
})