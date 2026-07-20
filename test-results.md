# Test Results

## Backend API — Manual Testing via Swagger UI

Status: **Pass** — tested via Swagger UI at `http://localhost:5020/swagger` and confirmed end-to-end via React frontend at `http://localhost:5173`.

---

## Results

| # | Endpoint | Result | Notes |
|---|---|---|---|
| U1 | GET /api/users | Pass | Returns all 4 seeded users |
| U2 | GET /api/users/1 | Pass | Returns Alice Admin |
| U3 | GET /api/users/999 | Pass | 404 with `{ "error": "User 999 not found." }` |
| T1 | GET /api/tickets | Pass | Returns 3 seeded tickets ordered by createdAt desc |
| T2 | GET /api/tickets?status=Open | Pass | Returns only Open tickets |
| T3 | GET /api/tickets?keyword=login | Pass | Returns ticket 1 (title match) |
| T4 | GET /api/tickets?keyword=export&status=InProgress | Pass | Returns ticket 2 only |
| T5 | GET /api/tickets/1 | Pass | Returns ticket with comments array |
| T6 | POST /api/tickets | Pass | 201, new ticket with status=Open |
| T7 | PUT /api/tickets/1 | Pass | 200, updated ticket fields |
| T8 | PATCH /api/tickets/1/status → InProgress | Pass | 200, status updated |
| T9 | PATCH /api/tickets/1/status → Resolved | Pass | 200, status updated |
| T10 | PATCH /api/tickets/1/status → Closed | Pass | 200, status updated |
| T11 | POST missing title | Pass | 400 with errors.Title |
| T12 | POST invalid priority | Pass | 400 with errors.Priority |
| T13 | PUT empty description | Pass | 400 with errors.Description |
| T14 | GET /api/tickets/999 | Pass | 404 |
| T15 | PUT /api/tickets/999 | Pass | 404 |
| T16 | PATCH Resolved→Closed | Pass | 200 |
| T17 | PATCH invalid transition | Pass | 409 with `from`, `to`, `allowed` |
| T18 | PATCH on Closed ticket | Pass | 409 with `allowed: []` and terminal state message |
| C1 | GET /api/tickets/2/comments | Pass | Returns 1 seeded comment |
| C2 | POST /api/tickets/1/comments | Pass | 201 with ticketId and createdAt set |
| C3 | GET /api/tickets/1/comments | Pass | Returns comments ordered oldest first |
| C4 | POST missing message | Pass | 400 with errors.Message |
| C5 | POST whitespace-only message | Pass | Trimmed to empty string, MinLength fails → 400 |
| C6 | POST /api/tickets/999/comments | Pass | 404 |
| C7 | GET /api/tickets/999/comments | Pass | 404 |

---

## Automated Unit Tests

| Suite | Tests | Result |
|---|---|---|
| `TicketStatusTransitionsTests` (state machine) | 32 | Pass |
| `TicketStatusTransitionControllerTests` (controller + Moq) | 30 | Pass |
| **Total** | **62** | **All Pass** |

Run with: `dotnet test tests/SupportTickets.Api.Tests`
