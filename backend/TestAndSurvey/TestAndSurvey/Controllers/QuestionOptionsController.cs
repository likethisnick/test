using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAndSurvey.DataAccess;
using TestAndSurvey.Models;

namespace TestAndSurvey.Controllers;

[ApiController]
[Route("[controller]")]
public class QuestionOptionsController(SurvefyDbContext dbContext) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] Guid? templateSurveyId, CancellationToken ct)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();
        
        var query = dbContext.QuestionOption.AsQueryable();

        if (templateSurveyId.HasValue)
            query = query.Where(q => q.TemplateSurveyId == templateSurveyId.Value);

        var options = await query
            .OrderBy(q => q.QuestionId)
            .ThenBy(q => q.QuestionOptionOrder)
            .Select(q => new QuestionOption
            {
                Id = q.Id,
                CreatedOn = q.CreatedOn,
                QuestionId = q.QuestionId,
                QuestionOptionOrder = q.QuestionOptionOrder,
                QuestionOptionText = q.QuestionOptionText,
                TemplateSurveyId = q.TemplateSurveyId
            })
            .ToListAsync(ct);

        return Ok(options);
    }
}
