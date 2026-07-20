# Database Setup Notes

## Development

- Provider: SQLite
- Connection string: `Data Source=support_tickets.db`
- DB file location: `backend/src/SupportTicketSystem.Api/support_tickets.db`
- DB file is gitignored

## Running Migrations

```bash
cd backend

# Create migration
dotnet ef migrations add InitialCreate \
  --project src/SupportTicketSystem.Infrastructure \
  --startup-project src/SupportTicketSystem.Api

# Apply migration (also runs automatically on app startup)
dotnet ef database update \
  --project src/SupportTicketSystem.Infrastructure \
  --startup-project src/SupportTicketSystem.Api
```

## Seed Data
Users are seeded via `modelBuilder.Entity<User>().HasData(...)` in `AppDbContext.OnModelCreating`.
Applied as part of the `InitialCreate` migration.
