using OptiOverflow.Core.Entities;
using OptiOverflow.Core.Interfaces.Common;

namespace OptiOverflow.Core.Dtos;

public class UserResponseDto : ProfileResponseDto, IMapFrom<ApplicationUser>
{
    public string? UserName { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool LockoutEnabled { get; set; }

    public Guid AccountId { get; set; }
    public string? Address { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdate { get; set; }
    public bool IsDeleted { get; set; }

}