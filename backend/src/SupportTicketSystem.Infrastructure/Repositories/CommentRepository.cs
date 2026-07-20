using Microsoft.EntityFrameworkCore;
using SupportTicketSystem.Core.Entities;
using SupportTicketSystem.Core.Interfaces;
using SupportTicketSystem.Core.StateMachine;
using SupportTicketSystem.Infrastructure.Data;

namespace SupportTicketSystem.Infrastructure.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly AppDbContext _db;

    public CommentRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<Comment>> GetByTicketIdAsync(int ticketId)
        => await _db.Comments
            .AsNoTracking()
            .Where(c => c.TicketId == ticketId)
            .Include(c => c.CreatedBy)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync();

    public async Task<Comment> CreateAsync(Comment comment)
    {
        var ticket = await _db.Tickets.FindAsync(comment.TicketId)
            ?? throw new KeyNotFoundException($"Ticket {comment.TicketId} not found.");

        if (TicketStatusTransitions.IsTerminal(ticket.Status))
            throw new InvalidOperationException(
                $"Cannot add a comment to a {ticket.Status} ticket.");

        comment.CreatedAt = DateTime.UtcNow;
        _db.Comments.Add(comment);
        await _db.SaveChangesAsync();
        return comment;
    }
}
