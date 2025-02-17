import { defineFeature, loadFeature } from 'jest-cucumber'
import { test, expect } from 'vitest'
import { TaskStatus } from '../../src/lib/types/constants'
import type { Task } from '../../src/lib/types/core'
import { SIM_TIME } from '../../src/lib/stores/simulation/constants'

const feature = loadFeature('./tests/features/timing.feature')

defineFeature(feature, test => {
    test('Measuring cycle time', ({ given, when, then }) => {
        let task: Task
        const startTick = 10

        given('a task started 3 ticks ago', () => {
            task = {
                createdAt: startTick - 2,
                startedAt: startTick,
                status: TaskStatus.IN_PROGRESS
            } as Task
        })

        when('the task is completed', () => {
            task.status = TaskStatus.DONE
            task.completedAt = startTick + 3
        })

        then('the cycle time should show >12 hours', () => {
            const cycleTime = (task.completedAt! - task.startedAt!) * SIM_TIME.TICK_HOURS
            expect(cycleTime).toBe(12)
        })
    })

    test('Measuring lead time', ({ given, when, then }) => {
        let task: Task
        const startTick = 10

        given('a task created 5 ticks ago', () => {
            task = {
                createdAt: startTick,
                status: TaskStatus.BACKLOG
            } as Task
        })

        when('it completes after 3 more ticks', () => {
            task.startedAt = startTick + 2
            task.status = TaskStatus.IN_PROGRESS
            task.completedAt = startTick + 5
            task.status = TaskStatus.DONE
        })

        then('the lead time should show >32 hours', () => {
            const leadTime = (task.completedAt! - task.createdAt) * SIM_TIME.TICK_HOURS
            expect(leadTime).toBe(20) // 5 ticks * 4h
        })
    })

    test('Realistic task durations', ({ given, when, then }) => {
        let task: Task
        let currentTick = 0

        given('a complex task', () => {
            task = {
                createdAt: currentTick,
                complexity: 2,
                status: TaskStatus.BACKLOG
            } as Task
        })

        when('the task goes through the system', () => {
            currentTick += 1
            task.startedAt = currentTick
            task.status = TaskStatus.IN_PROGRESS
            
            // Simulate progress over 5 ticks
            for(let i = 0; i < 5; i++) {
                task.progress = (i + 1) * 20
                currentTick++
            }
            
            task.completedAt = currentTick
            task.status = TaskStatus.DONE
        })

        then('it should take around 5 ticks (20 hours) to complete', () => {
            expect(task.completedAt! - task.startedAt!).toBe(SIM_TIME.TICKS_PER_TASK)
            expect((task.completedAt! - task.startedAt!) * SIM_TIME.TICK_HOURS).toBe(20)
        })
    })
})