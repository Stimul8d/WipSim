import { writable } from 'svelte/store'
import type { Params } from './types'
import { TaskStatus, TaskType as TT } from '../../types/constants'

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

const defaultParams: Params = {
    tasks: [generateTask(1, { frontend: true, backend: true, devops: true, testing: true })],
    engineers: 1,
    maxWip: 1,
    arrivalRate: 1,
    taskSize: 1,
    startingTasks: 1,
    workTypes: {
        frontend: true,
        backend: true,
        devops: true,
        testing: true
    }
}

function createStore() {
    const { subscribe, set, update } = writable<Params>(defaultParams)

    return {
        subscribe,
        set,
        update: (updater: (params: Params) => Params) => {
            update(state => {
                const newState = updater(state)
                // Always regenerate tasks if count changes
                newState.tasks = Array.from(
                    { length: newState.startingTasks }, 
                    (_, i) => generateTask(i + 1, newState.workTypes)
                )
                return newState
            })
        },
        reset: () => set(defaultParams)
    }
}

export const simStore = createStore()