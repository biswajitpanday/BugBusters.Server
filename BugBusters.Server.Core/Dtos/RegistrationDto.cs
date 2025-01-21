using System.ComponentModel.DataAnnotations;
using BugBusters.Server.Core.Entities;
using BugBusters.Server.Core.Interfaces.Common;

namespace BugBusters.Server.Core.Dtos;

public class RegistrationDto : IMapFrom<ApplicationUser>
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
    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Address { get; set; }
}