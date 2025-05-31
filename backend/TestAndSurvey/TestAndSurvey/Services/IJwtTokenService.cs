using TestAndSurvey.DataAccess;

namespace TestAndSurvey.Services
{
    public interface IJwtTokenService
    {
        string GenerateJwtToken(SurvefyUser user);
    }
}