using SupportTickets.Domain.Entities;

namespace SupportTickets.Domain.Interfaces;

public interface ICommentService
{
    Task<IEnumerable<Comment>> GetByTicketIdAsync(int ticketId);
    Task<Comment> AddAsync(int ticketId, Comment comment);
}
