using Microsoft.AspNetCore.Mvc;
using Moq;
using SupportTickets.Api.Controllers;
using SupportTickets.Api.DTOs;
using SupportTickets.Domain.Entities;
using SupportTickets.Domain.Enums;
using SupportTickets.Domain.Exceptions;
using SupportTickets.Domain.Interfaces;
using Xunit;

namespace SupportTickets.Api.Tests.Controllers;

/// <summary>
/// Controller-layer unit tests for PATCH /api/tickets/{id}/status.
/// ITicketService is mocked — no DB, no HTTP pipeline.
/// </summary>
public class TicketStatusTransitionControllerTests
{
    private readonly Mock<ITicketService> _mockService;
    private readonly TicketsController    _controller;

    public TicketStatusTransitionControllerTests()
    {
        _mockService = new Mock<ITicketService>(MockBehavior.Strict);
        _controller  = new TicketsController(_mockService.Object);
    }

    // -------------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------------

    private static Ticket MakeTicket(int id, TicketStatus status) => new()
    {
        Id          = id,
        Title       = "Test Ticket",
        Description = "Test",
        Status      = status,
        Priority    = TicketPriority.Medium,
        CreatedAt   = DateTime.UtcNow,
        UpdatedAt   = DateTime.UtcNow,
        CreatedById = 1
    };

    private static PatchTicketStatusRequest Req(TicketStatus s) =>
        new() { NewStatus = s };

    // -------------------------------------------------------------------------
    // Valid transitions → 200 OK
    // -------------------------------------------------------------------------

    [Theory]
    [InlineData(TicketStatus.InProgress)]
    [InlineData(TicketStatus.Cancelled)]
    [InlineData(TicketStatus.Resolved)]
    [InlineData(TicketStatus.Closed)]
    public async Task UpdateStatus_ValidTransition_Returns200WithUpdatedTicket(TicketStatus to)
    {
        var updated = MakeTicket(1, to);
        _mockService.Setup(s => s.TransitionStatusAsync(1, to))
                    .ReturnsAsync(updated);

        var result = await _controller.UpdateStatus(1, Req(to));

        var ok = Assert.IsType<OkObjectResult>(result);
        var ticket = Assert.IsType<Ticket>(ok.Value);
        Assert.Equal(to, ticket.Status);
    }

    // -------------------------------------------------------------------------
    // Invalid transition → 409 Conflict with structured body
    // -------------------------------------------------------------------------

    [Theory]
    [InlineData(TicketStatus.Open,       TicketStatus.Resolved)]   // skip
    [InlineData(TicketStatus.Open,       TicketStatus.Closed)]     // skip
    [InlineData(TicketStatus.Open,       TicketStatus.Open)]       // same
    [InlineData(TicketStatus.InProgress, TicketStatus.Open)]       // backward
    [InlineData(TicketStatus.InProgress, TicketStatus.Closed)]     // skip
    [InlineData(TicketStatus.InProgress, TicketStatus.InProgress)] // same
    [InlineData(TicketStatus.Resolved,   TicketStatus.Open)]       // backward
    [InlineData(TicketStatus.Resolved,   TicketStatus.InProgress)] // backward
    [InlineData(TicketStatus.Resolved,   TicketStatus.Cancelled)]  // not allowed
    [InlineData(TicketStatus.Resolved,   TicketStatus.Resolved)]   // same
    [InlineData(TicketStatus.Closed,     TicketStatus.Open)]       // terminal
    [InlineData(TicketStatus.Closed,     TicketStatus.InProgress)] // terminal
    [InlineData(TicketStatus.Closed,     TicketStatus.Resolved)]   // terminal
    [InlineData(TicketStatus.Closed,     TicketStatus.Cancelled)]  // terminal
    [InlineData(TicketStatus.Closed,     TicketStatus.Closed)]     // terminal + same
    [InlineData(TicketStatus.Cancelled,  TicketStatus.Open)]       // terminal
    [InlineData(TicketStatus.Cancelled,  TicketStatus.InProgress)] // terminal
    [InlineData(TicketStatus.Cancelled,  TicketStatus.Resolved)]   // terminal
    [InlineData(TicketStatus.Cancelled,  TicketStatus.Closed)]     // terminal
    [InlineData(TicketStatus.Cancelled,  TicketStatus.Cancelled)]  // terminal + same
    public async Task UpdateStatus_InvalidTransition_Returns409Conflict(
        TicketStatus from, TicketStatus to)
    {
        var allowed   = Domain.StateMachine.TicketStatusTransitions.GetAllowed(from);
        var exception = new InvalidTransitionException(from, to, allowed);

        _mockService.Setup(s => s.TransitionStatusAsync(1, to))
                    .ThrowsAsync(exception);

        var result = await _controller.UpdateStatus(1, Req(to));

        Assert.IsType<ConflictObjectResult>(result);
    }

    // -------------------------------------------------------------------------
    // 409 response body carries error / from / to / allowed fields
    // -------------------------------------------------------------------------

    [Fact]
    public async Task UpdateStatus_InvalidTransition_ResponseBodyContainsStructuredError()
    {
        var exception = new InvalidTransitionException(
            TicketStatus.Open,
            TicketStatus.Closed,
            new[] { TicketStatus.InProgress, TicketStatus.Cancelled });

        _mockService.Setup(s => s.TransitionStatusAsync(1, TicketStatus.Closed))
                    .ThrowsAsync(exception);

        var result  = await _controller.UpdateStatus(1, Req(TicketStatus.Closed));
        var conflict = Assert.IsType<ConflictObjectResult>(result);

        // The body is an anonymous object; inspect via reflection
        var body    = conflict.Value!;
        var type    = body.GetType();

        var errorProp   = type.GetProperty("error");
        var fromProp    = type.GetProperty("from");
        var toProp      = type.GetProperty("to");
        var allowedProp = type.GetProperty("allowed");

        Assert.NotNull(errorProp);
        Assert.NotNull(fromProp);
        Assert.NotNull(toProp);
        Assert.NotNull(allowedProp);

        Assert.Equal("Open",   fromProp!.GetValue(body));
        Assert.Equal("Closed", toProp!.GetValue(body));

        var allowedValues = ((IEnumerable<string>)allowedProp!.GetValue(body)!).ToList();
        Assert.Contains("InProgress", allowedValues);
        Assert.Contains("Cancelled",  allowedValues);
    }

    // -------------------------------------------------------------------------
    // Terminal states: Closed and Cancelled — 409 with empty "allowed" list
    // -------------------------------------------------------------------------

    [Theory]
    [InlineData(TicketStatus.Closed,    TicketStatus.Open)]
    [InlineData(TicketStatus.Cancelled, TicketStatus.Open)]
    public async Task UpdateStatus_FromTerminalState_Returns409WithEmptyAllowed(
        TicketStatus from, TicketStatus to)
    {
        var exception = new InvalidTransitionException(from, to, Enumerable.Empty<TicketStatus>());

        _mockService.Setup(s => s.TransitionStatusAsync(1, to))
                    .ThrowsAsync(exception);

        var result   = await _controller.UpdateStatus(1, Req(to));
        var conflict  = Assert.IsType<ConflictObjectResult>(result);

        var body         = conflict.Value!;
        var allowedProp  = body.GetType().GetProperty("allowed");
        var allowedValues = ((IEnumerable<string>)allowedProp!.GetValue(body)!).ToList();

        Assert.Empty(allowedValues);
    }

    // -------------------------------------------------------------------------
    // Ticket not found → 404
    // -------------------------------------------------------------------------

    [Fact]
    public async Task UpdateStatus_TicketNotFound_Returns404()
    {
        _mockService.Setup(s => s.TransitionStatusAsync(99, TicketStatus.InProgress))
                    .ThrowsAsync(new KeyNotFoundException("Ticket 99 not found."));

        var result = await _controller.UpdateStatus(99, Req(TicketStatus.InProgress));

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task UpdateStatus_TicketNotFound_ResponseBodyContainsErrorMessage()
    {
        _mockService.Setup(s => s.TransitionStatusAsync(99, TicketStatus.InProgress))
                    .ThrowsAsync(new KeyNotFoundException("Ticket 99 not found."));

        var result = await _controller.UpdateStatus(99, Req(TicketStatus.InProgress));
        var notFound = Assert.IsType<NotFoundObjectResult>(result);

        var errorProp = notFound.Value!.GetType().GetProperty("error");
        Assert.NotNull(errorProp);

        var msg = errorProp!.GetValue(notFound.Value)?.ToString();
        Assert.Contains("99", msg);
    }

    // -------------------------------------------------------------------------
    // Service is called exactly once per request — no duplicate calls
    // -------------------------------------------------------------------------

    [Fact]
    public async Task UpdateStatus_ValidRequest_CallsServiceExactlyOnce()
    {
        var updated = MakeTicket(1, TicketStatus.InProgress);
        _mockService.Setup(s => s.TransitionStatusAsync(1, TicketStatus.InProgress))
                    .ReturnsAsync(updated);

        await _controller.UpdateStatus(1, Req(TicketStatus.InProgress));

        _mockService.Verify(s => s.TransitionStatusAsync(1, TicketStatus.InProgress), Times.Once);
    }
}
