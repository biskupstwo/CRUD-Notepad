using Microsoft.AspNetCore.Mvc;
using Notepad.Core.Interfaces.Repositories;
using Notepad.Core.Interfaces.Services;
using Notepad.Core.Models;

namespace Notepad.WebApi.Controllers;

[ApiController]

[Route("api/[controller]")]
public class NotesController : ControllerBase
{
    private readonly INoteRepository _noteRepository;
    private readonly IDatabaseServices _databaseServices;

    public NotesController(INoteRepository noteRepository, IDatabaseServices databaseServices)
    {
        _noteRepository = noteRepository;
        _databaseServices = databaseServices;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Note>>> GetAllNotesAsync()
    {
        var notes = await _noteRepository.GetAllAsync();
        return Ok(notes);
    }

    [HttpGet("{id:int}", Name = "GetNoteById")]
    public async Task<ActionResult<Note>> GetNoteByIdAsync(int id)
    {
        var note = await _noteRepository.ReadAsync(id);
        if (note == null)
            return NotFound();
        return note;
    }

    
    [HttpGet("search/{input}")]
    public async Task<ActionResult<Note>> GetNoteByInputAsync(string input)
    {
        var notes = await _databaseServices.GetNotesCombinedByInputAsync(input);
        return Ok(notes);
    }
    
    
    [HttpPost]
    public async Task<ActionResult> AddNoteAsync(Note note)
    {
        var addedNote = await _noteRepository.AddAsync(note);
        return CreatedAtAction("GetNoteById", new { id = addedNote.Id }, addedNote);
    }

    [HttpPut("{id:int}")]
    public async Task UpdateNoteAsync(int id, Note note)
    {
        await _noteRepository.UpdateAsync(id, note);
    }

    [HttpDelete("{id:int}")]
    public async Task DeleteNoteAsync(int id)
    {
        await _noteRepository.DeleteAsync(id);
    }
}