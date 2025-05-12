namespace TestAndSurvey.Contracts;

public class QuestionOptionDto
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid QuestionId { get; set; }
    public int QuestionOptionOrder { get; set; }
    public string? QuestionOptionText { get; set; }
    public Guid TemplateSurveyId { get; set; }
}
