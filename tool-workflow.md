# Tool Workflow

## Git Strategy
- `main` — stable, production-ready branch. All features merge here via PR.
- `feature/<name>` — one branch per requirement/feature, cut from `main`.
- No direct commits to `main`.

## Branch Naming
```
feature/core-domain-model
feature/ef-migrations
feature/<next-feature>
```

## Commit Discipline
- Each commit contains only what was asked for in that implementation step
- A single commit can span multiple files/layers if they are all part of the same ask
- Do not bundle future features into a commit

## PR Flow
1. Cut branch from `main`
2. Implement what was asked (and only that)
3. Update relevant on-the-go md files alongside the code
4. Commit → push → raise PR → merge into `main`
5. Pull `main` locally after merge

## AI Tool Usage
- Cursor AI used for implementation guidance, code generation, and design review
- All AI prompts logged in `ai-prompts/` folder per phase
- Implementation is reviewed for alignment with planned design before merging
