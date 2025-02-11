# WipSim

Proof that doing five things at once makes everything take longer. Watch your dev team's productivity tank in real-time as you pile on the work.

## What it Shows

- Live simulation of work flowing (or not) through dev stages
- Context switching penalties (because your brain isn't a CPU)
- Impact of WIP limits on throughput and lead time
- Why your "urgent" ticket is making everything slower

Built because PowerPoints about Little's Law are boring and managers need to feel the pain.

## Tech Stack

- SvelteKit (because React's chunky)
- Pico CSS (because life's too short)
- PNPM (because npm's having a mare)
- Vitest + Jest-Cucumber for BDD

## Getting Started

```bash
# Grab dependencies
pnpm install

# Run tests
pnpm test

# Start dev server
pnpm dev
```

## Testing

We use BDD with Gherkin syntax. Tests live in `tests/features`.

```bash
# Run all tests
pnpm test

# Check types and linting
pnpm run check
```

## Deployment

Pushes to main trigger GitHub Actions to:
1. Run tests
2. Build static site
3. Deploy to GitHub Pages

## Contributing

The irony of having multiple PRs in progress is not lost on us.

## Prior Art

Inspired by every project manager who's ever said "Can't you just squeeze this quick one in?"