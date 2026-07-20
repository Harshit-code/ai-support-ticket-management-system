using SupportTickets.Domain.Enums;
using SupportTickets.Domain.StateMachine;
using Xunit;

namespace SupportTickets.Api.Tests.StateMachine;

/// <summary>
/// Covers all 25 from→to combinations from the transition table.
/// No mocks needed — TicketStatusTransitions is a pure static class.
/// </summary>
public class TicketStatusTransitionsTests
{
    // -------------------------------------------------------------------------
    // Valid transitions (5 of 25)
    // -------------------------------------------------------------------------

    [Theory]
    [InlineData(TicketStatus.Open,       TicketStatus.InProgress)]  // row 2
    [InlineData(TicketStatus.Open,       TicketStatus.Cancelled)]   // row 5
    [InlineData(TicketStatus.InProgress, TicketStatus.Resolved)]    // row 8
    [InlineData(TicketStatus.InProgress, TicketStatus.Cancelled)]   // row 10
    [InlineData(TicketStatus.Resolved,   TicketStatus.Closed)]      // row 14
    public void IsAllowed_ValidTransition_ReturnsTrue(TicketStatus from, TicketStatus to)
    {
        Assert.True(TicketStatusTransitions.IsAllowed(from, to));
    }

    // -------------------------------------------------------------------------
    // Invalid: same-state (rows 1, 7, 13, 19, 25)
    // -------------------------------------------------------------------------

    [Theory]
    [InlineData(TicketStatus.Open)]
    [InlineData(TicketStatus.InProgress)]
    [InlineData(TicketStatus.Resolved)]
    [InlineData(TicketStatus.Closed)]
    [InlineData(TicketStatus.Cancelled)]
    public void IsAllowed_SameState_ReturnsFalse(TicketStatus status)
    {
        Assert.False(TicketStatusTransitions.IsAllowed(status, status));
    }

    // -------------------------------------------------------------------------
    // Invalid: forward skips (rows 3, 4, 9)
    // -------------------------------------------------------------------------

    [Theory]
    [InlineData(TicketStatus.Open,       TicketStatus.Resolved)]  // row 3
    [InlineData(TicketStatus.Open,       TicketStatus.Closed)]    // row 4
    [InlineData(TicketStatus.InProgress, TicketStatus.Closed)]    // row 9
    public void IsAllowed_ForwardSkip_ReturnsFalse(TicketStatus from, TicketStatus to)
    {
        Assert.False(TicketStatusTransitions.IsAllowed(from, to));
    }

    // -------------------------------------------------------------------------
    // Invalid: backward transitions (rows 6, 11, 12)
    // -------------------------------------------------------------------------

    [Theory]
    [InlineData(TicketStatus.InProgress, TicketStatus.Open)]        // row 6
    [InlineData(TicketStatus.Resolved,   TicketStatus.Open)]        // row 11
    [InlineData(TicketStatus.Resolved,   TicketStatus.InProgress)]  // row 12
    public void IsAllowed_BackwardTransition_ReturnsFalse(TicketStatus from, TicketStatus to)
    {
        Assert.False(TicketStatusTransitions.IsAllowed(from, to));
    }

    // -------------------------------------------------------------------------
    // Invalid: Resolved → Cancelled (row 15 — notable edge case)
    // -------------------------------------------------------------------------

    [Fact]
    public void IsAllowed_ResolvedToCancelled_ReturnsFalse()
    {
        Assert.False(TicketStatusTransitions.IsAllowed(TicketStatus.Resolved, TicketStatus.Cancelled));
    }

    // -------------------------------------------------------------------------
    // Invalid: terminal states — Closed and Cancelled (rows 16-25)
    // -------------------------------------------------------------------------

    [Theory]
    [InlineData(TicketStatus.Open)]
    [InlineData(TicketStatus.InProgress)]
    [InlineData(TicketStatus.Resolved)]
    [InlineData(TicketStatus.Closed)]
    [InlineData(TicketStatus.Cancelled)]
    public void IsAllowed_FromClosed_AlwaysReturnsFalse(TicketStatus to)
    {
        Assert.False(TicketStatusTransitions.IsAllowed(TicketStatus.Closed, to));
    }

    [Theory]
    [InlineData(TicketStatus.Open)]
    [InlineData(TicketStatus.InProgress)]
    [InlineData(TicketStatus.Resolved)]
    [InlineData(TicketStatus.Closed)]
    [InlineData(TicketStatus.Cancelled)]
    public void IsAllowed_FromCancelled_AlwaysReturnsFalse(TicketStatus to)
    {
        Assert.False(TicketStatusTransitions.IsAllowed(TicketStatus.Cancelled, to));
    }

    // -------------------------------------------------------------------------
    // GetAllowed — correct sets returned per state
    // -------------------------------------------------------------------------

    [Fact]
    public void GetAllowed_Open_ReturnsInProgressAndCancelled()
    {
        var allowed = TicketStatusTransitions.GetAllowed(TicketStatus.Open).ToList();
        Assert.Equal(2, allowed.Count);
        Assert.Contains(TicketStatus.InProgress, allowed);
        Assert.Contains(TicketStatus.Cancelled,  allowed);
    }

    [Fact]
    public void GetAllowed_InProgress_ReturnsResolvedAndCancelled()
    {
        var allowed = TicketStatusTransitions.GetAllowed(TicketStatus.InProgress).ToList();
        Assert.Equal(2, allowed.Count);
        Assert.Contains(TicketStatus.Resolved,  allowed);
        Assert.Contains(TicketStatus.Cancelled, allowed);
    }

    [Fact]
    public void GetAllowed_Resolved_ReturnsClosed()
    {
        var allowed = TicketStatusTransitions.GetAllowed(TicketStatus.Resolved).ToList();
        Assert.Single(allowed);
        Assert.Contains(TicketStatus.Closed, allowed);
    }

    [Fact]
    public void GetAllowed_Closed_ReturnsEmpty()
    {
        Assert.Empty(TicketStatusTransitions.GetAllowed(TicketStatus.Closed));
    }

    [Fact]
    public void GetAllowed_Cancelled_ReturnsEmpty()
    {
        Assert.Empty(TicketStatusTransitions.GetAllowed(TicketStatus.Cancelled));
    }
}
