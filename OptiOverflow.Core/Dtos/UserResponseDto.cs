using OptiOverflow.Core.Entities;
using OptiOverflow.Core.Interfaces.Common;

namespace OptiOverflow.Core.Dtos;

public class UserResponseDto : ProfileResponseDto, IMapFrom<ApplicationUser>
{
    public string? UserName { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool LockoutEnabled { get; set; }
    public string? Address { get; set; }
    public bool IsDeleted { get; set; }
    public List<QuestionResponseDto>? Questions { get; set; } = null;
}