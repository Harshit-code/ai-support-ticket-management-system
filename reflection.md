# Reflection

## What went well

**Architecture held up cleanly.** The 3-layer split (Domain / Infrastructure / Api) made it easy to add features without touching unrelated code. Adding search/filter to the ticket list was a change in 4 files with zero side effects.

**State machine as a static class worked well.** Having `TicketStatusTransitions` as the single source of truth meant the service only had to call `IsAllowed(from, to)` — no scattered if/switch chains across the codebase.

**Custom exception with structured data paid off.** `InvalidTransitionException` carrying `From`, `To`, and `Allowed` as typed properties meant the controller could forward a rich error response without any extra querying or string parsing.

**On-the-go documentation.** Keeping `api-contract.md`, `implementation-plan.md`, and `ai-prompts/` updated alongside each feature commit made it easy to audit progress and catch gaps.

## What could have been better

**Comment blocking on terminal tickets was missed.** The original design required blocking new comments on Closed/Cancelled tickets. This rule was in `requirements-analysis.md` but not caught until the sanity check. It is tracked as a known gap.

**DTO/DB constraint mismatch slipped through.** `CreateCommentRequest` had `MaxLength(5000)` while `AppDbContext` enforced `HasMaxLength(1000)`. This would have caused DB errors at runtime for messages over 1000 chars. Caught during backend sanity check.

**`UsersController` was missing.** The API contract documented user endpoints from the start but they weren't implemented until the sanity check before the final merge.

**No auto-migration initially.** Fresh clones would have had no DB until `dotnet ef database update` was run manually. Fixed by adding `db.Database.Migrate()` on startup.

## Key technical decisions made

| Decision | Choice | Reason |
|---|---|---|
| Business logic location | Domain services | Controllers stay thin, testable without HTTP |
| Status transition HTTP code | 409 Conflict | Request is valid; server state disagrees |
| Exception design | Typed properties on custom exception | Controller gets structured data, not just a message string |
| Enum serialization | `JsonStringEnumConverter` globally | Readable in Swagger and for the frontend |
| Cycle handling | `ReferenceHandler.IgnoreCycles` | Avoids `[JsonIgnore]` on every nav property |
| DB | SQLite with EF Core migrations | Zero setup locally, swap connection string for production |
