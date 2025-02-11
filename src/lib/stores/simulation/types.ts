export interface Params {
    tasks: any[]
    engineers: number
    maxWip: number
    arrivalRate: number
    taskSize: number
    workTypes: {
        frontend: boolean
        backend: boolean
        devops: boolean
        testing: boolean
    }
}