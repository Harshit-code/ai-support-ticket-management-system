# Final AI Usage Summary

## Tool Used
Cursor (Claude Sonnet) — AI coding assistant used throughout the backend build.

## How AI was used

### Planning and architecture
- Used to validate the 3-layer architecture approach (Domain / Infrastructure / Api) and confirm the folder structure before writing any code.
- Prompted to review the initial entity design and state machine for gaps — identified the question around terminal state behaviour and comment blocking.

### Implementation
- Generated all entity, enum, interface, service, repository, controller, DTO, and frontend component files from scratch.
- Each file was reviewed before committing; AI caught its own errors (e.g. business logic in repositories in the first pass) and refactored when corrected.
- Frontend (React + TypeScript + Tailwind): scaffolded Vite project, implemented all pages (TicketListPage, TicketDetailPage, CreateTicketPage), shared components, API layer, and status transition mirroring.

### Design decisions
- Used to discuss HTTP status codes (409 vs 400 for invalid transitions) — AI presented both options with reasoning, final call was made by the developer.
- Used to design the `InvalidTransitionException` shape (typed properties vs plain message string).

### Debugging and sanity checking
- Final backend sanity check caught 4 bugs: missing `UsersController`, `MaxLength` mismatch, GET comments returning 200 for non-existent tickets, no auto-migration.
- Port mismatch between `launchSettings.json` (5000) and `client.ts` (5020) caught after frontend showed permanent loading spinner.

### Testing
- Generated 32 pure unit tests for `TicketStatusTransitions` static class covering all 25 from→to combinations.
- Generated 30 controller unit tests mocking `ITicketService` with Moq (`MockBehavior.Strict`).
- All 62 tests pass with zero warnings.

### Documentation
- AI drafted all `.md` files and kept them updated on the go throughout all phases.

## What AI did well
- Followed the established architecture consistently across all features.
- Generated clean, compilable code with 0 build errors on each iteration.
- Proactively caught the comment MaxLength mismatch and missing controller during review.
- Mirrored the backend state machine exactly on the frontend to keep both in sync.
- Produced structured documentation that accurately reflects the implementation.

## What required human direction
- Identifying the 409 vs 400 trade-off (AI gave both options, developer chose).
- Deciding that comment blocking on terminal tickets would be deferred (scoping call).
- Confirming each feature was complete before the next one was started.
- Approving all commits and pushes — no autonomous execution of destructive commands.
- Identifying the port mismatch from a browser screenshot (AI could not observe the running process).
