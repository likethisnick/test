using TestAndSurvey.Contracts;
namespace TestAndSurvey.Models;

public class QuestionOption
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid QuestionId { get; set; }
    public int QuestionOptionOrder { get; set; }
    public string? QuestionOptionText { get; set; }
    public Guid TemplateSurveyId { get; set; }

    public Question Question { get; set; }
    public TemplateSurvey TemplateSurvey { get; set; }
}