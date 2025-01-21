using BugBusters.Server.Core.Entities;
using BugBusters.Server.Core.Interfaces.Common;

namespace BugBusters.Server.Core.Dtos;

public class ProfileUpdateDto : IMapFrom<ApplicationUser>
{
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Address { get; set; }
}