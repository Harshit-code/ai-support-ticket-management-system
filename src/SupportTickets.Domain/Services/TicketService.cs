using SupportTickets.Domain.Entities;
using SupportTickets.Domain.Enums;
using SupportTickets.Domain.Exceptions;
using SupportTickets.Domain.Interfaces;
using SupportTickets.Domain.StateMachine;

namespace SupportTickets.Domain.Services;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _repo;

    public TicketService(ITicketRepository repo) => _repo = repo;

    public Task<IEnumerable<Ticket>> GetAllAsync() => _repo.GetAllAsync();

    public Task<Ticket?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

    public Task<Ticket> CreateAsync(Ticket ticket) => _repo.CreateAsync(ticket);

    public Task<Ticket?> UpdateAsync(int id, Ticket ticket)
    {
        ticket.Id = id;
        return _repo.UpdateAsync(ticket);
    }

    public async Task<Ticket> TransitionStatusAsync(int id, TicketStatus newStatus)
    {
        var ticket = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Ticket {id} not found.");

        if (!TicketStatusTransitions.IsAllowed(ticket.Status, newStatus))
            throw new InvalidTransitionException(
                ticket.Status,
                newStatus,
                TicketStatusTransitions.GetAllowed(ticket.Status));

        return (await _repo.UpdateStatusAsync(id, newStatus))!;
    }
}
