# AI Prompts — Design

## Feature: Core Domain Model

**Prompt 1 — Architecture approach**
> "I want to keep the controllers really thin and put all the business logic especially the status transition validation inside a service layer instead of the controllers — suggest a clean folder structure inside src for this setup"

Used to confirm 3-layer architecture (Domain / Infrastructure / Api) with service layer sitting in Domain.

**Prompt 2 — Endpoint design**
> "Give me a first draft of the REST endpoints for tickets, comments, and a separate endpoint just for changing ticket status — PATCH /tickets/{id}/status"

Used to finalise API contract before implementation.

**Prompt 3 — Design review**
> "Examine from the design I already did — are we aligned?"

Revealed that first implementation placed business logic in repositories instead of services. Led to the service layer refactor.

---

## Feature: Status Transition Endpoint

**Prompt 4 — HTTP status code for invalid transition**
> "For an invalid status transition what HTTP status code makes more sense — 400 or 409? I've seen both being used for business rule validation."

Decision: **409 Conflict** over 400 Bad Request.

Reasoning:
- `400` signals the request body is structurally wrong (missing fields, invalid types) — caught by `[ApiController]` before the action runs.
- `409` signals a well-formed request that conflicts with the current state of the resource on the server.
- A status transition failure is state-dependent: the same request (`Open → Closed`) is invalid on an Open ticket but would be valid if the ticket were in a different state. That conditionality is the definition of a conflict.
- Practical benefit: frontend can distinguish "fix your input" (400) from "the action isn't possible right now given the ticket's state" (409) without inspecting the body.

---

## Feature: Comment Endpoints

**Prompt 5 — Comment endpoint scoping**
> "Add POST to add a comment and GET to fetch all comments. Make sure ticket exists — 404 if not. Message required, no empty text."

Design decision: ticket existence check belongs in the service layer (`CommentService`), not the controller or repository. Service calls `ITicketRepository.GetByIdAsync` directly — reuses the existing contract rather than adding a new `TicketExistsAsync` method.

Note: the original design called for blocking comments on Closed/Cancelled tickets (terminal state check). This was not included in this prompt. It is tracked as a known gap in `requirements-analysis.md`.
