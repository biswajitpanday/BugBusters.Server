using System.Security.Claims;
using OptiOverflow.Core.Interfaces.Common;

namespace OptiOverflow.Service;

public class CurrentUserService : ICurrentUserService
{
    // public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    // {
    //     var userId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    //     UserId = userId != null ? new Guid(userId) : Guid.Empty;
    //     UserName = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
    //     Email = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
    //     Role = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);
    // }
    // public Guid UserId { get; }
    // public string? UserName { get; }
    // public string? Email { get; }
    // public string? Role { get; }


    private List<Claim> _claims;
    
    public Guid UserId
    {
        get
        {
            var userId = GetClaim(ClaimTypes.NameIdentifier);
            return new Guid(userId);
        }
    }
    
    public string Role
    {
        get
        {
            var role = GetClaim(ClaimTypes.Role);
            return role.ToLower();
        }
    }
    
    public void SetClaims(IEnumerable<Claim> claims)
    {
        _claims = claims.ToList();
    }
    
    private string GetClaim(string type)
    {
        return _claims.SingleOrDefault(c => c.Type == type)?.Value;
    }
}