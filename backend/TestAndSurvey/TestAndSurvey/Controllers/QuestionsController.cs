using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAndSurvey.Constants;
using TestAndSurvey.Contracts;
using TestAndSurvey.DataAccess;
using TestAndSurvey.Models;

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
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateQuestion([FromBody] CreateQuestionRequest request, CancellationToken ct)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();
        
        var questionTypeId = request.QuestionTypeId == Guid.Empty
            ? DBConstants.DefaultQuestionTypeId
            : request.QuestionTypeId;
        
        var question = new Question
        {
            Id = Guid.NewGuid(),
            CreatedOn = DateTime.UtcNow,
            TemplateSurveyId = request.TemplateSurveyId,
            QuestionTypeId = questionTypeId,
            QuestionOrder = request.QuestionOrder,
            QuestionText = request.QuestionText
        };

        try
        {
            dbContext.Question.Add(question);
            await dbContext.SaveChangesAsync(ct);

            return CreatedAtAction(nameof(GetBySurveyId), new { templateSurveyId = question.TemplateSurveyId }, new { question.Id });
        }
        catch (DbUpdateException ex)
        {
            var baseMessage = ex.InnerException?.Message ?? ex.Message;
            return StatusCode(500, $"Database error: {baseMessage}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }
    
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQuestion(Guid id, CancellationToken ct)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var question = await dbContext.Question.FindAsync(new object[] { id }, ct);
        if (question == null)
            return NotFound();

        dbContext.Question.Remove(question);

        try
        {
            await dbContext.SaveChangesAsync(ct);
            return NoContent(); // 204
        }
        catch (DbUpdateException ex)
        {
            var baseMessage = ex.InnerException?.Message ?? ex.Message;
            return StatusCode(500, $"Database error: {baseMessage}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }
    
    [Authorize]
    [HttpPost("options")]
    public async Task<IActionResult> CreateQuestionOption([FromBody] CreateQuestionOptionRequest request, CancellationToken ct)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();
        
        if (request.QuestionId == Guid.Empty || string.IsNullOrWhiteSpace(request.QuestionOptionText))
            return BadRequest("Invalid input");

        var option = new QuestionOption
        {
            Id = Guid.NewGuid(),
            QuestionId = request.QuestionId,
            QuestionOptionOrder = request.QuestionOptionOrder,
            QuestionOptionText = request.QuestionOptionText,
            TemplateSurveyId = request.TemplateSurveyId
        };

        dbContext.QuestionOption.Add(option);

        try
        {
            await dbContext.SaveChangesAsync(ct);
            return CreatedAtAction(null, new { id = option.Id });
        }
        catch (DbUpdateException ex)
        {
            var msg = ex.InnerException?.Message ?? ex.Message;
            return StatusCode(500, $"Database error: {msg}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }
    
    [Authorize]
    [HttpDelete("options/{id}")]
    public async Task<IActionResult> DeleteOption(Guid id, CancellationToken ct)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var option = await dbContext.QuestionOption.FindAsync(new object[] { id }, ct);
        if (option == null)
            return NotFound();

        dbContext.QuestionOption.Remove(option);

        try
        {
            await dbContext.SaveChangesAsync(ct);
            return NoContent(); // 204
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }
    
    [Authorize]
    [HttpPut("options")]
    public async Task<IActionResult> UpdateOption([FromBody] UpdateQuestionOptionRequest request, CancellationToken ct)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var option = await dbContext.QuestionOption.FindAsync(new object[] { request.Id }, ct);
        if (option == null)
            return NotFound();

        option.QuestionOptionText = request.QuestionOptionText;
        option.QuestionOptionOrder = request.QuestionOptionOrder;

        try
        {
            await dbContext.SaveChangesAsync(ct);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }
}

