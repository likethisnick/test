using System.Linq.Expressions;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAndSurvey.Contracts;
using TestAndSurvey.DataAccess;
using TestAndSurvey.Models;

namespace TestAndSurvey.Controllers;

[ApiController]
[Route("[controller]")]
public class NewSurveyController(UserManager<SurvefyUser> userManager, IConfiguration config, SurvefyDbContext dbContext) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateTemplateSurvey([FromBody] CreateTemplateSurveyRequest request, CancellationToken ct)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var newSurvey = new TemplateSurvey
        {
            Id = Guid.NewGuid(),
            CreatedByUserId = userId,
            CreatedOn = DateTime.UtcNow,
            Name = request.Name,
            Description = request.Description
        };

        dbContext.TemplateSurvey.Add(newSurvey);
        await dbContext.SaveChangesAsync(ct);

        return Ok(new { newSurvey.Id });
    }
}