using SupportTickets.Domain.Enums;

namespace SupportTickets.Domain.StateMachine;

/// <summary>
/// Single source of truth for all allowed ticket status transitions.
/// Closed and Cancelled are terminal — no outbound transitions.
/// </summary>
public static class TicketStatusTransitions
{
    private static readonly IReadOnlyDictionary<TicketStatus, IReadOnlySet<TicketStatus>> _map =
        new Dictionary<TicketStatus, IReadOnlySet<TicketStatus>>
        {
            [TicketStatus.Open]       = new HashSet<TicketStatus> { TicketStatus.InProgress, TicketStatus.Cancelled },
            [TicketStatus.InProgress] = new HashSet<TicketStatus> { TicketStatus.Resolved,   TicketStatus.Cancelled },
            [TicketStatus.Resolved]   = new HashSet<TicketStatus> { TicketStatus.Closed },
            [TicketStatus.Closed]     = new HashSet<TicketStatus>(),
            [TicketStatus.Cancelled]  = new HashSet<TicketStatus>(),
        };

    public static bool IsAllowed(TicketStatus from, TicketStatus to)
        => _map.TryGetValue(from, out var allowed) && allowed.Contains(to);

    public static IEnumerable<TicketStatus> GetAllowed(TicketStatus from)
        => _map.TryGetValue(from, out var allowed) ? allowed : Enumerable.Empty<TicketStatus>();
}
