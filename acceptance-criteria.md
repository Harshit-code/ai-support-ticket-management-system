# Acceptance Criteria

## Feature: Core Domain Model

- [x] Three entities exist: User, Ticket, Comment — with exactly the fields specified
- [x] User is seeded only — no create/update/delete API
- [x] Ticket status defaults to Open on creation
- [x] Only allowed status transitions succeed; all others return 400 with an error message
- [x] `assignedTo` is nullable — tickets can be created without an assignee
- [x] Comments cannot be added to Closed or Cancelled tickets — returns 400
- [x] Comments are immutable — no edit or delete endpoint
- [x] 5 seed users exist on first run (1 Admin, 2 Agents, 2 Customers)
- [x] API is browsable via Swagger at `/swagger` in development
- [x] All business logic (state machine, terminal state check) lives in the service layer — not controllers or repositories
