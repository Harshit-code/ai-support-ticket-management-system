# AI Prompts — Design

## Feature: Core Domain Model

**Prompt 1 — Architecture approach**
> "I want to keep the controllers really thin and put all the business logic especially the status transition validation inside a service layer instead of the controllers — suggest a clean folder structure inside src for this setup"

Used to confirm 3-layer architecture (Core / Infrastructure / Api) with service layer sitting in Core.

**Prompt 2 — Endpoint design**
> "Give me a first draft of the REST endpoints for tickets, comments, and a separate endpoint just for changing ticket status — PATCH /tickets/{id}/status"

Used to finalise API contract before implementation.

**Prompt 3 — Design review**
> "Examine from the design I already did — are we aligned?"

Revealed that first implementation placed business logic in repositories instead of services. Led to the service layer refactor.
