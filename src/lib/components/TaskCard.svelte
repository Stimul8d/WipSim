<script lang="ts">
import type { Task, Worker } from '../types/core'
import { TaskStatus } from '../types/constants'
import { simStore } from '../stores/simulation'

export let task: Task

let workerMap: Map<string, Worker>
simStore.subscribe(state => workerMap = state.workerMap)
</script>

<article class:blocked={task.status === TaskStatus.BLOCKED}>
    <header>
        <strong>{task.id}</strong>
        <span>{task.type}</span>
    </header>
    <progress value={task.progress} max="100" />
    <footer>
        <small>Assigned: {task.assignedTo ? workerMap.get(task.assignedTo)?.name : '-'}</small>
    </footer>
</article>

<style>
    article {
        margin: 0;
        padding: 0.5rem;
    }
    article.blocked {
        border-color: var(--del-color);
    }
    header {
        display: flex;
        justify-content: space-between;
        margin-bottom: 0.5rem;
    }
    progress {
        width: 100%;
        height: 4px;
        margin-bottom: 0.5rem;
    }
    footer {
        font-size: 0.8rem;
        color: var(--muted-color);
    }
</style>