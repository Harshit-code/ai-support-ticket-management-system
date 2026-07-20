# AI Prompts — Testing

## Backend Manual Testing

**Prompt 1 — Test strategy generation**
> "Backend is complete. Add a backend README, test all the ticket and comment API, do a sanity check."

Used to generate the full manual test case list in `test-strategy.md`. AI produced 27 test cases covering:
- Users: GET all, GET by id, GET non-existent (404)
- Tickets: all happy-path flows, keyword+status filter combinations, validation failures (400), not found (404), invalid status transitions (409), terminal state transitions
- Comments: happy path, ticket not found on both GET and POST (404), empty/missing message (400)

**Prompt 2 — Test results template**
Generated `test-results.md` as a blank pass/fail table aligned to test-strategy.md cases. To be filled in manually by running `dotnet run` and executing each case through Swagger UI at `http://localhost:5020/swagger`.

---

## Unit Tests — Status Transition Logic

**Prompt 3 — Transition table then automated tests**
> "I need to write integration tests for the status transition logic but first list every possible from→to combination in a table and mark which are valid/invalid. Then mocking the service layer."

First produced the full 25-row table (5 valid, 20 invalid). Then created the xUnit + Moq test project.

**State machine tests** (`TicketStatusTransitionsTests`) — 32 tests, no mocks:
- 5 valid transitions (`IsAllowed` returns true)
- 5 same-state transitions (false)
- 3 backward transitions (false)
- 3 forward skips (false)
- 1 Resolved→Cancelled edge case (false)
- 10 transitions from terminal states Closed/Cancelled (always false)
- `GetAllowed` correctness: Open→{InProgress,Cancelled}, InProgress→{Resolved,Cancelled}, Resolved→{Closed}, Closed→{}, Cancelled→{}

**Controller tests** (`TicketStatusTransitionControllerTests`) — 30 tests, `ITicketService` mocked:
- Valid transitions → 200 OK
- Invalid transitions → 409 Conflict (all 20 cases)
- Structured error body verified (reflection on anonymous object)
- Terminal state → 409 with empty `allowed`
- Not found → 404
- Service called exactly once

All 62 tests pass, zero warnings. Run with: `dotnet test tests/SupportTickets.Api.Tests`
