using Microsoft.EntityFrameworkCore;
using Notepad.Core.Data;
using Notepad.Core.Interfaces.Repositories;
using Notepad.Core.Models;

namespace Notepad.Core.Repositories;

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
    
    public async Task<Note?> AddAsync(Note? note)
    {
        if (note == null) return null;
        _notepadDatabase.Add(note);
        await _notepadDatabase.SaveChangesAsync();
        return note;
    }

    public async Task<Note> AddAsync(string title, string content, string tag, TagColor color)
    {
        Note note = new Note(title, content, tag, color);
        _notepadDatabase.Add(note);
        await _notepadDatabase.SaveChangesAsync();
        return note;
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

    public async Task UpdateAsync(int id, Note? newNote)
    {
        if (newNote == null) return;
        Note? note = await ReadAsync(id);
        if (note == null) return;
        note.Edit(newNote.Title, newNote.Content, newNote.Tag, newNote.Color);
        _notepadDatabase.Update(note);
        await _notepadDatabase.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Note? note = await ReadAsync(id);
        if (note == null) return false;
        _notepadDatabase.Remove(note);
        await _notepadDatabase.SaveChangesAsync();
        return true;
    }
}