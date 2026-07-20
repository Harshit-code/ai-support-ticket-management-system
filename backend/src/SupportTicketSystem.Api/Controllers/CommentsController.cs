using Microsoft.AspNetCore.Mvc;
using SupportTicketSystem.Core.Entities;
using SupportTicketSystem.Core.Interfaces;

namespace SupportTicketSystem.Api.Controllers;

[ApiController]
[Route("api/tickets/{ticketId:int}/comments")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _comments;

    public CommentsController(ICommentService comments) => _comments = comments;

    [HttpGet]
    public async Task<IActionResult> GetByTicket(int ticketId)
        => Ok(await _comments.GetByTicketIdAsync(ticketId));

    [HttpPost]
    public async Task<IActionResult> Create(int ticketId, [FromBody] CreateCommentRequest req)
    {
        try
        {
            var comment = new Comment
            {
                TicketId    = ticketId,
                Message     = req.Message,
                CreatedById = req.CreatedById
            };

            var created = await _comments.CreateAsync(comment);
            return StatusCode(201, created);
        }
        catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }
}

public record CreateCommentRequest(string Message, int CreatedById);
