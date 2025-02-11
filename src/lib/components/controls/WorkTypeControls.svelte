<script lang="ts">
import { simStore } from '../../stores/simulation'

let workTypes: { frontend: boolean, backend: boolean, devops: boolean, testing: boolean }

simStore.subscribe(state => {
    workTypes = state.workTypes
})

function updateWorkType(type: keyof typeof workTypes) {
    simStore.update(s => ({
        ...s, 
        workTypes: {...s.workTypes, [type]: !s.workTypes[type]}
    }))
}
</script>

<details open>
    <summary>Work Types</summary>
    <label><input type="checkbox" checked={workTypes?.frontend} on:change={() => updateWorkType('frontend')}> Frontend</label>
    <label><input type="checkbox" checked={workTypes?.backend} on:change={() => updateWorkType('backend')}> Backend</label>
    <label><input type="checkbox" checked={workTypes?.devops} on:change={() => updateWorkType('devops')}> DevOps</label>
    <label><input type="checkbox" checked={workTypes?.testing} on:change={() => updateWorkType('testing')}> Testing</label>
</details>