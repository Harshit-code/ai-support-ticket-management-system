using SupportTicketSystem.Core.Entities;

namespace SupportTicketSystem.Core.Interfaces;

public interface ICommentRepository
{
    Task<IEnumerable<Comment>> GetByTicketIdAsync(int ticketId);
    Task<Comment> CreateAsync(Comment comment);
}
