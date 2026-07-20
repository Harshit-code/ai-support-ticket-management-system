using SupportTicketSystem.Core.Entities;
using SupportTicketSystem.Core.Interfaces;

namespace SupportTicketSystem.Core.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;

    public UserService(IUserRepository repo) => _repo = repo;

    public Task<IEnumerable<User>> GetAllAsync() => _repo.GetAllAsync();

    public Task<User?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
}
