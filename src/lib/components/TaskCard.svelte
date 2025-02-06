<script lang="ts">
    import type { Task } from '$lib/types/core';
    
    export let task: Task;
</script>

<article class="task">
    <header>
        <strong>#{task.id}</strong>
        <span>{task.type}</span>
    </header>
    
    <div class="progress">
        <progress value={task.progress} max="100" />
        <small>{task.progress}%</small>
    </div>
    
    {#if task.assignedTo}
        <footer>
            <small>Assigned: {task.assignedTo.name}</small>
        </footer>
    {/if}
</article>

<style>
    .task {
        margin: 0;
        padding: 1rem;
    }
    
    header {
        display: flex;
        justify-content: space-between;
        margin-bottom: 0.5rem;
    }
    
    .progress {
        display: flex;
        align-items: center;
        gap: 0.5rem;
    }
    
    progress {
        flex: 1;
        height: 0.5rem;
    }
</style>