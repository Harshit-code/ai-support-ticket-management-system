using Microsoft.EntityFrameworkCore;
using SupportTickets.Domain.Entities;
using SupportTickets.Domain.Enums;

namespace SupportTickets.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(u => u.Id);
            e.Property(u => u.Name).IsRequired().HasMaxLength(100);
            e.Property(u => u.Email).IsRequired().HasMaxLength(200);
            e.HasIndex(u => u.Email).IsUnique();
        });

        modelBuilder.Entity<Ticket>(e =>
        {
            e.HasKey(t => t.Id);
            e.Property(t => t.Title).IsRequired().HasMaxLength(200);
            e.Property(t => t.Description).IsRequired().HasMaxLength(2000);

            e.HasOne(t => t.CreatedBy)
             .WithMany(u => u.CreatedTickets)
             .HasForeignKey(t => t.CreatedById)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(t => t.AssignedTo)
             .WithMany(u => u.AssignedTickets)
             .HasForeignKey(t => t.AssignedToId)
             .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Comment>(e =>
        {
            e.HasKey(c => c.Id);
            e.Property(c => c.Message).IsRequired().HasMaxLength(1000);

            e.HasOne(c => c.Ticket)
             .WithMany(t => t.Comments)
             .HasForeignKey(c => c.TicketId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(c => c.CreatedBy)
             .WithMany(u => u.Comments)
             .HasForeignKey(c => c.CreatedById)
             .OnDelete(DeleteBehavior.Restrict);
        });

        Seed(modelBuilder);
    }

    private static void Seed(ModelBuilder modelBuilder)
    {
        var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Name = "Alice Admin",   Email = "alice@support.dev",  Role = UserRole.Admin    },
            new User { Id = 2, Name = "Bob Agent",     Email = "bob@support.dev",    Role = UserRole.Agent    },
            new User { Id = 3, Name = "Carol Agent",   Email = "carol@support.dev",  Role = UserRole.Agent    },
            new User { Id = 4, Name = "Dave Customer", Email = "dave@support.dev",   Role = UserRole.Customer }
        );

        modelBuilder.Entity<Ticket>().HasData(
            new Ticket
            {
                Id          = 1,
                Title       = "Login page throws 500 on bad credentials",
                Description = "Entering wrong password causes an unhandled server error instead of a validation message.",
                Priority    = TicketPriority.High,
                Status      = TicketStatus.Open,
                CreatedById = 4,
                AssignedToId = null,
                CreatedAt   = seedDate,
                UpdatedAt   = seedDate
            },
            new Ticket
            {
                Id          = 2,
                Title       = "Export to CSV not working for large datasets",
                Description = "Exporting more than 1000 rows causes a timeout. Need pagination or async export.",
                Priority    = TicketPriority.Medium,
                Status      = TicketStatus.InProgress,
                CreatedById = 4,
                AssignedToId = 2,
                CreatedAt   = seedDate,
                UpdatedAt   = seedDate
            },
            new Ticket
            {
                Id          = 3,
                Title       = "Update email notification template",
                Description = "The confirmation email still shows the old company logo. Update to new branding.",
                Priority    = TicketPriority.Low,
                Status      = TicketStatus.Resolved,
                CreatedById = 4,
                AssignedToId = 3,
                CreatedAt   = seedDate,
                UpdatedAt   = seedDate
            }
        );

        modelBuilder.Entity<Comment>().HasData(
            new Comment
            {
                Id          = 1,
                TicketId    = 2,
                Message     = "Reproduced locally. Looks like the query is missing an index. Investigating.",
                CreatedById = 2,
                CreatedAt   = seedDate
            },
            new Comment
            {
                Id          = 2,
                TicketId    = 3,
                Message     = "New logo applied. Awaiting sign-off from the design team.",
                CreatedById = 3,
                CreatedAt   = seedDate
            }
        );
    }
}
