using Microsoft.AspNetCore.Identity;

namespace OptiOverflow.Core.Entities;

public class ApplicationUser: IdentityUser<Guid>
{
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdate { get; set; }
    public bool IsDeleted { get; set; }
}