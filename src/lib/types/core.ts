import type { TaskStatus, TaskType } from './constants';

export interface Worker {
    id: string
    name: string
    skills: Skill[]
    currentTasks: string[]  // Just store task IDs
    efficiency: number 
    maxTasks: number
}

export interface Task {
    id: string
    type: TaskType
    status: TaskStatus
    progress: number
    complexity: number
    assignedTo?: string    // Store worker ID
}

export interface Skill {
    type: TaskType
    level: number // 1-5
}