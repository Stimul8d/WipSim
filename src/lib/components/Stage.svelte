<script lang="ts">
    import type { Task } from '$lib/types/core';
    import TaskCard from './TaskCard.svelte';
    
    export let title: string;
    export let tasks: Task[] = [];
    export let wipLimit: number;
</script>

<section class="stage">
    <header>
        <h3>{title}</h3>
        <span class="wip-count">{tasks.length}/{wipLimit}</span>
    </header>
    
    <div class="tasks" class:overload={tasks.length > wipLimit}>
        {#each tasks as task (task.id)}
            <TaskCard {task} />
        {/each}
    </div>
</section>

<style>
    .stage {
        background: var(--card-background-color);
        border-radius: var(--border-radius);
        padding: 1rem;
        height: 100%;
    }
    
    header {
        display: flex;
        justify-content: space-between;
        align-items: baseline;
        margin-bottom: 1rem;
    }
    
    h3 {
        margin: 0;
    }
    
    .wip-count {
        font-size: 0.875rem;
        opacity: 0.8;
    }
    
    .tasks {
        display: flex;
        flex-direction: column;
        gap: 0.5rem;
    }
    
    .overload {
        background: var(--card-sectionning-background-color);
    }
</style>