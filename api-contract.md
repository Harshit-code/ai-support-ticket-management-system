# API Contract

## Users

| Method | Route | Description |
|---|---|---|
| GET | /api/users | List all seeded users |
| GET | /api/users/{id} | Get user by id |

## Tickets

| Method | Route | Description |
|---|---|---|
| GET | /api/tickets | List all tickets (includes createdBy, assignedTo) |
| GET | /api/tickets/{id} | Get ticket with comments |
| POST | /api/tickets | Create ticket |
| PUT | /api/tickets/{id} | Update title, description, priority, assignedTo |
| PATCH | /api/tickets/{id}/status | Transition status (state machine enforced) |

### POST /api/tickets — Request
```json
{
  "title": "string",
  "description": "string",
  "priority": "Low | Medium | High | Critical",
  "createdById": 1,
  "assignedToId": 2
}
```

### PUT /api/tickets/{id} — Request
```json
{
  "title": "string",
  "description": "string",
  "priority": "Low | Medium | High | Critical",
  "assignedToId": 2
}
```

### PATCH /api/tickets/{id}/status — Request
```json
{ "newStatus": "InProgress" }
```
Returns `400` with `{ "error": "...", "allowed": [...] }` if transition is invalid.

## Comments

| Method | Route | Description |
|---|---|---|
| GET | /api/tickets/{ticketId}/comments | List comments for a ticket |
| POST | /api/tickets/{ticketId}/comments | Add comment (blocked on terminal tickets) |

### POST /api/tickets/{ticketId}/comments — Request
```json
{
  "message": "string",
  "createdById": 1
}
```
Returns `400` if ticket is Closed or Cancelled.
Returns `404` if ticket not found.
