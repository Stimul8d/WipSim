# WipSim

Proof that doing five things at once makes everything take longer. Watch your dev team's productivity tank in real-time as you pile on the work.

## What it Shows
- Live simulation of task flow
- Context switching kills productivity
- How WIP limits affect throughput
- The true cost of "urgent" interruptions

Built because PowerPoints about Little's Law are boring and managers need to feel the pain.

## Features
- Token-based task visualisation
- Real-time cycle/lead time tracking
- Context switch overhead simulation
- Stage-by-stage bottleneck detection

## Tech Stack
- SvelteKit (better than React)
- Pico CSS (life's too short)
- PNPM (because npm's having a mare)
- BDD with Jest-Cucumber

## Getting Started
```bash
# Dependencies
pnpm install

# Tests (BDD)
pnpm test

# Dev server
pnpm dev
```

## Testing
Gherkin specs in `tests/features`. Run with:
```bash
pnpm test
pnpm run check
```

## Deployment
Pushes to main trigger:
1. Run tests
2. Build static site
3. Deploy to GitHub Pages

## Prior Art
Inspired by a manager asking "Can't you just squeeze this quick one in?"