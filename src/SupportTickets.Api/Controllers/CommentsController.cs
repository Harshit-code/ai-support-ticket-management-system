using Microsoft.AspNetCore.Mvc;
using SupportTickets.Api.DTOs;
using SupportTickets.Domain.Entities;
using SupportTickets.Domain.Interfaces;

namespace SupportTickets.Api.Controllers;

/// <summary>Manage comments on a ticket.</summary>
[ApiController]
[Route("api/tickets/{ticketId:int}/comments")]
[Produces("application/json")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _comments;

    public CommentsController(ICommentService comments) => _comments = comments;

    /// <summary>Get all comments for a ticket ordered oldest first.</summary>
    [HttpGet]
    public async Task<IActionResult> GetByTicketId(int ticketId)
        => Ok(await _comments.GetByTicketIdAsync(ticketId));

    /// <summary>Add a comment to a ticket. Returns 404 if the ticket does not exist.</summary>
    [HttpPost]
    public async Task<IActionResult> Create(int ticketId, [FromBody] CreateCommentRequest req)
    {
        try
        {
            var comment = new Comment
            {
                Message     = req.Message.Trim(),
                CreatedById = req.CreatedById
            };

            var created = await _comments.AddAsync(ticketId, comment);
            return StatusCode(201, created);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { error = $"Ticket {ticketId} not found." });
        }
    }
}
