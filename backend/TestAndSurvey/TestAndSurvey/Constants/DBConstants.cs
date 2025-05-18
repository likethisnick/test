namespace TestAndSurvey.Constants;

public static class DBConstants
{
    // Question Types
    public static readonly Guid DefaultQuestionTypeId = Guid.Parse("4B7B25C7-6EFA-430D-9113-67ED57CA7BE5");
    
    public const int MaxQuestionTextLength = 4000;
    public const string QuestionForeignKeyConstraint = "FK_Question_QuestionType";
}