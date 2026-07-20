# Implementation Plan

## Phase 1 — Core Domain Model ✅
- [x] Entities: User, Ticket, Comment
- [x] Enums: UserRole, TicketPriority, TicketStatus
- [x] State machine: TicketStatusTransitions (static, Core layer)
- [x] Repository interfaces: IUserRepository, ITicketRepository, ICommentRepository
- [x] EF Core DbContext with Fluent API configuration
- [x] Seed data: 5 users (1 Admin, 2 Agents, 2 Customers)
- [x] Repository implementations
- [x] API controllers: Users, Tickets, Comments
- [x] Program.cs wiring (DI, CORS, Swagger, auto-migrate)

## Phase 2 — .NET Solution Scaffold ✅
- [x] New solution: `src/SupportTickets.sln`
- [x] `SupportTickets.Api` — Web API project (net8.0)
- [x] `SupportTickets.Domain` — class library for entities and enums
- [x] `SupportTickets.Infrastructure` — class library for EF Core, DbContext, migrations, seed
- [x] Project references wired: Api → Domain + Infrastructure, Infrastructure → Domain
- [x] SQLite connection string in appsettings.json
- [x] Swagger configured: API info, XML docs, annotations, request duration
- [x] Build verified: 0 errors, 0 warnings
- [x] Domain entities: User, Ticket, Comment + enums (UserRole, TicketPriority, TicketStatus)
- [x] AppDbContext with Fluent API configuration and seed data
- [x] EF Core migration: `InitialCreate` created and applied to SQLite
- [x] Seed data: 4 users (1 Admin, 2 Agents, 1 Customer), 3 tickets (Open/InProgress/Resolved), 2 comments

## Phase 3 — Ticket API Endpoints ✅
- [x] `ITicketRepository` + `ITicketService` interfaces (Domain/Interfaces)
- [x] `TicketService` implementation (Domain/Services) — business logic owner
- [x] `TicketRepository` implementation (Infrastructure/Repositories) — pure data access
- [x] `CreateTicketRequest` + `UpdateTicketRequest` DTOs with DataAnnotations validation
- [x] `TicketsController` — GET all, GET by id, POST, PUT (status excluded from PUT)
- [x] `[ApiController]` automatic 400 with field-specific errors on validation failure
- [x] `JsonStringEnumConverter` + `ReferenceHandler.IgnoreCycles` added globally
- [x] DI registered in Program.cs
- [x] Build verified: 0 errors, 0 warnings

## Phase 4 — Status Transition Endpoint ✅
- [x] `TicketStatusTransitions` static class (Domain/StateMachine) — single map of all allowed transitions
- [x] `InvalidTransitionException` (Domain/Exceptions) — carries From, To, Allowed fields
- [x] `ITicketService.TransitionStatusAsync` + `ITicketRepository.UpdateStatusAsync` added to interfaces
- [x] `TicketService.TransitionStatusAsync` — reads current status, checks map, throws `InvalidTransitionException` on violation
- [x] `TicketRepository.UpdateStatusAsync` — writes status + UpdatedAt only, zero logic
- [x] `PatchTicketStatusRequest` DTO with `[EnumDataType]` validation
- [x] `PATCH /api/tickets/{id}/status` — catches `InvalidTransitionException` and returns 400 with `error`, `from`, `to`, `allowed`
- [x] Build verified: 0 errors, 0 warnings

## Phase 5 — (next)
- [ ] Frontend ticket list and detail views
- [ ] Status transition UI
- [ ] Comment thread UI
- [ ] Tests
