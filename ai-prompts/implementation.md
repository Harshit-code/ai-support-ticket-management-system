# AI Prompts — Implementation

## Feature: Core Domain Model

**Prompt 1 — Entity design review**
> "entities are user which is seeded only, ticket with id title description priority status assignedTo createdBy createdAt updatedAt and comment with id ticketId message createdBy createdAt — look for anything missing especially around the state machine"

Used to identify gap around Resolved→InProgress and comment behaviour on terminal tickets.

**Prompt 2 — State machine lock-in**
> Confirmed final allowed transitions, terminal states, comment blocking on Closed/Cancelled, assignedTo nullable.

**Prompt 3 — Implementation**
> Full core domain model implemented: enums, entities, TicketStatusTransitions, interfaces, DbContext with seeding, repositories, controllers.

**Prompt 4 — Solution scaffold**
> "Scaffold the .NET solution inside the src folder — SupportTickets.Api, SupportTickets.Domain, SupportTickets.Infrastructure. Wire project references, use SQLite, verify dotnet build."

**Prompt 5 — Swagger setup**
> "Setup Swagger for testing the APIs."
> Configured AddSwaggerGen with API info, XML docs, Swashbuckle.Annotations, request duration display.

---

## Feature: EF Core Migration and Seed Data

**Prompt 6 — Migration and seed data**
> "Create the first EF Core migration and add some seed data — a few users with different roles, a couple of sample tickets in different statuses. Run the migration on a local SQLite database."

Output: `InitialCreate` migration generated and applied. Seed data: 4 users (1 Admin, 2 Agents, 1 Customer), 3 tickets (Open, InProgress, Resolved), 2 comments.

---

## Feature: Ticket API Endpoints

**Prompt 7 — Ticket CRUD endpoints**
> "Add the ticket API endpoints — POST to create, GET all, GET by id, PUT to update title/description/priority/assignee. Don't allow status to be updated. Add proper validation — title and description required, priority only valid values, return 400 with field-specific errors."

Output: `TicketsController` with 4 actions. `CreateTicketRequest` and `UpdateTicketRequest` DTOs with DataAnnotations. `[ApiController]` auto-returns 400 `ValidationProblemDetails`. `ITicketRepository`, `ITicketService`, `TicketService`, `TicketRepository` created. `JsonStringEnumConverter` + `ReferenceHandler.IgnoreCycles` added globally in Program.cs.

---

## Feature: Status Transition Endpoint

**Prompt 8 — PATCH status endpoint**
> "Add the status update endpoint. Define all allowed transitions in one map. Make the service check the map before changing status. If someone tries an invalid transition, throw a custom InvalidTransitionException instead of a normal exception."

Output: `TicketStatusTransitions` static class (single source of truth). `InvalidTransitionException` with `From`, `To`, `Allowed` properties. `ITicketService.TransitionStatusAsync` + `ITicketRepository.UpdateStatusAsync`. `PATCH /api/tickets/{id}/status` catches `InvalidTransitionException` and returns structured response.

**Prompt 9 — Improve exception message**
> "The exception message is too generic. Make it include the current status, the status it was trying to change to, and the list of valid statuses."

Output: `BuildMessage` helper added to `InvalidTransitionException`. Message now reads: `"Cannot transition from 'X' to 'Y'. Valid transitions from 'X': Z."` Terminal states show `"none — this is a terminal state"`.

**Prompt 10 — 409 vs 400 for transition errors**
> "For an invalid status transition what HTTP status code makes more sense, 400 or 409?"

Decision: 409 Conflict. Rationale: a well-formed request rejected due to server resource state is a conflict, not a bad request. 400 is reserved for DTO validation failures caught by `[ApiController]` before the action body runs. Changed `BadRequest()` to `Conflict()` in the controller.

---

## Feature: Comment Endpoints

**Prompt 11 — Comment endpoints**
> "Add the comment endpoints — POST to add a comment to a ticket and GET to fetch all comments for a ticket. Make sure the ticket exists before adding — return 404 if not. Comment message required, no empty text."

Output: `ICommentRepository`, `ICommentService`, `CommentService`, `CommentRepository`. `CreateCommentRequest` with `[Required]` + `[MinLength(1)]`. `CommentsController` with `GET` and `POST` at `api/tickets/{ticketId}/comments`. `KeyNotFoundException` from service caught in controller → 404. Message `.Trim()` applied before save.

Note: blocking comments on Closed/Cancelled tickets was in the original design but not implemented in this prompt — tracked as a known gap.

---

## Feature: Search and Filtering

**Prompt 12 — Keyword search and status filter on GET /api/tickets**
> "Add search and filtering to the ticket list endpoint — keyword search on title and description, status filter. Both as query params. Work together if both provided. Keep it on the existing GET endpoint."

Output: `GetAllAsync(string? keyword, TicketStatus? status)` signature added to `ITicketRepository` and `ITicketService`. Conditional `IQueryable` in `TicketRepository` — `Contains` for keyword (EF Core translates to SQL LIKE, case-insensitive in SQLite), exact enum match for status. Controller `GetAll` action now accepts `[FromQuery] string? keyword` and `[FromQuery] TicketStatus? status`. Both params are optional; omitting both returns all tickets.

---

## Feature: Backend Completion and Sanity Check

**Prompt 13 — Backend README and sanity check**
> "Backend is complete now. Add the relevant backend README architecture design file, test all the ticket and comment APIs, do a sanity check, then update the relevant md files."

Sanity check output — 4 bugs found and fixed:
1. `GET /api/tickets/{id}/comments` returned `200 []` for non-existent ticket. Fixed: ticket existence check added to `CommentService.GetByTicketIdAsync`.
2. `CreateCommentRequest` had `MaxLength(5000)` but `AppDbContext` enforces `HasMaxLength(1000)`. Fixed: aligned DTO to `MaxLength(1000)`.
3. `UsersController` was missing despite being documented in `api-contract.md`. Fixed: added `IUserRepository`, `UserRepository`, `UsersController` (`GET /api/users`, `GET /api/users/{id}`).
4. No auto-migration on startup. Fixed: `db.Database.Migrate()` added in `Program.cs`.
5. No CORS. Fixed: `WithOrigins("http://localhost:5173")` added for frontend dev server.

New files: `src/README.md` (setup + architecture + endpoint list), `test-strategy.md` (27 manual test cases), `test-results.md` (blank table ready to fill), `pr-description.md`, `reflection.md`, `final-ai-usage-summary.md`.

**Prompt 14 — Update remaining ai-prompts files**
> "Also the ai-prompt relevant md files that also need updating."

Created missing files: `ai-prompts/testing.md`, `ai-prompts/debugging.md`, `ai-prompts/code-review.md`, `ai-prompts/documentation.md`. Updated `ai-prompts/implementation.md` with Prompts 13–14.

---

## Feature: Frontend Core

**Prompt 15 — Frontend scaffold and all pages**
> "Start working on the frontend now. I need a ticket list page with search and status filter, a ticket details page showing ticket info and comments, a create ticket page. On the details page add a status dropdown that only shows valid next statuses based on the same transition rules as the backend. Every page must handle loading, empty, and error states."

Output:
- Vite + React 18 + TypeScript + Tailwind CSS scaffolded in `frontend/`
- `frontend/src/types/index.ts` — `User`, `Ticket`, `Comment`, enums, `CreateTicketPayload`, `UpdateTicketPayload`, `ApiError`
- `frontend/src/utils/statusTransitions.ts` — `STATUS_TRANSITIONS` map, `getAllowedTransitions`, `isTerminal` mirroring backend exactly
- `frontend/src/api/client.ts` — Axios instance with `baseURL: http://localhost:5020/api`
- `frontend/src/api/tickets.ts`, `comments.ts`, `users.ts` — typed API calls
- Shared components: `StatusBadge`, `PriorityBadge`, `LoadingSpinner`, `ErrorMessage`, `EmptyState`
- `TicketListPage` — debounced keyword search + status filter dropdown; loading/empty/error states; links to detail and create
- `TicketDetailPage` — ticket info, comment thread, add comment form, dynamic status transition buttons (only valid next states)
- `CreateTicketPage` — form with client + server-side validation, user dropdowns from API
- React Router v6 routes: `/`, `/tickets/new`, `/tickets/:id`
- TypeScript build: 0 errors

**Prompt 16 — Frontend structured error display**
> "Right now when the API rejects an invalid status change the frontend just shows a generic toast. Update it to display the actual error message coming back from the backend so the user knows exactly why the transition failed."

Output:
- `TransitionError` interface added to `types/index.ts` with `message: string` and `allowed: string[]`
- `transitionError` state in `TicketDetailPage` changed from `string | null` to `TransitionError | null`
- `axios.isAxiosError` guard in `catch` block safely extracts `error` and `allowed` from `err.response.data`
- Error block now renders the message text plus each allowed status as a styled badge
- Users see e.g. "Cannot transition from 'Closed' to 'Open'. Valid transitions: none — this is a terminal state."

---

## Feature: Unit Tests

**Prompt 17 — Status transition unit tests with mocked service layer**
> "I need to write integration tests for the status transition logic. First list every possible from→to combination, mark which are valid/invalid. Then write the tests mocking the service layer."

Output:
- `tests/SupportTickets.Api.Tests/` project created (xUnit 2.9.2 + Moq 4.20.72, net8.0)
- Added to `src/SupportTickets.sln`
- `StateMachine/TicketStatusTransitionsTests.cs` — 32 pure tests covering all 25 from→to combinations + `GetAllowed` correctness per state
- `Controllers/TicketStatusTransitionControllerTests.cs` — 30 controller tests with `Mock<ITicketService>(MockBehavior.Strict)`
  - All 5 valid transitions → 200 OK with updated ticket
  - All 20 invalid combinations → 409 Conflict
  - Structured 409 body verified via reflection (anonymous object)
  - Terminal states → 409 with empty `allowed` list
  - Ticket not found → 404 with error body
  - Service called exactly once per valid request
- All 62 tests pass, zero warnings
