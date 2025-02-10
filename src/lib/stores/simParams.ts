import { writable } from 'svelte/store'

export const simParamsStore = writable({
    tasks: []
})

export const resetParams = () => {
    simParamsStore.set({
        tasks: []
    })
}