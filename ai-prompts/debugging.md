# AI Prompts — Debugging

## Build Issues

**Issue 1 — DLL file lock on rebuild**
> `dotnet build` failed with `MSB3027: Could not copy SupportTickets.Infrastructure.dll — file locked by SupportTickets.Api (PID 9472)`

Cause: a previous `dotnet run` process was still running and holding the output DLL.
Fix: killed the process with `taskkill /PID 9472 /F`, then rebuilt successfully.

**Issue 2 — PowerShell heredoc not supported**
> Git commit with multiline message using `cat <<'EOF'` syntax failed in PowerShell.

Cause: PowerShell does not support bash-style heredoc (`<<'EOF'`). The `<` operator is reserved.
Fix: used a single-line commit message with `-m "..."` instead.

**Issue 3 — `dotnet` not found in Git Bash**
> `bash: dotnet: command not found` when running `dotnet run` from Git Bash.

Cause: Git Bash does not inherit the Windows PATH for .NET SDK by default.
Fix: use PowerShell or Windows Terminal instead of Git Bash for .NET commands.

---

## Sanity Check Bugs (found before final merge)

**Bug 1 — GET comments returns 200 for non-existent ticket**
> `GET /api/tickets/999/comments` returned `200 []` instead of `404`.

Cause: `CommentService.GetByTicketIdAsync` delegated directly to the repository with no ticket existence check.
Fix: added `_tickets.GetByIdAsync(ticketId) ?? throw new KeyNotFoundException(...)` in the service method. Controller now catches `KeyNotFoundException` and returns `404`.

**Bug 2 — MaxLength mismatch between DTO and DbContext**
> `CreateCommentRequest` validated up to 5000 chars; `AppDbContext` had `HasMaxLength(1000)`.

Cause: DTO and DB constraint were set independently and not cross-checked.
Fix: aligned `CreateCommentRequest` to `[MaxLength(1000)]`.

**Bug 3 — UsersController missing**
> `GET /api/users` was documented in `api-contract.md` but no controller existed.

Cause: endpoint was planned from the start but implementation was never triggered.
Fix: added `IUserRepository`, `UserRepository`, and `UsersController` with GET all and GET by id.

**Bug 4 — No auto-migration on startup**
> Fresh clones had no `support_tickets.db` and no instructions to run `dotnet ef database update`.

Fix: added `db.Database.Migrate()` in `Program.cs` inside a scoped service call before the app starts listening.
