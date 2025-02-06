import type { TaskStatus, TaskType } from './constants';

export interface Worker {
    id: string
    name: string
    skills: Skill[]
    currentTasks: Task[]
    efficiency: number 
    maxTasks: number
}

export interface Task {
    id: string
    type: TaskType
    status: TaskStatus
    progress: number
    complexity: number
    assignedTo?: Worker
    startedAt?: Date
    blockedBy?: Task[]
}

export interface Skill {
    type: TaskType
    level: number // 1-5
}

// Re-export for convenience
export type { TaskStatus, TaskType };