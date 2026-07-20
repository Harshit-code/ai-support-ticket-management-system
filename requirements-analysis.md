# Requirements Analysis

## Feature: Core Domain Model

### Entities

**User** *(seeded only — no user-management API)*
| Field | Type | Notes |
|---|---|---|
| id | int | PK |
| name | string | required, max 100 |
| email | string | required, unique, max 200 |
| role | enum | Admin / Agent / Customer |

**Ticket**
| Field | Type | Notes |
|---|---|---|
| id | int | PK |
| title | string | required, max 200 |
| description | string | required, max 2000 |
| priority | enum | Low / Medium / High / Critical |
| status | enum | Open / InProgress / Resolved / Closed / Cancelled |
| createdById | int FK → User | required |
| assignedToId | int FK → User | nullable |
| createdAt | datetime | UTC, set on create |
| updatedAt | datetime | UTC, updated on every change |

**Comment**
| Field | Type | Notes |
|---|---|---|
| id | int | PK |
| ticketId | int FK → Ticket | required, cascades on delete |
| message | string | required, max 1000 |
| createdById | int FK → User | required |
| createdAt | datetime | UTC, set on create — immutable |

### Allowed Status Transitions (server-enforced)
```
Open        → InProgress, Cancelled
InProgress  → Resolved, Cancelled
Resolved    → Closed
Closed      → (terminal)
Cancelled   → (terminal)
```

### Behaviour Rules
- New comments are blocked on Closed and Cancelled tickets ⚠️ *(not yet implemented — tracked as known gap)*
- `assignedTo` is nullable — tickets can be created unassigned
- Comments are immutable — no edit, no delete
- Users are seeded only — no CRUD API for users
