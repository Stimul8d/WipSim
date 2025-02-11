import { writable } from 'svelte/store'
import type { Params } from './types'

const defaultParams: Params = {
    tasks: [],
    engineers: 1,
    maxWip: 1,
    arrivalRate: 1,
    taskSize: 1,
    workTypes: {
        frontend: true,
        backend: true,
        devops: true,
        testing: true
    }
}

export const store = writable<Params>(defaultParams)

export const reset = () => store.set(defaultParams)