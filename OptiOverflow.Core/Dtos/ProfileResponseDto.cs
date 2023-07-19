using OptiOverflow.Core.Entities;
using OptiOverflow.Core.Interfaces.Common;

namespace OptiOverflow.Core.Dtos;

public class ProfileResponseDto : IMapFrom<ApplicationUser>
{
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set;}
    public string? FullName => GetFullName();
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }

    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdate { get; set; }

    private string? GetFullName()
    {
        var fullName = FirstName;
        if (MiddleName != null)
            fullName += $" {MiddleName}";
        if (LastName != null)
            fullName += $" {LastName}"; ;
        return fullName;
    }
}