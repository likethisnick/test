using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using TestAndSurvey.DataAccess;
using TestAndSurvey.Models;
using TestAndSurvey.Contracts;

namespace MXDevelop.Controllers;

[ApiController]
[Route("[controller]")]
public class TemplateSurveysController(SurvefyDbContext dbContext) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetSurveys([FromQuery] GetTemplateSurveysRequest request, CancellationToken ct)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();
        
        if (string.IsNullOrWhiteSpace(request.CreatedByUserId))
            return BadRequest("CreatedByUserId is required");

        var surveys = await dbContext.TemplateSurvey
            .Where(s => s.CreatedByUserId == request.CreatedByUserId)
            .OrderByDescending(s => s.CreatedOn)
            .Select(s => new TemplateSurveyDto(
                s.Id,
                s.Name,
                s.Description,
                s.CreatedOn
            ))
            .ToListAsync(ct);

        return Ok(new GetTemplateSurveysResponse(surveys));
    }
    
    [Authorize]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllSurveys(CancellationToken ct)
    {
        var surveys = await dbContext.TemplateSurvey
            .OrderByDescending(s => s.CreatedOn)
            .Select(s => new TemplateSurveyDto(
                s.Id,
                s.Name,
                s.Description,
                s.CreatedOn
            ))
            .ToListAsync(ct);

        return Ok(new GetTemplateSurveysResponse(surveys));
    }
}