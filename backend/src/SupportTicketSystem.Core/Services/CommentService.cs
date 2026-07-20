using SupportTicketSystem.Core.Entities;
using SupportTicketSystem.Core.Interfaces;
using SupportTicketSystem.Core.StateMachine;

namespace SupportTicketSystem.Core.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepo;
    private readonly ITicketRepository _ticketRepo;

    public CommentService(ICommentRepository commentRepo, ITicketRepository ticketRepo)
    {
        _commentRepo = commentRepo;
        _ticketRepo  = ticketRepo;
    }

    public Task<IEnumerable<Comment>> GetByTicketIdAsync(int ticketId)
        => _commentRepo.GetByTicketIdAsync(ticketId);

    public async Task<Comment> CreateAsync(Comment comment)
    {
        var ticket = await _ticketRepo.GetByIdAsync(comment.TicketId)
            ?? throw new KeyNotFoundException($"Ticket {comment.TicketId} not found.");

        if (TicketStatusTransitions.IsTerminal(ticket.Status))
            throw new InvalidOperationException(
                $"Cannot add a comment to a {ticket.Status} ticket.");

        return await _commentRepo.CreateAsync(comment);
    }
}
