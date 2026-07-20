using Microsoft.EntityFrameworkCore;
using SupportTickets.Domain.Entities;
using SupportTickets.Domain.Enums;
using SupportTickets.Domain.Interfaces;
using SupportTickets.Infrastructure.Data;

namespace SupportTickets.Infrastructure.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly AppDbContext _db;

    public TicketRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<Ticket>> GetAllAsync()
        => await _db.Tickets
            .AsNoTracking()
            .Include(t => t.CreatedBy)
            .Include(t => t.AssignedTo)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

    public async Task<Ticket?> GetByIdAsync(int id)
        => await _db.Tickets
            .AsNoTracking()
            .Include(t => t.CreatedBy)
            .Include(t => t.AssignedTo)
            .Include(t => t.Comments).ThenInclude(c => c.CreatedBy)
            .FirstOrDefaultAsync(t => t.Id == id);

    public async Task<Ticket> CreateAsync(Ticket ticket)
    {
        ticket.Status    = TicketStatus.Open;
        ticket.CreatedAt = DateTime.UtcNow;
        ticket.UpdatedAt = DateTime.UtcNow;
        _db.Tickets.Add(ticket);
        await _db.SaveChangesAsync();
        return ticket;
    }

    public async Task<Ticket?> UpdateAsync(Ticket ticket)
    {
        var existing = await _db.Tickets.FindAsync(ticket.Id);
        if (existing is null) return null;

        existing.Title        = ticket.Title;
        existing.Description  = ticket.Description;
        existing.Priority     = ticket.Priority;
        existing.AssignedToId = ticket.AssignedToId;
        existing.UpdatedAt    = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return existing;
    }
}
