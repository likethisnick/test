namespace TestAndSurvey.Contracts;

public class CreateQuestionOptionRequest
{
    public Guid QuestionId { get; set; }
    public int QuestionOptionOrder { get; set; }
    public string QuestionOptionText { get; set; } = string.Empty;
    public Guid TemplateSurveyId { get; set; }
}