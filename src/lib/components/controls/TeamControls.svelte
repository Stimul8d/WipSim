<script lang="ts">
export let engineers = 3;
export let maxWip = 2;
</script>

<details open>
    <summary>Team Setup</summary>
    <label>
        Engineers: <span>{engineers}</span>
        <input type="range" min="1" max="10" bind:value={engineers}>
    </label>
    <label>
        Max WIP: <span>{maxWip}</span>
        <input type="range" min="1" max="5" bind:value={maxWip}>
    </label>
</details>