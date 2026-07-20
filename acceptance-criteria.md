# Acceptance Criteria

## Feature: Core Domain Model

- [x] Three entities exist: User, Ticket, Comment — with exactly the fields specified
- [x] User is seeded only — no create/update/delete API
- [x] Ticket status defaults to Open on creation
- [x] Only allowed status transitions succeed; all others return **409 Conflict** with structured `error`, `from`, `to`, `allowed` fields
- [x] `assignedTo` is nullable — tickets can be created without an assignee
- [ ] Comments cannot be added to Closed or Cancelled tickets — **known gap, not yet implemented**
- [x] Comments are immutable — no edit or delete endpoint
- [x] 4 seed users exist on first run (1 Admin, 2 Agents, 1 Customer)
- [x] API is browsable via Swagger at `/swagger` in development
- [x] All business logic (state machine, terminal state check) lives in the service layer — not controllers or repositories

## Feature: Frontend

- [x] Ticket list page with keyword search and status filter
- [x] Ticket detail page showing ticket info and all comments
- [x] Create ticket page with form validation
- [x] Status transition buttons show only valid next statuses (mirrors backend state machine)
- [x] Structured 409 error shown on invalid transition — includes error message and allowed transitions as badges
- [x] Every page handles loading, empty, and error states
- [x] Frontend connects to backend via CORS at `http://localhost:5020`

## Feature: Unit Tests

- [x] All 25 from→to status combinations tested (5 valid, 20 invalid)
- [x] Controller `PATCH /status` tested with mocked `ITicketService`
- [x] 62 tests total, all passing
