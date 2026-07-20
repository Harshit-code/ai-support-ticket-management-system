using SupportTicketSystem.Core.Entities;
using SupportTicketSystem.Core.Enums;
using SupportTicketSystem.Core.Interfaces;
using SupportTicketSystem.Core.StateMachine;

namespace SupportTicketSystem.Core.Services;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _repo;

    public TicketService(ITicketRepository repo) => _repo = repo;

    public Task<IEnumerable<Ticket>> GetAllAsync() => _repo.GetAllAsync();

    public Task<Ticket?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

    public Task<Ticket> CreateAsync(Ticket ticket) => _repo.CreateAsync(ticket);

    public Task<Ticket?> UpdateAsync(Ticket ticket) => _repo.UpdateAsync(ticket);

    public async Task<Ticket?> TransitionStatusAsync(int id, TicketStatus newStatus)
    {
        var ticket = await _repo.GetByIdAsync(id);
        if (ticket is null) return null;

        if (!TicketStatusTransitions.IsValid(ticket.Status, newStatus))
            throw new InvalidOperationException(
                $"Transition from {ticket.Status} to {newStatus} is not allowed. " +
                $"Allowed: [{string.Join(", ", TicketStatusTransitions.GetAllowed(ticket.Status))}]");

        return await _repo.UpdateStatusAsync(id, newStatus);
    }
}
