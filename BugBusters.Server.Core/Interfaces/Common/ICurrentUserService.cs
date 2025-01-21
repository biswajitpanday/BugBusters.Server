using System.Security.Claims;

namespace BugBusters.Server.Core.Interfaces.Common;

public interface ICurrentUserService
{
    // Guid UserId { get; }
    // string? UserName { get; }
    // string? Email { get; }
    // string? Role { get; }

    Guid UserId { get; }
    string Role { get; }
    void SetClaims(IEnumerable<Claim> claims);
}