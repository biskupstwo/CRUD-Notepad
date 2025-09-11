using Notepad.Models;

namespace Notepad.Interfaces.Repositories;

public interface INoteRepository
{
    public Task<IEnumerable<Note>> GetAllAsync();
    public Task AddAsync(Note note);
    public Task AddAsync(string title, string content, string tag, TagColor color);
    public Task<Note?> ReadAsync(int id);
    public Task UpdateAsync(int id, string newTitle, string newContent, string newTag,
        TagColor newColor);
    public Task DeleteAsync(int id);
}