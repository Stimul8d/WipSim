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

export enum TaskType {
    FRONTEND = 'Frontend',
    BACKEND = 'Backend',
    DEVOPS = 'DevOps',
    QA = 'Testing'
}

export enum TaskStatus {
    BACKLOG = 'Backlog',
    IN_PROGRESS = 'In Progress',
    BLOCKED = 'Blocked',
    DONE = 'Done'
}

export interface Skill {
    type: TaskType
    level: number // 1-5
}