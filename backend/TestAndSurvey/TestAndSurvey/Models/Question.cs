namespace TestAndSurvey.Contracts;

public class Question
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid QuestionTypeId { get; set; }
    public Guid TemplateSurveyId { get; set; }
    public int QuestionOrder { get; set; }
    public string? QuestionText { get; set; }
}
