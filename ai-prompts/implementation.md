# AI Prompts — Implementation

## Feature: Core Domain Model

**Prompt 1 — Entity design review**
> "entities are user which is seeded only, ticket with id title description priority status assignedTo createdBy createdAt updatedAt and comment with id ticketId message createdBy createdAt — look for anything missing especially around the state machine"

Used to identify gap around Resolved→InProgress and comment behaviour on terminal tickets.

**Prompt 2 — State machine lock-in**
> Confirmed final allowed transitions, terminal states, comment blocking on Closed/Cancelled, assignedTo nullable.

**Prompt 3 — Implementation**
> Full core domain model implemented: enums, entities, TicketStatusTransitions, interfaces, DbContext with seeding, repositories, controllers.
