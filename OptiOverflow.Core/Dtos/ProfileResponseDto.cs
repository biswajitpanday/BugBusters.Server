using BugBusters.Server.Core.Entities;
using BugBusters.Server.Core.Interfaces.Common;

namespace BugBusters.Server.Core.Dtos;

public class ProfileResponseDto : IMapFrom<ApplicationUser>
{
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
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
        if (!string.IsNullOrEmpty(MiddleName))
            fullName += $" {MiddleName}";
        if (!string.IsNullOrEmpty(LastName))
            fullName += $" {LastName}"; ;
        return fullName;
    }
}