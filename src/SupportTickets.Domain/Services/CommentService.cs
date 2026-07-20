using SupportTickets.Domain.Entities;
using SupportTickets.Domain.Interfaces;

namespace SupportTickets.Domain.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _comments;
    private readonly ITicketRepository  _tickets;

    public CommentService(ICommentRepository comments, ITicketRepository tickets)
    {
        _comments = comments;
        _tickets  = tickets;
    }

    public async Task<IEnumerable<Comment>> GetByTicketIdAsync(int ticketId)
    {
        var ticket = await _tickets.GetByIdAsync(ticketId)
            ?? throw new KeyNotFoundException($"Ticket {ticketId} not found.");

        return await _comments.GetByTicketIdAsync(ticket.Id);
    }

    public async Task<Comment> AddAsync(int ticketId, Comment comment)
    {
        var ticket = await _tickets.GetByIdAsync(ticketId)
            ?? throw new KeyNotFoundException($"Ticket {ticketId} not found.");

        comment.TicketId = ticket.Id;
        return await _comments.CreateAsync(comment);
    }
}
