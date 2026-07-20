using System.ComponentModel.DataAnnotations;

namespace SupportTickets.Api.DTOs;

public class CreateCommentRequest
{
    [Required(ErrorMessage = "Message is required.")]
    [MinLength(1, ErrorMessage = "Message cannot be empty.")]
    [MaxLength(1000, ErrorMessage = "Message cannot exceed 1000 characters.")]
    public string Message { get; set; } = string.Empty;

    [Required(ErrorMessage = "CreatedById is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "CreatedById must be a valid user id.")]
    public int CreatedById { get; set; }
}
