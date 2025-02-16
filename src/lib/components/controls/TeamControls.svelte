<script lang="ts">
import { simStore } from '../../stores/simulation'

let workerCount: number
let maxWip: number
let startingTasks: number

simStore.subscribe(state => {
    workerCount = state.workers.length
    maxWip = state.maxWip
    startingTasks = state.startingTasks
})
</script>

<details open>
    <summary>Team Setup</summary>
    <label>
        Workers: <span>{workerCount}</span>
        <input type="range" min="1" max="10" bind:value={workerCount} on:input={() => simStore.update(s => ({...s, workers: Array.from({length: workerCount}, (_, i) => ({ id: `ENG-${i+1}`, name: `Engineer ${i+1}`, currentTasks: [], efficiency: 1, maxTasks: 1, skills: [] }))}))} />
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