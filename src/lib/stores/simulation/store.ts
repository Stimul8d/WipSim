import { writable } from 'svelte/store'
import type { Params, SimState, StoreUpdater } from './types'
import type { Worker, Task } from '../../types/core'
import { TaskStatus, TaskType as TT } from '../../types/constants'
import { SIM_SPEED, SIM_TIME } from './constants'

function getRandomTaskType(workTypes: Params['workTypes']): TT {
    const enabledTypes = Object.entries(workTypes)
        .filter(([_, enabled]) => enabled)
        .map(([type]) => type.toUpperCase() as TT)
    return enabledTypes[Math.floor(Math.random() * enabledTypes.length)]
}

function generateTask(id: number, workTypes: Params['workTypes'], tick: number): Task {
    return {
        id: `TASK-${id}`,
        type: id === 1 ? TT.FRONTEND : getRandomTaskType(workTypes),
        status: TaskStatus.BACKLOG,
        progress: 0,
        complexity: 1,
        createdAt: tick
    }
}

function generateWorker(id: number): Worker {
    return {
        id: `ENG-${id}`,
        name: `Engineer ${id}`,
        currentTasks: [],
        efficiency: 1,
        maxTasks: 1,
        skills: Object.values(TT).map(type => ({
            type,
            level: 3
        }))
    }
}

function buildWorkerMap(workers: Worker[]): Map<string, Worker> {
    return new Map(workers.map(w => [w.id, { ...w }]))
}

const initialState: SimState = {
    tasks: [generateTask(1, { frontend: true, backend: true, devops: true, testing: true }, 0)],
    workers: [generateWorker(1)],
    workerMap: new Map([[generateWorker(1).id, generateWorker(1)]]),
    maxWip: 1,
    arrivalRate: 1,
    taskSize: 1,
    startingTasks: 1,
    time: 'D0:00',  
    running: false,
    simTicks: 0,
    workTypes: {
        frontend: true,
        backend: true,
        devops: true,
        testing: true
    }
}

function resetState(currentState: SimState): SimState {
    const worker = generateWorker(1)
    return {
        ...currentState,
        tasks: [generateTask(1, currentState.workTypes, 0)],
        workers: [worker],
        workerMap: new Map([[worker.id, { ...worker }]]),
        time: 'D0:00',
        running: false,
        simTicks: 0
    }
}

function formatSimTime(ticks: number): string {
    const hours = ticks * SIM_TIME.TICK_HOURS
    const days = Math.floor(hours / 24)
    const remainingHours = hours % 24
    return `D${days}:${remainingHours.toString().padStart(2, '0')}`
}

function tryAllocateTasks(workers: Worker[], tasks: Task[], tick: number): void {
    let allocated = true
    while (allocated) {
        const freeWorker = workers.find(w => w.currentTasks.length < w.maxTasks)
        const freeTask = tasks.find(t => !t.assignedTo && t.status === TaskStatus.BACKLOG)
        
        if (freeWorker && freeTask) {
            freeTask.assignedTo = freeWorker.id
            freeTask.status = TaskStatus.IN_PROGRESS
            freeTask.startedAt = tick
            freeWorker.currentTasks.push(freeTask.id)
        } else {
            allocated = false
        }
    }
}

function progressTasks(tasks: Task[], workers: Worker[], tick: number): void {
    tasks.forEach(task => {
        if (task.assignedTo && task.status === TaskStatus.IN_PROGRESS) {
            task.progress = Math.min(100, task.progress + SIM_SPEED.PROGRESS_PCT)
            if (task.progress === 100) {
                const worker = workers.find(w => w.id === task.assignedTo)
                if (worker) {
                    worker.currentTasks = worker.currentTasks.filter(id => id !== task.id)
                    task.assignedTo = undefined
                    task.status = TaskStatus.DONE
                    task.completedAt = tick
                }
            }
        }
    })
}

export function createStore() {
    const { subscribe, set, update } = writable<SimState>(initialState)
    let timer: NodeJS.Timeout | undefined

    const store = {
        subscribe,
        set,
        update: (updater: StoreUpdater) => {
            update(state => {
                const newState = updater(state)
                if (newState.startingTasks !== state.startingTasks || newState.workTypes !== state.workTypes) {
                    newState.tasks = Array.from(
                        { length: newState.startingTasks }, 
                        (_, i) => generateTask(i + 1, newState.workTypes, newState.simTicks)
                    )
                }
                if (newState.workers !== state.workers) {
                    newState.workerMap = buildWorkerMap(newState.workers)
                }
                return newState
            })
        },
        reset: () => {
            if (timer) clearInterval(timer)
            timer = undefined
            update(state => resetState(state))
        },
        start: () => {
            update(s => ({ ...s, running: true }))
            timer = setInterval(() => {
                update(s => {
                    progressTasks(s.tasks, s.workers, s.simTicks)
                    tryAllocateTasks(s.workers, s.tasks, s.simTicks)
                    return { 
                        ...s, 
                        simTicks: s.simTicks + 1,
                        time: formatSimTime(s.simTicks + 1)
                    }
                })
            }, SIM_SPEED.TICK_MS)
        },
        stop: () => {
            if (timer) clearInterval(timer)
            timer = undefined
            update(s => ({ ...s, running: false }))
        }
    }

    return store
}

export const simStore = createStore()
export type { SimState, StoreUpdater }