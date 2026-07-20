using Microsoft.EntityFrameworkCore;
using SupportTicketSystem.Core.Entities;
using SupportTicketSystem.Core.Interfaces;
using SupportTicketSystem.Infrastructure.Data;

namespace SupportTicketSystem.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<User>> GetAllAsync()
        => await _db.Users.AsNoTracking().ToListAsync();

    public async Task<User?> GetByIdAsync(int id)
        => await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
}
