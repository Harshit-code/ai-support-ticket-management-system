using SupportTickets.Domain.Entities;

namespace SupportTickets.Domain.Interfaces;

public interface ITicketService
{
    Task<IEnumerable<Ticket>> GetAllAsync();
    Task<Ticket?> GetByIdAsync(int id);
    Task<Ticket> CreateAsync(Ticket ticket);
    Task<Ticket?> UpdateAsync(int id, Ticket ticket);
}
