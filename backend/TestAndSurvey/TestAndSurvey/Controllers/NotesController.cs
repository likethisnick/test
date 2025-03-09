using System.Linq.Expressions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAndSurvey.Contracts;
using TestAndSurvey.DataAccess;
using TestAndSurvey.Models;

namespace TestAndSurvey.Controllers;

[ApiController]
[Route("[controller]")]
public class NotesController(SurvefyDbContext dbContext) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateNoteRequest request, CancellationToken ct)
    {
        var note = new Note(request.Title, request.Description);
        
        await dbContext.Notes.AddAsync(note, ct);
        await dbContext.SaveChangesAsync(ct);
        // add validation and check all added successfully
        return Ok();
    }
    
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetNotesRequest request, CancellationToken ct)
    {
        var notesQuery = dbContext.Notes
            .Where(n => string.IsNullOrWhiteSpace(request.Search) ||
                        n.Title.ToLower().Contains(request.Search.ToLower()));

        Expression<Func<Note,object>> selectorKey = request.SortItem?.ToLower() switch
        {
            "date" => note => note.CreatedAt,
            "title" => note => note.Title,
            _ => note => note.Id
        };
        
        notesQuery = request?.SortOrder == "desc" 
            ? notesQuery.OrderByDescending(selectorKey) 
            : notesQuery.OrderBy(selectorKey);

        var noteDtos = await notesQuery
            .Select(n => new NoteDto(n.Id, n.Title, n.Description,n.CreatedAt))
            .ToListAsync(ct);
        
        return Ok(new GetNotesResponse(noteDtos));
    }
}