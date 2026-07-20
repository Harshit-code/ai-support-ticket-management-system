# Final AI Usage Summary

## Tool Used
Cursor (Claude Sonnet) — AI coding assistant used throughout the backend build.

## How AI was used

### Planning and architecture
- Used to validate the 3-layer architecture approach (Domain / Infrastructure / Api) and confirm the folder structure before writing any code.
- Prompted to review the initial entity design and state machine for gaps — identified the question around terminal state behaviour and comment blocking.

### Implementation
- Generated all entity, enum, interface, service, repository, and controller files from scratch.
- Each file was reviewed before committing; AI caught its own errors (e.g. business logic in repositories in the first pass) and refactored when corrected.

### Design decisions
- Used to discuss HTTP status codes (409 vs 400 for invalid transitions) — AI presented both options with reasoning, final call was made by the developer.
- Used to design the `InvalidTransitionException` shape (typed properties vs plain message string).

### Debugging and sanity checking
- Final sanity check before merge caught 4 bugs: missing `UsersController`, `MaxLength` mismatch, GET comments returning 200 for non-existent tickets, no auto-migration.

### Documentation
- AI drafted all `.md` files (`api-contract.md`, `design-notes.md`, `implementation-plan.md`, `requirements-analysis.md`, `test-strategy.md`, `reflection.md`, `pr-description.md`, `ai-prompts/*.md`) and kept them updated on the go.

## What AI did well
- Followed the established architecture consistently across all features.
- Generated clean, compilable code with 0 build errors on each iteration.
- Proactively caught the comment MaxLength mismatch and missing controller during review.
- Produced structured documentation that accurately reflects the implementation.

## What required human direction
- Identifying the 409 vs 400 trade-off (AI gave both options, developer chose).
- Deciding that comment blocking on terminal tickets would be deferred (scoping call).
- Confirming each feature was complete before the next one was started.
- Approving all commits and pushes — no autonomous execution of destructive commands.
