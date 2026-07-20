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

## Phase 3 — (next)
- [ ] Domain entities and enums in SupportTickets.Domain
- [ ] EF Core DbContext + seed data in SupportTickets.Infrastructure
- [ ] EF Core initial migration
- [ ] Service layer + repository pattern
- [ ] API controllers
- [ ] React frontend scaffold

## Phase 4 — (future)
- [ ] Frontend ticket list and detail views
- [ ] Status transition UI
- [ ] Comment thread UI
- [ ] Tests
