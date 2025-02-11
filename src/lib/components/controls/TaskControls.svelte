<script lang="ts">
import { simStore } from '../../stores/simulation'

let arrivalRate: number
let taskSize: number

simStore.subscribe(state => {
    arrivalRate = state.arrivalRate
    taskSize = state.taskSize
})
</script>

<details open>
    <summary>Task Settings</summary>
    <label>
        Arrival: <span>{arrivalRate}m</span>
        <input type="range" min="1" max="10" bind:value={arrivalRate} on:change={() => simStore.update(s => ({...s, arrivalRate}))}>
    </label>
    <label>
        Size: <span>{taskSize}</span>
        <input type="range" min="1" max="10" bind:value={taskSize} on:change={() => simStore.update(s => ({...s, taskSize}))}>
    </label>
</details>