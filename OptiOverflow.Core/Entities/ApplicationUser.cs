using Microsoft.AspNetCore.Identity;

namespace OptiOverflow.Core.Entities;

public class ApplicationUser: IdentityUser<Guid>
{
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Address { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }
}