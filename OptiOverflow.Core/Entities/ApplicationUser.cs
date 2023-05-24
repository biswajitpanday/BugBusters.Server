using Microsoft.AspNetCore.Identity;

namespace OptiOverflow.Core.Entities;

public class ApplicationUser: IdentityUser<Guid>
{
    public virtual UserProfile Profile { get; set; } = null!;
}