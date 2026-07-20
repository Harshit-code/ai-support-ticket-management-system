using SupportTicketSystem.Core.Entities;
using SupportTicketSystem.Core.Enums;

namespace SupportTicketSystem.Core.Interfaces;

public interface ITicketService
{
    Task<IEnumerable<Ticket>> GetAllAsync();
    Task<Ticket?> GetByIdAsync(int id);
    Task<Ticket> CreateAsync(Ticket ticket);
    Task<Ticket?> UpdateAsync(Ticket ticket);
    Task<Ticket?> TransitionStatusAsync(int id, TicketStatus newStatus);
}
