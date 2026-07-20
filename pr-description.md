# PR Description

## Backend API — feature/comment-endpoints → main

### Summary

Completes the backend for the AI Support Ticket Management System. All ticket and comment endpoints are implemented, validated, and wired up following a 3-layer clean architecture (Domain / Infrastructure / Api).

### What's included

**Ticket endpoints**
- `GET /api/tickets` — list all tickets with optional `?keyword=` and `?status=` query filters
- `GET /api/tickets/{id}` — get ticket with comments included
- `POST /api/tickets` — create ticket (status forced to `Open`, validated)
- `PUT /api/tickets/{id}` — update title, description, priority, assignee (status excluded)
- `PATCH /api/tickets/{id}/status` — transition status via enforced state machine

**Comment endpoints**
- `GET /api/tickets/{ticketId}/comments` — list comments (404 if ticket missing)
- `POST /api/tickets/{ticketId}/comments` — add comment (404 if ticket missing, message required + non-empty)

**User endpoints**
- `GET /api/users` — list seeded users
- `GET /api/users/{id}` — get user by id

**Infrastructure**
- EF Core SQLite with `InitialCreate` migration and seed data (4 users, 3 tickets, 2 comments)
- Auto-migration on startup — no manual `dotnet ef database update` needed
- Swagger UI at `/swagger`
- CORS configured for Vite dev server (`http://localhost:5173`)

### Architecture decisions
- Business logic (status transition validation, ticket existence checks) lives in Domain services
- Repositories are pure data access with no logic
- `InvalidTransitionException` carries `From`, `To`, `Allowed` for structured error responses
- Invalid transitions return `409 Conflict` (not `400`) — request is well-formed, server state disagrees
- `JsonStringEnumConverter` + `ReferenceHandler.IgnoreCycles` applied globally

### Bugs fixed during sanity check
- `GET /api/tickets/{id}/comments` returned `200 []` for non-existent tickets — now returns `404`
- `CreateCommentRequest` had `MaxLength(5000)` while DbContext caps at `1000` — aligned to `1000`
- `UsersController` was missing despite being in the API contract
- No auto-migration on startup — fixed

### Test plan
See `test-strategy.md` for the full manual test case list.
