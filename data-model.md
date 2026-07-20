# Data Model

## Entity Relationship Diagram (text)

```
User (seeded)
├── id PK
├── name
├── email (unique)
└── role  [Admin | Agent | Customer]

Ticket
├── id PK
├── title
├── description
├── priority  [Low | Medium | High | Critical]
├── status    [Open | InProgress | Resolved | Closed | Cancelled]
├── createdById  FK → User (restrict delete)
├── assignedToId FK → User (set null on delete), nullable
├── createdAt
└── updatedAt

Comment
├── id PK
├── ticketId    FK → Ticket (cascade delete)
├── message
├── createdById FK → User (restrict delete)
└── createdAt
```

## Relationships
- One User → many Tickets (as creator)
- One User → many Tickets (as assignee, optional)
- One Ticket → many Comments
- One User → many Comments

## Seed Data (Users)
| id | name | email | role |
|---|---|---|---|
| 1 | Alice Admin | alice@support.dev | Admin |
| 2 | Bob Agent | bob@support.dev | Agent |
| 3 | Carol Agent | carol@support.dev | Agent |
| 4 | Dave Customer | dave@support.dev | Customer |
| 5 | Eve Customer | eve@support.dev | Customer |
