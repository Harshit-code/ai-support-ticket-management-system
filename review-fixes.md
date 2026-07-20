# Review Fixes

## PR #2 — core domain model

### Fix: service layer added, business logic moved out of repositories

**What was wrong:** `TicketRepository.TransitionStatusAsync` contained state machine validation. `CommentRepository.CreateAsync` contained terminal status check.

**What was fixed:**
- `Core/Services/TicketService.cs` — `TransitionStatusAsync` now lives here; validates transition via `TicketStatusTransitions.IsValid`, throws `InvalidOperationException` on invalid transition
- `Core/Services/CommentService.cs` — terminal state check (`IsTerminal`) lives here; loads ticket to verify status before delegating to repository
- `Infrastructure/Repositories/TicketRepository.cs` — replaced `TransitionStatusAsync` with `UpdateStatusAsync` (no validation, just sets status + updatedAt)
- `Infrastructure/Repositories/CommentRepository.cs` — pure insert, no business logic
- `Api/Controllers/*.cs` — all controllers now inject `ITicketService`, `ICommentService`, `IUserService` instead of repositories
- `Api/Program.cs` — services registered alongside repositories in DI
