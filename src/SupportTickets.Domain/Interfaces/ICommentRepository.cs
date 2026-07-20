using SupportTickets.Domain.Entities;

namespace SupportTickets.Domain.Interfaces;

public interface ICommentRepository
{
    Task<IEnumerable<Comment>> GetByTicketIdAsync(int ticketId);
    Task<Comment> CreateAsync(Comment comment);
}
