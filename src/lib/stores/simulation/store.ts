import { writable } from 'svelte/store'
import type { Params } from './types'

const defaultParams: Params = {
    tasks: []
}

export const store = writable<Params>(defaultParams)

export const reset = () => store.set(defaultParams)