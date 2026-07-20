using SupportTicketSystem.Core.Entities;

namespace SupportTicketSystem.Core.Interfaces;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
}
