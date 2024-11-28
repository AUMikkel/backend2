using System.ComponentModel.DataAnnotations;

namespace backendassign3.DTOs;
public class LoginDTO
{
    [Required]
    [EmailAddress]
    public string? UserName { get; set; }
    [Required]
    public string? Password { get; set; }
}