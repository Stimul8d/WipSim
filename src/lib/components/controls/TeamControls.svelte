<script lang="ts">
import { simStore } from '../../stores/simulation'

let engineers: number
let maxWip: number
let startingTasks: number

simStore.subscribe(state => {
    engineers = state.engineers
    maxWip = state.maxWip
    startingTasks = state.startingTasks
})
</script>

<details open>
    <summary>Team Setup</summary>
    <label>
        Engineers: <span>{engineers}</span>
        <input type="range" min="1" max="10" bind:value={engineers} on:input={() => simStore.update(s => ({...s, engineers}))}>
    </label>
    <label>
        Max WIP: <span>{maxWip}</span>
        <input type="range" min="1" max="5" bind:value={maxWip} on:input={() => simStore.update(s => ({...s, maxWip}))}>
    </label>
    <label>
        Starting Tasks: <span>{startingTasks}</span>
        <input type="range" min="1" max="10" step="1" bind:value={startingTasks} on:input={() => simStore.update(s => ({...s, startingTasks}))}>
    </label>
</details>