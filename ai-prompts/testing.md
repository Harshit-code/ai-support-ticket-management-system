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
