import { defineFeature, loadFeature } from 'jest-cucumber'
import { test, expect } from 'vitest'
import { simStore } from '../../src/lib/stores/simulation'
import type { Params } from '../../src/lib/stores/simulation/types'

const feature = loadFeature('./tests/features/controls.feature')

defineFeature(feature, test => {
  let state: Params
  let currentControl: keyof Params
  let currentValue: number

  test('Control updates sync to store', ({ given, when, then }) => {
    given('I load the simulation', () => {
      simStore.subscribe(s => state = s)
    })

    when(/^I change (.*) to (.*)$/, (control: keyof Params, value: string) => {
      currentControl = control
      currentValue = parseInt(value)
      simStore.update(s => ({...s, [control]: currentValue}))
    })

    then('the store should reflect that change', () => {
      expect(state[currentControl]).toBe(currentValue)
    })
  })
})