<script lang="ts">
export let arrivalRate = 5;
export let taskSize = 3;
</script>

<details open>
    <summary>Task Settings</summary>
    <label>
        Arrival: <span>{arrivalRate}m</span>
        <input type="range" min="1" max="10" bind:value={arrivalRate}>
    </label>
    <label>
        Size: <span>{taskSize}</span>
        <input type="range" min="1" max="10" bind:value={taskSize}>
    </label>
</details>