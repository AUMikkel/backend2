using System.ComponentModel.DataAnnotations;

namespace backendassign3.DTOs;
public class RegisterDTO
{
    [Required]
    public string? FullName { get; set; }
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    [Required]
    public string? Password { get; set; }
    [Required]
    public string? Address { get; set; }
    [Required]
    public string? PhoneNo { get; set; }
}