# Code Review Notes

## PR #2 — core domain model

### Issue found: business logic in wrong layer
**Problem:** Initial implementation placed `TransitionStatusAsync` (with state machine validation) inside `TicketRepository` (Infrastructure layer). Similarly, the terminal state check for comments was inside `CommentRepository`.

Repositories should be pure data access — no business logic.

**Expected:** All validation and business rules belong in the Service layer (Core.Services).

### Fix applied
Refactored in 3 follow-up commits on the same branch before merging:
1. Added service interfaces (`ITicketService`, `ICommentService`, `IUserService`) and implementations
2. Stripped repositories back to pure CRUD — `TransitionStatusAsync` replaced with dumb `UpdateStatusAsync`, terminal check removed from `CommentRepository`
3. Updated controllers to inject services instead of repositories; updated Program.cs DI registrations
