namespace SupportTicketSystem.Core.Entities;

public class Comment
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;

    public int TicketId { get; set; }
    public Ticket Ticket { get; set; } = null!;

    public int CreatedById { get; set; }
    public User CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
