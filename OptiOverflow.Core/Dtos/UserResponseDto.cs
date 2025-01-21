using BugBusters.Server.Core.Entities;
using BugBusters.Server.Core.Interfaces.Common;

namespace BugBusters.Server.Core.Dtos;

public class UserResponseDto : ProfileResponseDto, IMapFrom<ApplicationUser>
{
    public string? UserName { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool LockoutEnabled { get; set; }
    public string? Address { get; set; }
    public bool IsDeleted { get; set; }
    public List<QuestionResponseDto>? Questions { get; set; } = null;
}