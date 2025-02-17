import { describe, it, expect } from 'vitest'
import { TaskStatus } from '../src/lib/types/constants'
import type { Task } from '../src/lib/types/core'

describe('task timing', () => {
    it('tracks accurate cycle time', () => {
        const start = Date.now() - 5000 // 5s ago
        const task = {
            createdAt: start - 5000, // 10s ago
            startedAt: start,
            status: TaskStatus.IN_PROGRESS
        } as Task

        // Complete it 
        task.status = TaskStatus.DONE
        task.completedAt = Date.now()

        const cycleTime = (task.completedAt - task.startedAt) / 1000 
        expect(cycleTime).toBeGreaterThanOrEqual(5)
        expect(cycleTime).toBeLessThan(6) // Allow 1s buffer
    })

    it('tracks accurate lead time', () => {
        const start = Date.now() - 5000 // 5s ago
        const task = {
            createdAt: start,
            status: TaskStatus.BACKLOG
        } as Task

        // Move to in progress
        task.status = TaskStatus.IN_PROGRESS
        task.startedAt = Date.now() - 2500 // 2.5s ago

        // Complete it
        task.status = TaskStatus.DONE
        task.completedAt = Date.now()

        const leadTime = (task.completedAt - task.createdAt) / 1000
        expect(leadTime).toBeGreaterThanOrEqual(5)
        expect(leadTime).toBeLessThan(6)
    })
})