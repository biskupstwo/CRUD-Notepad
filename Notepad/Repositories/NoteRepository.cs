using Microsoft.EntityFrameworkCore;
using Notepad.Data;
using Notepad.Interfaces.Repositories;
using Notepad.Models;

namespace Notepad.Repositories;

public class NoteRepository : INoteRepository
{
    private readonly NotepadDatabase _notepadDatabase;

    public NoteRepository(NotepadDatabase notepadDatabase)
    {
        _notepadDatabase = notepadDatabase;
    }
    
    public async Task<IEnumerable<Note>> GetAllAsync()
    {
        return await _notepadDatabase.Notes.ToListAsync();
    }
    
    public async Task AddAsync(Note note)
    {
        _notepadDatabase.Add(note);
        await _notepadDatabase.SaveChangesAsync();
    }

    public async Task AddAsync(string title, string content, string tag, TagColor color)
    {
        Note note = new Note(title, content, tag, color);
        _notepadDatabase.Add(note);
        await _notepadDatabase.SaveChangesAsync();
    }

    public async Task<Note?> ReadAsync(int id) => await _notepadDatabase.Notes.FirstOrDefaultAsync(n => n.Id == id);

    public async Task UpdateAsync(int id, string newTitle, string newContent, string newTag,
        TagColor newColor)
    {
        Note? note = await ReadAsync(id);
        if (note == null) return;
        note.Edit(newTitle, newContent, newTag, newColor);
        _notepadDatabase.Update(note);
        await _notepadDatabase.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        Note? note = await ReadAsync(id);
        if (note == null) return;
        _notepadDatabase.Remove(note);
        await _notepadDatabase.SaveChangesAsync();
    }
}