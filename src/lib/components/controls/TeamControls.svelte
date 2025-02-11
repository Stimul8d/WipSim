<script lang="ts">
import { simStore } from '../../stores/simulation'

let engineers: number
let maxWip: number

simStore.subscribe(state => {
    engineers = state.engineers
    maxWip = state.maxWip
})
</script>

<details open>
    <summary>Team Setup</summary>
    <label>
        Engineers: <span>{engineers}</span>
        <input type="range" min="1" max="10" bind:value={engineers} on:change={() => simStore.update(s => ({...s, engineers}))}>
    </label>
    <label>
        Max WIP: <span>{maxWip}</span>
        <input type="range" min="1" max="5" bind:value={maxWip} on:change={() => simStore.update(s => ({...s, maxWip}))}>
    </label>
</details>