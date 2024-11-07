using System.ComponentModel.DataAnnotations;

namespace backendassign2.DTOs;
public class LoginDTO
{
    [Required]
    [EmailAddress]
    public string? UserName { get; set; }
    [Required]
    public string? Password { get; set; }
}