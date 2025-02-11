import type { Task } from '../../types/core'

export interface Params {
    tasks: Task[]
    engineers: number
    maxWip: number
    arrivalRate: number
    taskSize: number
    startingTasks: number
    workTypes: {
        frontend: boolean
        backend: boolean
        devops: boolean
        testing: boolean
    }
}