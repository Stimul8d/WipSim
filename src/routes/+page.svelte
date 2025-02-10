<script lang="ts">
    import type { Task } from '$lib/types/core';
    import { TaskStatus } from '$lib/types/constants';
    import { simStore } from '$lib/stores/simulation';
    import { TeamControls, TaskControls, WorkTypeControls } from '$lib/components/controls';
    
    let tasks: Task[] = [];
    simStore.subscribe(state => {
        tasks = state.tasks;
    });

    function getStagePosition(task: Task): number {
        return Math.min(Math.floor(task.progress / 20), 4);
    }
</script>

<main>
    <aside class="controls">
        <div class="control-panel">
            <TeamControls />
            <TaskControls />
            <WorkTypeControls />
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