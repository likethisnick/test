namespace TestAndSurvey.Contracts;

public class UpdateQuestionOptionRequest
{
    public Guid Id { get; set; }
    public string QuestionOptionText { get; set; } = string.Empty;
    public int QuestionOptionOrder { get; set; }
}