namespace API.DTOs;
using System.ComponentModel.DataAnnotations;
public class RegisterDto
{
    [Required]
    public string DisplayName { get; set; } = string.Empty;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    [StringLength(100, MinimumLength = 4)]
    public string Password { get; set; } = string.Empty;
}
