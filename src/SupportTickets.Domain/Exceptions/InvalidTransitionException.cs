using SupportTickets.Domain.Enums;

namespace SupportTickets.Domain.Exceptions;

public class InvalidTransitionException : Exception
{
    public TicketStatus From { get; }
    public TicketStatus To { get; }
    public IEnumerable<TicketStatus> Allowed { get; }

    public InvalidTransitionException(TicketStatus from, TicketStatus to, IEnumerable<TicketStatus> allowed)
        : base(BuildMessage(from, to, allowed))
    {
        From    = from;
        To      = to;
        Allowed = allowed;
    }

    private static string BuildMessage(TicketStatus from, TicketStatus to, IEnumerable<TicketStatus> allowed)
    {
        var allowedList = allowed.ToList();
        var allowedText = allowedList.Count > 0
            ? string.Join(", ", allowedList)
            : "none — this is a terminal state";

        return $"Cannot transition from '{from}' to '{to}'. Valid transitions from '{from}': {allowedText}.";
    }
}
