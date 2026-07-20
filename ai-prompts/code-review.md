# AI Prompts — Code Review

## Backend Sanity Check (pre-merge review)

**Prompt 1 — Full backend code review**
> "Backend is complete now. Test all the ticket and comment APIs and do a sanity check on your side."

AI reviewed all controllers, DI registrations, service/repository implementations, and DTO constraints against the API contract before the final merge commit.

### Findings

| # | File | Issue | Severity | Fixed |
|---|---|---|---|---|
| 1 | `CommentsController` / `CommentService` | `GET /api/tickets/{id}/comments` returned 200+empty array for non-existent tickets | Medium | ✅ |
| 2 | `CreateCommentRequest` | `MaxLength(5000)` vs `AppDbContext HasMaxLength(1000)` | High | ✅ |
| 3 | Controllers folder | `UsersController` missing despite being in api-contract | High | ✅ |
| 4 | `Program.cs` | No `db.Database.Migrate()` on startup — fresh clone had no DB | Medium | ✅ |
| 5 | `Program.cs` | No CORS configuration — frontend would be blocked | Medium | ✅ |

### What passed review

- `TicketsController` — all 5 actions correct, response codes match contract, validation working via `[ApiController]`
- `CommentsController POST` — ticket existence check in service, `KeyNotFoundException` mapped to 404, message trim applied
- `TicketService.TransitionStatusAsync` — state machine check correct, exception carries typed properties, service owns the logic
- `TicketRepository` — conditional `IQueryable` for search/filter is correctly built before `ToListAsync()`
- DI registrations in `Program.cs` — all repositories and services registered as `Scoped`
- `AppDbContext` — Fluent API configuration matches entity nav properties; seed data IDs are stable integers
- `JsonStringEnumConverter` + `ReferenceHandler.IgnoreCycles` — applied globally, no per-action or per-property workarounds needed
