# Design Notes

## Architecture

3-layer clean architecture:
- **Core** — entities, enums, interfaces, state machine logic. No external dependencies.
- **Infrastructure** — EF Core DbContext, repository implementations, seed data.
- **Api** — ASP.NET Core controllers, DI wiring, Swagger.

## Key Design Decisions

**State machine as a static class (`TicketStatusTransitions`)**
Transition rules live in `Core` as a dictionary of `TicketStatus → allowed next states`. This keeps the rule in one place and is testable without EF Core. Both the repository and API controller reference it independently.

**Terminal state check (`IsTerminal`)**
Extracted as a helper on the same class so the comment-blocking rule reads as intent: `if (IsTerminal(ticket.Status)) throw`.

**`assignedToId` nullable**
Tickets start unassigned. Assignment is done via the update endpoint.

**Comments are immutable**
No `updatedAt` on Comment. No edit or delete endpoint. Comments are an audit trail.

**SQLite for development**
Zero setup, single file DB. Easy to reset by deleting `support_tickets.db`. Swap connection string for SQL Server in production.

**Auto-migrate on startup**
`db.Database.Migrate()` runs in `Program.cs` so the DB and seed data are always up to date when the app starts locally.

## Database
- Provider: SQLite (dev)
- Migrations: EF Core code-first
- Location: `backend/src/SupportTicketSystem.Api/support_tickets.db` (gitignored)
