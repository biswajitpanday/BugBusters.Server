﻿namespace BugBusters.Server.Core.Entities;

public class Question : BaseEntity//, IStringSearchable
{
    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;
    public Guid CreatedById { get; set; }
    public Guid LastUpdatedById { get; set; }

    public ICollection<Answer>? Answers { get; set; }
    public ICollection<Vote>? Votes { get; set; }

    public ApplicationUser CreatedBy { get; set; } = null!;
    //public ApplicationUser LastUpdatedBy { get; set; } = null!;


    // public bool Search(string searchText)
    // {
    //     var values = new[]
    //     {
    //         Title.ToString(CultureInfo.InvariantCulture),
    //         Body.ToString(CultureInfo.InvariantCulture),
    //     };
    //     return Utility.SearchStringArray(values, searchText);
    // }
}