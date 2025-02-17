import { writable } from 'svelte/store'
import type { Params } from './types'
import type { Worker } from '../../types/core'
import { TaskStatus, TaskType as TT } from '../../types/constants'
import { SIM_SPEED } from './constants'

function getRandomTaskType(workTypes: Params['workTypes']) {
    const enabledTypes = Object.entries(workTypes)
        .filter(([_, enabled]) => enabled)
        .map(([type]) => type.toUpperCase() as TT)
    return enabledTypes[Math.floor(Math.random() * enabledTypes.length)]
}

function generateTask(id: number, workTypes: Params['workTypes']) {
    return {
        id: `TASK-${id}`,
        type: id === 1 ? TT.FRONTEND : getRandomTaskType(workTypes),
        status: TaskStatus.BACKLOG,
        progress: 0,
        complexity: 1
    }
}

function generateWorker(id: number) {
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
    return new Map(workers.map(w => [w.id, w]))
}

interface SimState extends Params {
    workerMap: Map<string, Worker>
}

const defaultState: SimState = {
    tasks: [generateTask(1, { frontend: true, backend: true, devops: true, testing: true })],
    workers: [generateWorker(1)],
    workerMap: new Map([[generateWorker(1).id, generateWorker(1)]]),
    maxWip: 1,
    arrivalRate: 1,
    taskSize: 1,
    startingTasks: 1,
    time: '00:00',
    running: false,
    workTypes: {
        frontend: true,
        backend: true,
        devops: true,
        testing: true
    }
}

function formatTime(seconds: number): string {
    const mins = Math.floor(seconds / 60)
    const secs = seconds % 60
    return `${mins.toString().padStart(2, '0')}:${secs.toString().padStart(2, '0')}`
}

function tryAllocateTasks(workers: Worker[], tasks: Params['tasks']) {
    const freeWorker = workers.find(w => w.currentTasks.length < w.maxTasks)
    const freeTask = tasks.find(t => !t.assignedTo && t.status === TaskStatus.BACKLOG)
    
    if (freeWorker && freeTask) {
        freeTask.assignedTo = freeWorker.id
        freeTask.status = TaskStatus.IN_PROGRESS
        freeWorker.currentTasks.push(freeTask.id)
    }
}

function progressTasks(tasks: Params['tasks'], workers: Worker[]) {
    tasks.forEach(task => {
        if (task.assignedTo && task.status === TaskStatus.IN_PROGRESS) {
            task.progress = Math.min(100, task.progress + SIM_SPEED.PROGRESS_PCT)
            if (task.progress === 100) {
                const worker = workers.find(w => w.id === task.assignedTo)
                if (worker) {
                    worker.currentTasks = worker.currentTasks.filter(id => id !== task.id)
                    task.assignedTo = undefined
                    task.status = TaskStatus.DONE
                }
            }
        }
    })
}

function createStore() {
    const { subscribe, set, update } = writable<SimState>(defaultState)
    let timer: ReturnType<typeof setInterval>
    let seconds = 0

    return {
        subscribe,
        set,
        update: (updater: (state: SimState) => SimState) => {
            update(state => {
                const newState = updater(state)
                if (newState.startingTasks !== state.startingTasks || newState.workTypes !== state.workTypes) {
                    newState.tasks = Array.from(
                        { length: newState.startingTasks }, 
                        (_, i) => generateTask(i + 1, newState.workTypes)
                    )
                }
                if (newState.workers !== state.workers) {
                    newState.workerMap = buildWorkerMap(newState.workers)
                }
                return newState
            })
        },
        reset: () => {
            clearInterval(timer)
            seconds = 0
            set(defaultState)
        },
        start: () => {
            update(s => ({ ...s, running: true }))
            timer = setInterval(() => {
                update(s => {
                    progressTasks(s.tasks, s.workers)
                    tryAllocateTasks(s.workers, s.tasks)
                    return { ...s, time: formatTime(++seconds) }
                })
            }, SIM_SPEED.TICK_MS)
        },
        stop: () => {
            clearInterval(timer)
            update(s => ({ ...s, running: false }))
        }
    }
}

export const simStore = createStore()