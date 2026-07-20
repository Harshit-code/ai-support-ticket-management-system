using System.ComponentModel.DataAnnotations;
using SupportTickets.Domain.Enums;

namespace SupportTickets.Api.DTOs;

public class CreateTicketRequest
{
    [Required(ErrorMessage = "Title is required.")]
    [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required.")]
    [MaxLength(2000, ErrorMessage = "Description cannot exceed 2000 characters.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Priority is required.")]
    [EnumDataType(typeof(TicketPriority), ErrorMessage = "Invalid priority. Allowed: Low, Medium, High, Critical.")]
    public TicketPriority Priority { get; set; }

    [Required(ErrorMessage = "CreatedById is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "CreatedById must be a valid user id.")]
    public int CreatedById { get; set; }

    public int? AssignedToId { get; set; }
}
