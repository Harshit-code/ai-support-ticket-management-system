using SupportTickets.Domain.Entities;
using SupportTickets.Domain.Interfaces;

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
}
