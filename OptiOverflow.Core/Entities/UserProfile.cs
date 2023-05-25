namespace OptiOverflow.Core.Entities;

public class UserProfile : BaseEntity
{
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public Guid AccountId { get; set; }
}