namespace BugBusters.Server.Core.Dtos;

public class JiraTicket
{
    public string? Expand { get; set; }
    public int StartAt { get; set; }
    public int MaxResults { get; set; }
    public int Total { get; set; }
    public required List<Issue> Issues { get; set; }

}

public class Issue
{
    public int Id { get; set; }
    public string? Expand { get; set; }
    public string? Self { get; set; }
    public string? Key { get; set; }
    public Field? Fields { get; set; }
}

public class Field
{
    public DateTime StatusCategoryChangeDate { get; set; }
    public string? Parent { get; set; }     // Need to Fix the Type
    public List<FixVersion>? FixVersions { get; set; }
    public Resolution Resolution { get; set; }
    public string? LastViewed { get; set; } // Need to Fix the Type
    public Priority? Priority { get; set; }
    //public List<Label> Labels { get; set; }
    public double? TimeEstimate { get; set; }
    public double? AggregateTimeOriginalEstimate { get; set; }
    public List<Version>? Versions { get; set; }
    public List<IssueLink>? IssueLinks { get; set; }
    public Profile? Assignee { get; set; }
    public string? AccountType { get; set; }
    public Status Status { get; set; }
    public List<Component> Components { get; set; }
    public double? AggregateTimeEstimate { get; set; }
    public Profile? Creator { get; set; }
    public List<Subtask>? Subtasks { get; set; }
    public Profile? Reporter { get; set; }
    public AggregateProgress? AggregateProgress { get; set; }
    public Votes? Votes { get; set; }
    public IssueType? IssueType { get; set; }
    public string? TimeSpent { get; set; }     // Need to Fix the Type
    public Project? Project { get; set; }
    //public DateTime? ResolutionDate { get; set; }
    public long WorkRatio { get; set; }
    public Watches? Watches { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public long? TimeOriginalEstimate { get; set; } = null;
    public Description? Description { get; set; }
    public string? Security { get; set; }   // Need to Fix the Type
    public string? Summary { get; set; }
    public Description? Environment { get; set; }    // Need to Fix the Type
}

public class FixVersion
{
    // Need to Add
}

public class Priority
{
    public string? Self { get; set; }
    public string? IconUrl { get; set; }
    public string? Name { get; set; }
    public string? Id { get; set; }
}

public class Label
{
    // Need to Add
}
public class Version
{
    // Need to Add
}
public class IssueLink
{
    // Need to Add
}

public class Profile
{
    public string? Self { get; set; }
    public string? AccountId { get; set; }
    public string? EmailAddress { get; set; }
    public Dictionary<string, string>? AvatarUrls { get; set; }
    public string? DisplayName { get; set; }
    public bool Active { get; set; }
    public string? TimeZone { get; set; }
}

public class Status
{
    public string? Self { get; set; }
    public string? Description { get; set; }
    public string? IconUrl { get; set; }
    public string? Name { get; set; }
    public string? Id { get; set; }
    public StatusCategory? StatusCategory { get; set; }
}

public class StatusCategory
{
    public string? Self { get; set; }
    public string? Id { get; set; }
    public string? Key { get; set; }
    public string? ColorName { get; set; }
    public string? Name { get; set; }
}

public class Component
{
    public string? Self { get; set; }
    public string? Id { get; set; }
    public string? Name { get; set; }
}
public class Subtask
{
    // Need to Add
}

public class AggregateProgress
{
    public long Progress { get; set; }
    public long Total { get; set; }
}

public class Votes
{
    public string? Self { get; set; }
    public int Vote { get; set; }   // public int Votes { get; set; }   
    public bool HasVoted { get; set; }
}

public class IssueType
{
    public string? Self { get; set; }
    public string? Id { get; set; }
    public string? Description { get; set; }
    public string? IconUrl { get; set; }
    public string? Name { get; set; }
    public bool Subtask { get; set; }
    public int AvatarId { get; set; }
    public int HierarchyLevel { get; set; }
}

public class Project
{
    public string? Self { get; set; }
    public string? Id { get; set; }
    public string? Key { get; set; }
    public string? Name { get; set; }
    public string? ProjectTypeKey { get; set; }
    public bool Simplified { get; set; }
    public Dictionary<string, string>? AvatarUrls { get; set; }
    public ProjectCategory ProjectCategory { get; set; }
}

public class ProjectCategory
{
    public string? Self { get; set; }
    public string? Id { get; set; }
    public string? Description { get; set; }
    public string? Name { get; set; }
}

public class Watches
{
    public string? Self { get; set; }
    public long WatchCount { get; set; }
    public bool IsWatching { get; set; }
}

public class Description
{
    public int Version { get; set; }
    public string? Type { get; set; }
    public List<Content>? Content { get; set; }
    public string? Name { get; set; }
}

public class Content
{
    public string? Type { get; set; }

    //public List<Contents> Content { get; set; }
}

public class Contents
{
    public string? Type { get; set; }
    public string? Text { get; set; }
}

public class Resolution
{
    public string? Self { get; set; }
    public string Id { get; set; }
    public string? Description { get; set; }
    public string? Name { get; set; }
}
