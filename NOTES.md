# WipSim Development Notes

## Current State
- Basic layout sorted with three-panel design 
- Task visualisation using moving tokens rather than progress bars
- Controls and metrics panels independently scrollable
- Nav cleaned up, minimal header, good use of space

## Next Up
- Simulation store needs building
  - Task generation with configurable rates
  - Worker allocation with skill matching
  - Context switching penalties (make it proper brutal)
  - Time-based progression

- Key metrics to track
  - Lead time (from create to done)
  - WIP count per stage
  - Team utilisation
  - Context switch overhead

## Gotchas & Decisions
- Using pnpm (npm was having a mare with deps)
- Pico CSS for styling (keep it lean)
- GitHub Pages deployment sorted
- Proper fixed panels left/right
- Went with token movement vs progress bars (clearer flow)

## TODO
1. Simulation core
   - Task generation
   - Work allocation
   - State transitions
   - Time progression

2. Controls hookup
   - Team size/skills
   - WIP limits
   - Task generation rate
   - Work type toggles

3. Metrics display
   - Real-time updates
   - Historical trends?
   - Bottleneck highlighting

4. Polish
   - Colour coding for states
   - Tooltips for task details
   - Animation smoothing

## Questions for Next Session
- Add skill specialisation to workers?
- Show efficiency hit from context switching?
- Add dependencies between tasks?
- Track interruptions/urgent work?