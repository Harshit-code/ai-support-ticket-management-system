# AI Prompts — Documentation

## Documentation Generated During Build

**Prompt 1 — Documentation structure and update cadence**
> "Finalize your workflow — list down all the md files. Some can be updated at last, some on the go."

Established cadence:
- **On the go (each feature):** `implementation-plan.md`, `api-contract.md`, `design-notes.md`, `ai-prompts/*.md`
- **Once at start:** `requirements-analysis.md`, `candidate-info.md`, `data-model.md`
- **At the end:** `reflection.md`, `final-ai-usage-summary.md`, `test-results.md`, `pr-description.md`, `test-strategy.md`

**Prompt 2 — ai-prompts backfill**
> "Are md files being updated as features get completed? I can't see the updated prompt or relevant information."

Revealed that `ai-prompts/planning.md` was empty and `ai-prompts/implementation.md` had stopped at Prompt 5. Triggered a full backfill of all ai-prompts files.

**Prompt 3 — End-of-backend documentation**
> "Backend is complete. Add the relevant backend README architecture design file and update the relevant md files we decided to do at last for backend."

Generated:
- `src/README.md` — setup instructions, project structure, API endpoint reference, state machine diagram, validation table, seed data
- `test-strategy.md` — 27 manual test cases
- `test-results.md` — blank pass/fail table to be filled after running Swagger tests
- `pr-description.md` — PR summary including what's included, bugs fixed, architecture decisions
- `reflection.md` — what went well, what could be better, key technical decisions table
- `final-ai-usage-summary.md` — how AI was used, what it did well, what required human direction

**Prompt 4 — ai-prompts files update**
> "Also ai-prompt relevant md files that also need updating."

Created missing files: `ai-prompts/testing.md`, `ai-prompts/debugging.md`, `ai-prompts/code-review.md`, `ai-prompts/documentation.md`. Updated `ai-prompts/implementation.md` with Prompts 13–14.
