namespace TestAndSurvey.Contracts;

public class CreateQuestionRequest
{
    public Guid TemplateSurveyId { get; set; }
    public Guid QuestionTypeId { get; set; }
    public int QuestionOrder { get; set; }
    public string QuestionText { get; set; } = string.Empty;
}