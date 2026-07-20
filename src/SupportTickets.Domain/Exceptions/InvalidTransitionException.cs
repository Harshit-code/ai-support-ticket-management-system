using SupportTickets.Domain.Enums;

namespace SupportTickets.Domain.Exceptions;

public class InvalidTransitionException : Exception
{
    public TicketStatus From { get; }
    public TicketStatus To { get; }
    public IEnumerable<TicketStatus> Allowed { get; }

    public InvalidTransitionException(TicketStatus from, TicketStatus to, IEnumerable<TicketStatus> allowed)
        : base($"Cannot transition from '{from}' to '{to}'.")
    {
        From    = from;
        To      = to;
        Allowed = allowed;
    }
}
