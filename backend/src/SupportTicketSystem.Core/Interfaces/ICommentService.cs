using SupportTicketSystem.Core.Entities;

namespace SupportTicketSystem.Core.Interfaces;

public interface ICommentService
{
    Task<IEnumerable<Comment>> GetByTicketIdAsync(int ticketId);
    Task<Comment> CreateAsync(Comment comment);
}
