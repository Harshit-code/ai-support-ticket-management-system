using Microsoft.AspNetCore.Mvc;
using SupportTicketSystem.Core.Entities;
using SupportTicketSystem.Core.Enums;
using SupportTicketSystem.Core.Interfaces;
using SupportTicketSystem.Core.StateMachine;

namespace SupportTicketSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly ITicketRepository _tickets;

    public TicketsController(ITicketRepository tickets) => _tickets = tickets;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _tickets.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ticket = await _tickets.GetByIdAsync(id);
        return ticket is null ? NotFound() : Ok(ticket);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTicketRequest req)
    {
        var ticket = new Ticket
        {
            Title       = req.Title,
            Description = req.Description,
            Priority    = req.Priority,
            CreatedById = req.CreatedById,
            AssignedToId = req.AssignedToId
        };

        var created = await _tickets.CreateAsync(ticket);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTicketRequest req)
    {
        var ticket = new Ticket
        {
            Id           = id,
            Title        = req.Title,
            Description  = req.Description,
            Priority     = req.Priority,
            AssignedToId = req.AssignedToId
        };

        var updated = await _tickets.UpdateAsync(ticket);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> TransitionStatus(int id, [FromBody] TransitionStatusRequest req)
    {
        try
        {
            var updated = await _tickets.TransitionStatusAsync(id, req.NewStatus);
            return updated is null ? NotFound() : Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message, allowed = TicketStatusTransitions.GetAllowed(req.NewStatus) });
        }
    }
}

public record CreateTicketRequest(
    string Title,
    string Description,
    TicketPriority Priority,
    int CreatedById,
    int? AssignedToId
);

public record UpdateTicketRequest(
    string Title,
    string Description,
    TicketPriority Priority,
    int? AssignedToId
);

public record TransitionStatusRequest(TicketStatus NewStatus);
