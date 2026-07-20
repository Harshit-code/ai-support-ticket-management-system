using Microsoft.AspNetCore.Mvc;
using SupportTickets.Api.DTOs;
using SupportTickets.Domain.Entities;
using SupportTickets.Domain.Exceptions;
using SupportTickets.Domain.Interfaces;

namespace SupportTickets.Api.Controllers;

/// <summary>Manage support tickets.</summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _tickets;

    public TicketsController(ITicketService tickets) => _tickets = tickets;

    /// <summary>Get all tickets ordered by creation date descending.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _tickets.GetAllAsync());

    /// <summary>Get a single ticket with its comments.</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ticket = await _tickets.GetByIdAsync(id);
        return ticket is null ? NotFound(new { error = $"Ticket {id} not found." }) : Ok(ticket);
    }

    /// <summary>Create a new ticket. Status is always set to Open.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTicketRequest req)
    {
        var ticket = new Ticket
        {
            Title        = req.Title,
            Description  = req.Description,
            Priority     = req.Priority,
            CreatedById  = req.CreatedById,
            AssignedToId = req.AssignedToId
        };

        var created = await _tickets.CreateAsync(ticket);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Update title, description, priority, and assignee. Status is not updated here.</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTicketRequest req)
    {
        var ticket = new Ticket
        {
            Title        = req.Title,
            Description  = req.Description,
            Priority     = req.Priority,
            AssignedToId = req.AssignedToId
        };

        var updated = await _tickets.UpdateAsync(id, ticket);
        return updated is null ? NotFound(new { error = $"Ticket {id} not found." }) : Ok(updated);
    }

    /// <summary>
    /// Transition a ticket's status. Allowed transitions:
    /// Open → InProgress | Cancelled,
    /// InProgress → Resolved | Cancelled,
    /// Resolved → Closed.
    /// Closed and Cancelled are terminal states.
    /// </summary>
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] PatchTicketStatusRequest req)
    {
        try
        {
            var updated = await _tickets.TransitionStatusAsync(id, req.NewStatus);
            return Ok(updated);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { error = $"Ticket {id} not found." });
        }
        catch (InvalidTransitionException ex)
        {
            return BadRequest(new
            {
                error   = ex.Message,
                from    = ex.From.ToString(),
                to      = ex.To.ToString(),
                allowed = ex.Allowed.Select(s => s.ToString())
            });
        }
    }
}

