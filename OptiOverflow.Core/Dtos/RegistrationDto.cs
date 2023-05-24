using System.ComponentModel.DataAnnotations;

namespace OptiOverflow.Core.Dtos;

public class RegistrationDto
{
    [Required(ErrorMessage = "UserName is required")]
    public string UserName { get; set; } = null!;
    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = null!;
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Address { get; set; }
}