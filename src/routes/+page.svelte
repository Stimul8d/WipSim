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
        ...Array.from({ length: 20 }, (_, i) => ({
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
        <div class="control-panel">
            <details open>
                <summary>Team Setup</summary>
                <label>Engineers <input type="number" min="1" max="10" value="3"></label>
                <label>Max WIP <input type="number" min="1" max="5" value="2"></label>
            </details>

            <details open>
                <summary>Task Settings</summary>
                <label>
                    Arrival: <span>5m</span>
                    <input type="range" min="1" max="10" value="5">
                </label>
                <label>Size: <input type="number" min="1" max="10" value="3"></label>
            </details>

            <details open>
                <summary>Work Types</summary>
                <label><input type="checkbox" checked> Frontend</label>
                <label><input type="checkbox" checked> Backend</label>
                <label><input type="checkbox" checked> DevOps</label>
                <label><input type="checkbox" checked> Testing</label>
            </details>
        </div>
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

    <aside class="metrics">
        <div class="metrics-panel">
            <div class="metric">
                <span>Lead Time</span>
                <strong>4.2m</strong>
            </div>
            <div class="metric">
                <span>WIP</span>
                <strong>5</strong>
            </div>
            <div class="metric">
                <span>Done</span>
                <strong>12</strong>
            </div>
            <div class="metric">
                <span>Blocked</span>
                <strong>2</strong>
            </div>
        </div>
    </aside>
</main>

<style>
    main {
        display: grid;
        grid-template-columns: 250px 1fr 200px;
        height: calc(100vh - 53px);
        gap: 1rem;
        padding: 1rem;
    }

    .control-panel, .metrics-panel {
        background: var(--card-background-color);
        padding: 1rem;
        border-radius: var(--border-radius);
        height: 100%;
        overflow-y: auto;
    }

    details {
        margin-bottom: 1rem;
    }

    summary {
        font-weight: bold;
        margin-bottom: 0.5rem;
    }

    label {
        display: flex;
        align-items: center;
        gap: 0.5rem;
        margin: 0.25rem 0;
        font-size: 0.9rem;
    }

    input[type="number"] {
        width: 4rem;
        padding: 0.2rem;
        margin: 0;
    }

    input[type="range"] {
        margin: 0;
    }

    .metric {
        display: flex;
        justify-content: space-between;
        padding: 0.5rem 0;
        border-bottom: 1px solid var(--card-border-color);
    }

    .metric:last-child {
        border: none;
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