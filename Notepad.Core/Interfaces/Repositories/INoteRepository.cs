using Notepad.Core.Models;

namespace Notepad.Core.Interfaces.Repositories;

public interface INoteRepository
{
    public Task<IEnumerable<Note>> GetAllAsync();
    public Task<Note> AddAsync(Note note);
    public Task<Note> AddAsync(string title, string content, string tag, TagColor color);
    public Task<Note?> ReadAsync(int id);
    public Task UpdateAsync(int id, string newTitle, string newContent, string newTag,
        TagColor newColor);
    public Task UpdateAsync(int id, Note note);
    public Task DeleteAsync(int id);
}