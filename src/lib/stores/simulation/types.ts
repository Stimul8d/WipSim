import type { Task, Worker } from '../../types/core'

export interface Params {
    tasks: Task[]
    workers: Worker[]
    maxWip: number
    arrivalRate: number
    taskSize: number
    startingTasks: number
    time: string
    running: boolean
    workTypes: {
        frontend: boolean
        backend: true 
        devops: boolean
        testing: boolean
    }
}