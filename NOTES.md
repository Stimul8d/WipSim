# WipSim Dev Notes

## Done
- Three panel layout works 
- Parameter store synced
- Token movement transitions
- Base task allocation (grab next free)
- Lead/cycle time tracking (bit wonky)

## Next Up
- Fix task timing (500 error in tests)
- Add task stage transitions
- Hook up metrics panel
- Proper task limits per stage

## Issues
- Task timing calc needs sorting (started vs created)
- UI doesn't show stage transitions clearly
- No bottleneck viz yet

## Questions
- Show efficiency hit from context switching?
- Track interruptions/urgent work?
- Add task dependencies?