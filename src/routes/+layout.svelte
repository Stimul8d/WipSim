<script lang="ts">
    import '@picocss/pico'
    import { simStore } from '$lib/stores/simulation'

    let time = '00:00'
    let running = false

    simStore.subscribe(state => {
        running = state.running
        time = state.time
    })
    
    function toggleRunning() {
        if (running) {
            simStore.stop()
        } else {
            simStore.start()
        }
    }

    function reset() {
        simStore.reset()
    }
</script>

<nav class="app-header">
    <ul>
        <li><strong>WipSim</strong></li>
        <li><button on:click={toggleRunning}>{running ? 'Stop' : 'Start'}</button></li>
        <li><button on:click={reset}>Reset</button></li>
        <li><span id="time">{time}</span></li>
    </ul>
    <ul>
        <li>Tasks: <strong>23</strong></li>
        <li>WIP: <strong>5</strong></li>
        <li>Done: <strong>12</strong></li>
        <li>Cycle Time: <strong>4.2m</strong></li>
    </ul>
</nav>

<slot />

<style>
    .app-header {
        padding: 0.5rem 1rem;
        margin-bottom: 0;
        background: var(--card-background-color);
    }

    .app-header ul {
        margin: 0;
    }

    .app-header li {
        margin: 0 1rem 0 0;
    }

    #time {
        font-family: monospace;
        font-size: 1.2rem;
    }

    button {
        margin: 0;
        padding: 0.2rem 1rem;
    }
</style>