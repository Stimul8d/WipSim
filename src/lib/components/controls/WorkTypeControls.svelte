<script lang="ts">
export let frontend = true;
export let backend = true;
export let devops = true;
export let testing = true;
</script>

<details open>
    <summary>Work Types</summary>
    <label><input type="checkbox" bind:checked={frontend}> Frontend</label>
    <label><input type="checkbox" bind:checked={backend}> Backend</label>
    <label><input type="checkbox" bind:checked={devops}> DevOps</label>
    <label><input type="checkbox" bind:checked={testing}> Testing</label>
</details>