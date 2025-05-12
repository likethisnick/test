using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAndSurvey.Contracts;
using TestAndSurvey.DataAccess;

namespace TestAndSurvey.Controllers;

[ApiController]
[Route("[controller]")]
public class QuestionsController(SurvefyDbContext dbContext) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetBySurveyId([FromQuery] Guid templateSurveyId, CancellationToken ct)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();
        
        var questions = await dbContext.Question
            .Where(q => q.TemplateSurveyId == templateSurveyId)
            .OrderBy(q => q.QuestionOrder)
            .Select(q => new Question
            {
                Id = q.Id,
                CreatedOn = q.CreatedOn,
                QuestionTypeId = q.QuestionTypeId,
                TemplateSurveyId = q.TemplateSurveyId,
                QuestionOrder = q.QuestionOrder,
                QuestionText = q.QuestionText
            })
            .ToListAsync(ct);

        return Ok(questions);
    }
}