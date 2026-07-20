# Implementation Plan

## Phase 1 — Core Domain Model ✅
- [x] Entities: User, Ticket, Comment
- [x] Enums: UserRole, TicketPriority, TicketStatus
- [x] State machine: TicketStatusTransitions (static, Core layer)
- [x] Repository interfaces: IUserRepository, ITicketRepository, ICommentRepository
- [x] EF Core DbContext with Fluent API configuration
- [x] Seed data: 4 users (1 Admin, 2 Agents, 1 Customer), 3 tickets (Open/InProgress/Resolved), 2 comments

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
- [x] `PATCH /api/tickets/{id}/status` — catches `InvalidTransitionException` and returns **409 Conflict** with `error`, `from`, `to`, `allowed`
- [x] Build verified: 0 errors, 0 warnings

## Phase 5 — Comment Endpoints ✅
- [x] `ICommentRepository` + `ICommentService` interfaces (Domain/Interfaces)
- [x] `CommentService` (Domain/Services) — validates ticket exists via `ITicketRepository`, throws `KeyNotFoundException` if not
- [x] `CommentRepository` (Infrastructure/Repositories) — sets `CreatedAt`, orders by `CreatedAt` ascending on reads
- [x] `CreateCommentRequest` DTO — message required + non-empty (`[Required]`, `[MinLength(1)]`, `[MaxLength(1000)]`)
- [x] `CommentsController` — `GET /api/tickets/{ticketId}/comments`, `POST /api/tickets/{ticketId}/comments`
- [x] `KeyNotFoundException` caught in controller → 404 (not 500)
- [x] Message trimmed before save to prevent whitespace-only comments slipping through
- [x] `ICommentRepository` + `ICommentService` registered in Program.cs
- [x] Build verified: 0 errors, 0 warnings

## Phase 6 — Search and Filtering ✅
- [x] `keyword` query param — case-insensitive `Contains` on title and description (EF Core translates to SQL LIKE)
- [x] `status` query param — exact enum match; accepts string values (`"InProgress"`) via `JsonStringEnumConverter`
- [x] Both params are optional and combine with AND when both supplied
- [x] Interface signatures updated: `GetAllAsync(string? keyword, TicketStatus? status)` on repo and service
- [x] Conditional query built with `IQueryable` — filters only applied when param is non-null/non-empty
- [x] Controller uses `[FromQuery]` on existing `GET /api/tickets` — no new endpoint added
- [x] Build verified: 0 errors, 0 warnings

## Phase 7 — Frontend Core ✅
- [x] Vite + React 18 + TypeScript + Tailwind CSS scaffolded in `frontend/`
- [x] Types matching backend entities: `User`, `Ticket`, `Comment`, enums
- [x] `statusTransitions.ts` — mirrors `TicketStatusTransitions.cs` exactly, single source of truth on frontend
- [x] API layer: `client.ts` (axios), `tickets.ts`, `comments.ts`, `users.ts`
- [x] Shared components: `StatusBadge`, `PriorityBadge`, `LoadingSpinner`, `ErrorMessage`, `EmptyState`
- [x] `TicketListPage` — search (keyword) + status filter, loading/empty/error states, links to detail
- [x] `TicketDetailPage` — full ticket info, comment thread, add comment form, status transition buttons (only valid next states shown)
- [x] `CreateTicketPage` — form with client-side + server-side validation, user dropdown from API
- [x] React Router v6 routing: `/`, `/tickets/new`, `/tickets/:id`
- [x] CORS already configured on backend for `http://localhost:5173`
- [x] TypeScript build: 0 errors

## Phase 8 — Frontend Error Display + Unit Tests ✅
- [x] `TicketDetailPage` — structured 409 error display: shows `error` message + `allowed` transitions as badges
- [x] `axios.isAxiosError` guard used to safely extract `error` and `allowed` from response body
- [x] `TransitionError` interface added to `frontend/src/types/index.ts`
- [x] `launchSettings.json` port corrected from `5000` → `5020` to match frontend `client.ts` baseURL
- [x] `tests/SupportTickets.Api.Tests` project created (xUnit + Moq, net8.0)
- [x] `TicketStatusTransitionsTests` — 32 pure unit tests covering all 25 from→to combinations + `GetAllowed` per state
- [x] `TicketStatusTransitionControllerTests` — 30 controller tests with `ITicketService` mocked (`MockBehavior.Strict`)
- [x] All 62 tests pass, zero warnings
- [x] Test project added to `src/SupportTickets.sln`
