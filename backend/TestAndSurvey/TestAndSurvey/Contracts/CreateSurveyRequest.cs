namespace TestAndSurvey.Contracts;

public record CreateSurveyRequest(string Name, string Description, DateTime CreatedOn, string createdbyuserid);