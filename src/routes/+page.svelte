<script lang="ts">
    import type { Task } from '$lib/types/core';
    import { TaskStatus, TaskType } from '$lib/types/constants';
    
    let tasks: Task[] = [
        {
            id: "PRJ-123",
            type: TaskType.FRONTEND,
            status: TaskStatus.BACKLOG,
            progress: 20,
            complexity: 3,
            assignedTo: { id: "1", name: "Dave", skills: [], currentTasks: [], efficiency: 1, maxTasks: 2 }
        },
        ...Array.from({ length: 10 }, (_, i) => ({
            id: `PRJ-${i + 124}`,
            type: Object.values(TaskType)[Math.floor(Math.random() * 4)],
            status: Object.values(TaskStatus)[Math.floor(Math.random() * 4)],
            progress: Math.floor(Math.random() * 100),
            complexity: Math.floor(Math.random() * 5) + 1,
            assignedTo: Math.random() > 0.5 ? { 
                id: String(Math.floor(Math.random() * 3) + 1), 
                name: ["Dave", "Alice", "Bob"][Math.floor(Math.random() * 3)], 
                skills: [], 
                currentTasks: [], 
                efficiency: 1, 
                maxTasks: 2 
            } : undefined
        }))
    ];

    function getStagePosition(task: Task): number {
        return Math.min(Math.floor(task.progress / 20), 4);
    }
</script>

<main>
    <aside class="controls">
        <section>
            <h3>Team</h3>
            <label>
                Engineers
                <input type="number" min="1" max="10" value="3">
            </label>
            <label>
                Max WIP per dev
                <input type="number" min="1" max="5" value="2">
            </label>
        </section>

        <section>
            <h3>Task Generation</h3>
            <label>
                New task every
                <input type="range" min="1" max="10" value="5">
                <small>5 minutes</small>
            </label>
            <label>
                Complexity range
                <div class="grid">
                    <input type="number" min="1" max="10" value="1">
                    <input type="number" min="1" max="10" value="5">
                </div>
            </label>
        </section>

        <section>
            <h3>Work Types</h3>
            {#each Object.values(TaskType) as type}
                <label>
                    <input type="checkbox" checked>
                    {type}
                </label>
            {/each}
        </section>
    </aside>

    <div class="tasks">
        <table role="grid">
            <thead>
                <tr>
                    <th>Task</th>
                    <th>Type</th>
                    <th>Owner</th>
                    <th class="stage-col">New</th>
                    <th class="stage-col">Planning</th>
                    <th class="stage-col">Dev</th>
                    <th class="stage-col">Test</th>
                    <th class="stage-col">Done</th>
                    <th>Age</th>
                </tr>
            </thead>
            <tbody>
                {#each tasks as task}
                    <tr>
                        <td class="mono">{task.id}</td>
                        <td>{task.type}</td>
                        <td>{task.assignedTo?.name || '-'}</td>
                        
                        {#each Array(5) as _, i}
                            <td class="stage-col">
                                {#if getStagePosition(task) === i}
                                    <div class="work-token" class:blocked={task.status === TaskStatus.BLOCKED} />
                                {/if}
                            </td>
                        {/each}
                        
                        <td class="mono">2m</td>
                    </tr>
                {/each}
            </tbody>
        </table>
    </div>
</main>

<style>
    main {
        display: grid;
        grid-template-columns: 250px 1fr;
        height: calc(100vh - 53px);
        gap: 1rem;
        padding: 1rem;
    }

    .controls {
        background: var(--card-background-color);
        padding: 1rem;
        border-radius: var(--border-radius);
    }

    .controls section {
        margin-bottom: 2rem;
    }

    .controls h3 {
        margin: 0 0 1rem 0;
        font-size: 1rem;
    }

    .controls label {
        margin: 0 0 0.5rem 0;
    }

    .tasks {
        overflow-y: auto;
    }

    table {
        margin: 0;
    }

    td, th {
        padding: 0.5rem;
        white-space: nowrap;
    }

    .stage-col {
        width: 100px;
        padding: 0.5rem !important;
        text-align: center;
        border-left: 1px solid var(--card-border-color);
    }

    .work-token {
        width: 16px;
        height: 16px;
        border-radius: 3px;
        margin: 0 auto;
        background: var(--primary);
    }

    .work-token.blocked {
        background: var(--del-color);
    }

    .mono {
        font-family: monospace;
    }
</style>