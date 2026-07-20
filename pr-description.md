# PR Description

## PR #2 — feat: core domain model

**Branch:** `feature/core-domain-model` → `main`

### What this PR does
Implements the full core domain model for the support ticket management system:
- All three entities: User, Ticket, Comment
- Enums: UserRole, TicketPriority, TicketStatus
- State machine (TicketStatusTransitions) enforcing allowed status transitions server-side
- 3-layer architecture: Core (entities, interfaces, services), Infrastructure (DbContext, repositories), Api (thin controllers)
- EF Core with SQLite, auto-migrate on startup, 5 seeded users
- Swagger enabled for local development

### Endpoints introduced
- `GET/POST /api/tickets`, `GET/PUT /api/tickets/{id}`, `PATCH /api/tickets/{id}/status`
- `GET/POST /api/tickets/{id}/comments`
- `GET /api/users`, `GET /api/users/{id}`

### Key decisions
- Status transitions enforced in `TicketService`, not the controller or repository
- Comments blocked on terminal states (Closed, Cancelled) in `CommentService`
- `assignedTo` nullable — tickets start unassigned
