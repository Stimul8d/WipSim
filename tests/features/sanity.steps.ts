import { defineFeature, loadFeature } from 'jest-cucumber'
import { test, expect } from 'vitest'

const feature = loadFeature('./tests/features/sanity.feature')

defineFeature(feature, test => {
  test('Running a basic test', ({ given, when, then }) => {
    let testRan = false

    given('I have written a test', () => {
      testRan = false
    })

    when('I run the test suite', () => {
      testRan = true
    })

    then('it should actually do something useful', () => {
      expect(testRan).toBe(true)
    })
  })
})