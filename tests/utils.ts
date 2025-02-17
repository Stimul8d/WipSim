import type { SimState } from '../src/lib/stores/simulation/store'
import { TaskStatus } from '../src/lib/types/constants'

export function runAllocation(state: SimState): SimState {
    const workers = state.workers
    const tasks = state.tasks
    let allocated = true
    
    while (allocated) {
        const freeWorker = workers.find(w => w.currentTasks.length < w.maxTasks)
        const freeTask = tasks.find(t => !t.assignedTo && t.status === TaskStatus.BACKLOG)
        
        if (freeWorker && freeTask) {
            freeTask.assignedTo = freeWorker.id
            freeTask.status = TaskStatus.IN_PROGRESS
            freeTask.startedAt = state.simTicks
            freeWorker.currentTasks.push(freeTask.id)
        } else {
            allocated = false
        }
    }

    return { ...state, simTicks: state.simTicks + 1 }
}