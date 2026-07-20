using SupportTicketSystem.Core.Entities;

namespace SupportTicketSystem.Core.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
}
