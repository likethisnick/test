namespace TestAndSurvey.Models;

public class Note
{
    public Note(string title, string description)
    {
        Title = title;
        Description = description;
        CreatedAt = DateTime.Now;
    }
    
    public Guid Id { get; init; }

    public string Title { get; init; }
    
    public string Description { get; init; }
    
    public DateTime CreatedAt { get; init; }
    
}