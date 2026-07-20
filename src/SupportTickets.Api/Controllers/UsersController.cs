using Microsoft.AspNetCore.Mvc;
using SupportTickets.Domain.Interfaces;

namespace SupportTickets.Api.Controllers;

/// <summary>Read seeded users (no create/update — users are seeded only).</summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _users;

    public UsersController(IUserRepository users) => _users = users;

    /// <summary>Get all users ordered by name.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _users.GetAllAsync());

    /// <summary>Get a single user by id.</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _users.GetByIdAsync(id);
        return user is null ? NotFound(new { error = $"User {id} not found." }) : Ok(user);
    }
}
