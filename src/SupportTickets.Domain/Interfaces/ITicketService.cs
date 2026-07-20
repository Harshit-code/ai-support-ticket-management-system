using SupportTickets.Domain.Entities;
using SupportTickets.Domain.Enums;

namespace SupportTickets.Domain.Interfaces;

public interface ITicketService
{
    Task<IEnumerable<Ticket>> GetAllAsync(string? keyword = null, TicketStatus? status = null);
    Task<Ticket?> GetByIdAsync(int id);
    Task<Ticket> CreateAsync(Ticket ticket);
    Task<Ticket?> UpdateAsync(int id, Ticket ticket);
    Task<Ticket> TransitionStatusAsync(int id, TicketStatus newStatus);
}
