using Notepad.Models;

namespace Notepad.Interfaces.Repositories;

public interface INoteRepository
{
    public Task<IEnumerable<Note>> GetAllAsync();
    public Task Add(Note note);
    public Task Add(string title, string content, string tag, TagColor color);
    public Task<Note?> Read(int id);
    public Task Update(int id, string newTitle, string newContent, string newTag,
        TagColor newColor);
    public Task Delete(int id);
}