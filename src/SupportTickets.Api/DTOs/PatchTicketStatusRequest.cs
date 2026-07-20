using System.ComponentModel.DataAnnotations;
using SupportTickets.Domain.Enums;

namespace SupportTickets.Api.DTOs;

public class PatchTicketStatusRequest
{
    [Required(ErrorMessage = "NewStatus is required.")]
    [EnumDataType(typeof(TicketStatus), ErrorMessage = "Invalid status. Allowed: Open, InProgress, Resolved, Closed, Cancelled.")]
    public TicketStatus NewStatus { get; set; }
}
