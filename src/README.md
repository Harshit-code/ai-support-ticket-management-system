# Support Tickets — Backend

ASP.NET Core 8 Web API with EF Core + SQLite.

---

## Prerequisites

| Tool | Version | Install |
|---|---|---|
| .NET SDK | 8.0+ | [dotnet.microsoft.com](https://dotnet.microsoft.com/download) |
| EF Core CLI | any | `dotnet tool install --global dotnet-ef` |

---

## Running locally

```bash
cd src/SupportTickets.Api
dotnet run
```

The database file (`support_tickets.db`) is created and migrated automatically on first startup. Seed data (users, tickets, comments) is applied via EF Core `HasData`.

Swagger UI: **http://localhost:5020/swagger**

---

## Project structure

```
src/
├── SupportTickets.sln
│
├── SupportTickets.Api/              # Web API — entry point
│   ├── Controllers/                 # Thin controllers, HTTP mapping only
│   │   ├── TicketsController.cs
│   │   ├── CommentsController.cs
│   │   └── UsersController.cs
│   ├── DTOs/                        # Request shapes with DataAnnotations validation
│   │   ├── CreateTicketRequest.cs
│   │   ├── UpdateTicketRequest.cs
│   │   ├── PatchTicketStatusRequest.cs
│   │   └── CreateCommentRequest.cs
│   └── Program.cs                   # DI wiring, Swagger, CORS, auto-migrate
│
├── SupportTickets.Domain/           # No external dependencies
│   ├── Entities/                    # User, Ticket, Comment
│   ├── Enums/                       # UserRole, TicketPriority, TicketStatus
│   ├── Interfaces/                  # ITicketRepository, ITicketService, ICommentRepository, ICommentService, IUserRepository
│   ├── Services/                    # TicketService, CommentService — business logic lives here
│   ├── StateMachine/                # TicketStatusTransitions — single source of truth for allowed transitions
│   └── Exceptions/                  # InvalidTransitionException
│
└── SupportTickets.Infrastructure/   # EF Core, SQLite
    ├── Data/
    │   └── AppDbContext.cs          # Fluent API config + HasData seed
    └── Repositories/                # TicketRepository, CommentRepository, UserRepository — pure data access
```

---

## Architecture

3-layer architecture with a strict dependency rule: outer layers depend on inner, never the reverse.

```
Api  →  Domain  ←  Infrastructure
```

- **Domain** owns entities, enums, interfaces, services, state machine, and custom exceptions. Zero external dependencies.
- **Infrastructure** implements the repository interfaces using EF Core. Depends on Domain only.
- **Api** wires everything together via DI. Controllers are thin — they map HTTP to service calls and return responses. All business logic is in Domain services.

---

## API endpoints

### Tickets

| Method | Route | Description |
|---|---|---|
| `GET` | `/api/tickets` | List tickets. Optional: `?keyword=` (title/description search), `?status=` filter |
| `GET` | `/api/tickets/{id}` | Get ticket with comments |
| `POST` | `/api/tickets` | Create ticket (status always set to `Open`) |
| `PUT` | `/api/tickets/{id}` | Update title, description, priority, assignee |
| `PATCH` | `/api/tickets/{id}/status` | Transition status (state machine enforced) |

### Comments

| Method | Route | Description |
|---|---|---|
| `GET` | `/api/tickets/{ticketId}/comments` | Get comments for a ticket (404 if ticket missing) |
| `POST` | `/api/tickets/{ticketId}/comments` | Add comment (404 if ticket missing) |

### Users

| Method | Route | Description |
|---|---|---|
| `GET` | `/api/users` | List all seeded users |
| `GET` | `/api/users/{id}` | Get user by id |

---

## Status state machine

```
Open  ──►  InProgress  ──►  Resolved  ──►  Closed
 │               │
 └──────────────►  Cancelled
```

All other transitions are rejected with `409 Conflict`.

---

## Validation behaviour

| Scenario | HTTP code |
|---|---|
| Missing required field / invalid enum in body | `400 Bad Request` (auto, via `[ApiController]`) |
| Ticket / comment not found | `404 Not Found` |
| Invalid status transition | `409 Conflict` with `error`, `from`, `to`, `allowed` fields |

---

## Seed data

| id | Name | Email | Role |
|---|---|---|---|
| 1 | Alice Admin | alice@support.dev | Admin |
| 2 | Bob Agent | bob@support.dev | Agent |
| 3 | Carol Agent | carol@support.dev | Agent |
| 4 | Dave Customer | dave@support.dev | Customer |

3 sample tickets (Open, InProgress, Resolved) and 2 comments are also seeded.
