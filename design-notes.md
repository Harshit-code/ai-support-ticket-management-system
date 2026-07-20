## Design Notes

## Architecture

3-layer clean architecture:
- **Domain** — entities, enums, interfaces, state machine, service implementations, custom exceptions. No external dependencies.
- **Infrastructure** — EF Core DbContext, repository implementations, seed data.
- **Api** — ASP.NET Core controllers, DTOs, DI wiring, Swagger.

## Solution Structure (active — inside `src/`)
```
src/
├── SupportTickets.sln
├── SupportTickets.Api/              ← Web API, thin controllers, DTOs, Swagger, DI wiring
├── SupportTickets.Domain/           ← Entities, enums, interfaces, services, state machine, exceptions (no deps)
└── SupportTickets.Infrastructure/   ← EF Core DbContext, repositories, seed data
```

## Key Design Decisions

**State machine as a static class (`TicketStatusTransitions`)**
Transition rules live in `Domain/StateMachine` as a private read-only dictionary of `TicketStatus → allowed next states`. This is the single source of truth — nothing else in the codebase declares what transitions are valid. Testable without EF Core.

**Business logic in the service layer, not the repository**
`TicketService.TransitionStatusAsync` reads the current status, checks the map, and throws `InvalidTransitionException` if the transition is invalid. `TicketRepository.UpdateStatusAsync` only writes status + `UpdatedAt` — no logic. Controllers catch the exception and map it to HTTP.

**409 Conflict for invalid status transitions (not 400)**
A well-formed `PATCH /api/tickets/{id}/status` request can be rejected purely because of the ticket's current state on the server. That is a resource conflict, not a malformed request. `400` is reserved for DTO validation failures (missing fields, invalid enum values) which are caught automatically by `[ApiController]` before the action body runs.

**`InvalidTransitionException` carries structured data**
The exception exposes `From`, `To`, and `Allowed` as typed properties (not just a message string) so the controller can forward all three as separate JSON fields. The `Message` string also includes all three inline so it is immediately readable without parsing.

**`assignedToId` nullable**
Tickets start unassigned. Assignment is done via the PUT endpoint.

**Comments are immutable**
No `updatedAt` on Comment. No edit or delete endpoint. Comments are an append-only audit trail.

**SQLite for development**
Zero setup, single file DB. Easy to reset by deleting the `.db` file. Swap connection string for SQL Server in production.

**`JsonStringEnumConverter` + `ReferenceHandler.IgnoreCycles` globally**
Enums serialize as strings (`"High"` not `2`) for readability in Swagger and the frontend. Cycle handling prevents infinite loops on EF Core navigation properties without needing explicit `[JsonIgnore]` attributes on every nav property.
