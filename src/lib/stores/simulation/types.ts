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
        backend: boolean 
        devops: boolean
        testing: boolean
    }
}

export interface SimState extends Params {
    workerMap: Map<string, Worker>
    simTicks: number
}

export type StoreUpdater = (state: SimState) => SimState