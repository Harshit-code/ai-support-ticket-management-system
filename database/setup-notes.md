# Database Setup Notes

## Development

- Provider: SQLite
- Connection string: `Data Source=support_tickets.db`
- DB file location: `backend/src/SupportTicketSystem.Api/support_tickets.db`
- DB file is gitignored

## Running the API locally

```bash
cd src/SupportTickets.Api
dotnet run
```

API: http://localhost:5000  
Swagger UI: http://localhost:5000/swagger

## Running Migrations (once domain entities are added)

```bash
cd src

# Create migration
dotnet ef migrations add InitialCreate \
  --project SupportTickets.Infrastructure \
  --startup-project SupportTickets.Api

# Apply migration
dotnet ef database update \
  --project SupportTickets.Infrastructure \
  --startup-project SupportTickets.Api
```

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
