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

## Phase 2 — (next)
- [ ] EF Core initial migration
- [ ] API validation (FluentValidation or DataAnnotations)
- [ ] Error handling middleware
- [ ] React frontend scaffold

## Phase 3 — (future)
- [ ] Frontend ticket list and detail views
- [ ] Status transition UI
- [ ] Comment thread UI
- [ ] Tests
