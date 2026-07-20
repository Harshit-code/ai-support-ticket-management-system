using SupportTicketSystem.Core.Enums;

namespace SupportTicketSystem.Core.StateMachine;

public static class TicketStatusTransitions
{
    private static readonly Dictionary<TicketStatus, IReadOnlySet<TicketStatus>> _allowed =
        new()
        {
            [TicketStatus.Open]       = new HashSet<TicketStatus> { TicketStatus.InProgress, TicketStatus.Cancelled },
            [TicketStatus.InProgress] = new HashSet<TicketStatus> { TicketStatus.Resolved,   TicketStatus.Cancelled },
            [TicketStatus.Resolved]   = new HashSet<TicketStatus> { TicketStatus.Closed },
            [TicketStatus.Closed]     = new HashSet<TicketStatus>(),
            [TicketStatus.Cancelled]  = new HashSet<TicketStatus>(),
        };

    public static bool IsValid(TicketStatus from, TicketStatus to)
        => _allowed.TryGetValue(from, out var targets) && targets.Contains(to);

    public static IReadOnlySet<TicketStatus> GetAllowed(TicketStatus from)
        => _allowed.TryGetValue(from, out var targets) ? targets : new HashSet<TicketStatus>();

    public static bool IsTerminal(TicketStatus status)
        => status is TicketStatus.Closed or TicketStatus.Cancelled;
}
