namespace TestAndSurvey.Contracts;

public class TemplateSurveyDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedOn { get; set; }

    public TemplateSurveyDto(Guid id, string name, string description, DateTime createdOn)
    {
        Id = id;
        Name = name;
        Description = description;
        CreatedOn = createdOn;
    }
}

public class GetTemplateSurveysResponse
{
    public List<TemplateSurveyDto> Surveys { get; set; }

    public GetTemplateSurveysResponse(List<TemplateSurveyDto> surveys)
    {
        Surveys = surveys;
    }
}