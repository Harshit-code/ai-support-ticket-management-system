# PR Description

## Full Project — feature/frontend-core → main

### Summary

Completes the AI Support Ticket Management System end-to-end: backend API, React frontend, and automated unit tests. All features are implemented, tested, and verified working locally.

---

### What's included

**Backend — Ticket endpoints**
- `GET /api/tickets` — list all tickets with optional `?keyword=` and `?status=` query filters
- `GET /api/tickets/{id}` — get ticket with comments included
- `POST /api/tickets` — create ticket (status forced to `Open`, validated)
- `PUT /api/tickets/{id}` — update title, description, priority, assignee (status excluded)
- `PATCH /api/tickets/{id}/status` — transition status via enforced state machine; returns 409 Conflict on invalid transitions

**Backend — Comment endpoints**
- `GET /api/tickets/{ticketId}/comments` — list comments ordered oldest first (404 if ticket missing)
- `POST /api/tickets/{ticketId}/comments` — add comment (404 if ticket missing, message required + non-empty)

**Backend — User endpoints**
- `GET /api/users` — list seeded users
- `GET /api/users/{id}` — get user by id

**Backend — Infrastructure**
- EF Core SQLite with `InitialCreate` migration and seed data (4 users, 3 tickets, 2 comments)
- Auto-migration on startup — no manual `dotnet ef database update` needed
- Swagger UI at `/swagger`
- CORS configured for Vite dev server (`http://localhost:5173`)

**Frontend (React + TypeScript + Tailwind)**
- `TicketListPage` — keyword search + status filter, loading/empty/error states
- `TicketDetailPage` — ticket info, comment thread, add comment form, dynamic status transition buttons
- `CreateTicketPage` — form with client-side and server-side validation
- Structured 409 error display on invalid status transition — message + allowed transitions as badges
- Frontend state machine mirrors backend `TicketStatusTransitions` exactly

**Unit Tests (xUnit + Moq)**
- `TicketStatusTransitionsTests` — 32 pure tests covering all 25 from→to combinations
- `TicketStatusTransitionControllerTests` — 30 controller tests with `ITicketService` mocked
- 62 tests total, all passing

---

### Architecture decisions
- Business logic (status transition validation, ticket existence checks) lives in Domain services
- Repositories are pure data access with no logic
- `InvalidTransitionException` carries `From`, `To`, `Allowed` for structured error responses
- Invalid transitions return `409 Conflict` — request is well-formed, server state disagrees
- `JsonStringEnumConverter` + `ReferenceHandler.IgnoreCycles` applied globally
- Frontend status transition map mirrors backend — both updated in one place if rules change

### Bugs fixed during development
- `GET /api/tickets/{id}/comments` returned `200 []` for non-existent tickets — now returns `404`
- `CreateCommentRequest` had `MaxLength(5000)` while DbContext caps at `1000` — aligned
- `UsersController` was missing despite being in the API contract
- No auto-migration on startup — fixed
- `launchSettings.json` had backend on port `5000`; frontend expected `5020` — port aligned

### Test plan
- See `test-strategy.md` for the full manual test case list (27 cases)
- See `test-results.md` for recorded pass/fail results
- Run automated tests: `dotnet test tests/SupportTickets.Api.Tests`
