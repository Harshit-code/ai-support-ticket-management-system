using Microsoft.EntityFrameworkCore;
using SupportTickets.Domain.Entities;
using SupportTickets.Domain.Interfaces;
using SupportTickets.Infrastructure.Data;

namespace SupportTickets.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<User>> GetAllAsync()
        => await _db.Users
            .AsNoTracking()
            .OrderBy(u => u.Name)
            .ToListAsync();

    public async Task<User?> GetByIdAsync(int id)
        => await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
}
