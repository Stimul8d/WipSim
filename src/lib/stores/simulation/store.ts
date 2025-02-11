import { writable } from 'svelte/store'
import type { Params } from './types'
import { TaskType, TaskStatus } from '../../types/constants'

const defaultParams: Params = {
    tasks: [{
        id: 'TASK-1',
        type: TaskType.FRONTEND,
        status: TaskStatus.BACKLOG,
        progress: 0,
        complexity: 1
    }],
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
        update,
        reset: () => set(defaultParams)
    }
}

export const simStore = createStore()