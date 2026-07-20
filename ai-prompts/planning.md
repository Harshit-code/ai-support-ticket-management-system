# AI Prompts — Planning

## Initial Planning

**Prompt 1 — Project setup**
> "I'm starting a new assessment project — a support ticket management system, backend-heavy option, built with ASP.NET Core Web API + EF Core + React frontend. Create the skeleton project and do the initial commit."

Decided on minimal skeleton (folders with `.gitkeep`, no code) to verify GitHub push before any implementation.

**Prompt 2 — Git workflow**
> "Create a main branch where all feature branches will be merged. Set up PR workflow."

Established: `main` as the base, feature branches created from `main`, changes merged via PRs.

**Prompt 3 — Documentation structure**
> "Finalize your workflow — list down all the md files. Some can be updated at last, some on the go while we push features."

Decided documentation update cadence:
- **On the go with each feature:** `implementation-plan.md`, `api-contract.md`, `design-notes.md`, `data-model.md`, `ai-prompts/*.md`
- **Once at start:** `requirements-analysis.md`, `candidate-info.md`
- **At the end:** `reflection.md`, `final-ai-usage-summary.md`, `test-results.md`, `pr-description.md`

**Prompt 4 — Architecture planning**
> "I want to keep the controllers really thin and put all the business logic inside a service layer. Suggest a clean folder structure inside src."

Confirmed 3-layer architecture:
- `SupportTickets.Domain` — entities, enums, interfaces, services, state machine, exceptions (no external deps)
- `SupportTickets.Infrastructure` — EF Core, repositories, seed data
- `SupportTickets.Api` — controllers, DTOs, DI wiring, Swagger

**Prompt 5 — Phased delivery approach**
> "When one feature is completed, merge to main before giving next requirement."

Established: each feature is a separate branch from `main`, work verified via build before committing, docs updated on the go within same commit or follow-up commit.
