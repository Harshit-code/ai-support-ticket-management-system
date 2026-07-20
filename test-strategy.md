# Test Strategy

## Scope
Backend API only (manual via Swagger UI for now). Automated tests are out of scope for this phase.

## Approach
Each endpoint is tested manually through Swagger UI at `http://localhost:5020/swagger`.

---

## Test Cases

### Users

| # | Action | Input | Expected |
|---|---|---|---|
| U1 | GET /api/users | — | 200, array of 4 seeded users |
| U2 | GET /api/users/1 | — | 200, Alice Admin |
| U3 | GET /api/users/999 | — | 404 `{ "error": "User 999 not found." }` |

---

### Tickets — Happy Path

| # | Action | Input | Expected |
|---|---|---|---|
| T1 | GET /api/tickets | — | 200, array of 3 seeded tickets ordered by createdAt desc |
| T2 | GET /api/tickets?status=Open | — | 200, only Open tickets |
| T3 | GET /api/tickets?keyword=login | — | 200, ticket 1 (title contains "Login") |
| T4 | GET /api/tickets?keyword=export&status=InProgress | — | 200, ticket 2 only |
| T5 | GET /api/tickets/1 | — | 200, ticket with comments array |
| T6 | POST /api/tickets | `{ title, description, priority: "High", createdById: 1 }` | 201, new ticket with status=Open |
| T7 | PUT /api/tickets/1 | `{ title, description, priority: "Medium", assignedToId: 2 }` | 200, updated ticket |
| T8 | PATCH /api/tickets/1/status | `{ "newStatus": "InProgress" }` | 200, ticket with status=InProgress |
| T9 | PATCH /api/tickets/1/status | `{ "newStatus": "Resolved" }` | 200, ticket with status=Resolved |
| T10 | PATCH /api/tickets/1/status | `{ "newStatus": "Closed" }` | 200, ticket with status=Closed |

---

### Tickets — Validation and Error Cases

| # | Action | Input | Expected |
|---|---|---|---|
| T11 | POST /api/tickets | missing title | 400, errors.Title present |
| T12 | POST /api/tickets | `priority: "Urgent"` (invalid) | 400, errors.Priority present |
| T13 | PUT /api/tickets/1 | empty description | 400, errors.Description present |
| T14 | GET /api/tickets/999 | — | 404 |
| T15 | PUT /api/tickets/999 | valid body | 404 |
| T16 | PATCH /api/tickets/3/status | `{ "newStatus": "Closed" }` (Resolved→Closed is valid) | 200 |
| T17 | PATCH /api/tickets/1/status | `{ "newStatus": "Open" }` (InProgress→Open invalid) | 409, `from`, `to`, `allowed` present |
| T18 | PATCH on a Closed ticket | any status | 409, allowed=[] with terminal state message |

---

### Comments — Happy Path

| # | Action | Input | Expected |
|---|---|---|---|
| C1 | GET /api/tickets/2/comments | — | 200, 1 seeded comment |
| C2 | POST /api/tickets/1/comments | `{ message: "test comment", createdById: 2 }` | 201, comment with ticketId=1 and createdAt set |
| C3 | GET /api/tickets/1/comments | — | 200, comments ordered oldest first |

---

### Comments — Validation and Error Cases

| # | Action | Input | Expected |
|---|---|---|---|
| C4 | POST /api/tickets/1/comments | missing message | 400, errors.Message present |
| C5 | POST /api/tickets/1/comments | `message: "   "` (whitespace only) | 400 (trimmed to empty, MinLength fails) or saved as empty — verify |
| C6 | POST /api/tickets/999/comments | valid body | 404 |
| C7 | GET /api/tickets/999/comments | — | 404 |
