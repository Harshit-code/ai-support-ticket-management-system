# Test Results

## Backend API — Manual Testing via Swagger UI

Status: **Pending** — run `dotnet run` in `src/SupportTickets.Api` and execute each case from `test-strategy.md`.

### How to run
1. `cd src/SupportTickets.Api && dotnet run`
2. Open `http://localhost:5020/swagger`
3. Work through each test case in `test-strategy.md`
4. Update the table below with Pass / Fail and any notes

---

## Results

| # | Endpoint | Result | Notes |
|---|---|---|---|
| U1 | GET /api/users | — | |
| U2 | GET /api/users/1 | — | |
| U3 | GET /api/users/999 | — | |
| T1 | GET /api/tickets | — | |
| T2 | GET /api/tickets?status=Open | — | |
| T3 | GET /api/tickets?keyword=login | — | |
| T4 | GET /api/tickets?keyword=export&status=InProgress | — | |
| T5 | GET /api/tickets/1 | — | |
| T6 | POST /api/tickets | — | |
| T7 | PUT /api/tickets/1 | — | |
| T8 | PATCH /api/tickets/1/status → InProgress | — | |
| T9 | PATCH /api/tickets/1/status → Resolved | — | |
| T10 | PATCH /api/tickets/1/status → Closed | — | |
| T11 | POST missing title | — | |
| T12 | POST invalid priority | — | |
| T13 | PUT empty description | — | |
| T14 | GET /api/tickets/999 | — | |
| T15 | PUT /api/tickets/999 | — | |
| T16 | PATCH Resolved→Closed | — | |
| T17 | PATCH invalid transition | — | |
| T18 | PATCH on Closed ticket | — | |
| C1 | GET /api/tickets/2/comments | — | |
| C2 | POST /api/tickets/1/comments | — | |
| C3 | GET /api/tickets/1/comments | — | |
| C4 | POST missing message | — | |
| C5 | POST whitespace-only message | — | |
| C6 | POST /api/tickets/999/comments | — | |
| C7 | GET /api/tickets/999/comments | — | |
