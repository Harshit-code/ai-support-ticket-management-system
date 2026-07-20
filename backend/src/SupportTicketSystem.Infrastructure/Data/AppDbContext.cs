using Microsoft.EntityFrameworkCore;
using SupportTicketSystem.Core.Entities;
using SupportTicketSystem.Core.Enums;

namespace SupportTicketSystem.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(200);
            entity.HasIndex(u => u.Email).IsUnique();
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Title).IsRequired().HasMaxLength(200);
            entity.Property(t => t.Description).IsRequired().HasMaxLength(2000);

            entity.HasOne(t => t.CreatedBy)
                  .WithMany(u => u.CreatedTickets)
                  .HasForeignKey(t => t.CreatedById)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(t => t.AssignedTo)
                  .WithMany(u => u.AssignedTickets)
                  .HasForeignKey(t => t.AssignedToId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Message).IsRequired().HasMaxLength(1000);

            entity.HasOne(c => c.Ticket)
                  .WithMany(t => t.Comments)
                  .HasForeignKey(c => c.TicketId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(c => c.CreatedBy)
                  .WithMany(u => u.Comments)
                  .HasForeignKey(c => c.CreatedById)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        SeedUsers(modelBuilder);
    }

    private static void SeedUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Name = "Alice Admin",    Email = "alice@support.dev",  Role = UserRole.Admin    },
            new User { Id = 2, Name = "Bob Agent",      Email = "bob@support.dev",    Role = UserRole.Agent    },
            new User { Id = 3, Name = "Carol Agent",    Email = "carol@support.dev",  Role = UserRole.Agent    },
            new User { Id = 4, Name = "Dave Customer",  Email = "dave@support.dev",   Role = UserRole.Customer },
            new User { Id = 5, Name = "Eve Customer",   Email = "eve@support.dev",    Role = UserRole.Customer }
        );
    }
}
