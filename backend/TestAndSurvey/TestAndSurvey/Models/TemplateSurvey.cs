namespace TestAndSurvey.Models;

public class TemplateSurvey
{
    public TemplateSurvey() { }
    public TemplateSurvey(string name, string description, DateTime createdOn, string createdbyuserid)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        CreatedOn = createdOn;
        CreatedByUserId = createdbyuserid;
    }
    
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public DateTime CreatedOn { get; init; }
    public string CreatedByUserId { get; init; }
    
}