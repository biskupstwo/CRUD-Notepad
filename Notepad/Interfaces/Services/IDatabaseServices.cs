using Notepad.Models;

namespace Notepad.Interfaces.Services;

public interface IDatabaseServices
{
    public Task<IEnumerable<Note>> GetNotesByTitleAsync(string title);
    public Task<IEnumerable<Note>> GetNotesByTagAsync(string tag);
    public Task<IEnumerable<Note>> GetNotesBySnippetAsync(string snippet);
    public Task<IEnumerable<Note>> GetNotesCombinedByInputAsync(string input);
    public IEnumerable<Note> GetNotesSortedAlphabetically(IEnumerable<Note> notes);
    public IEnumerable<Note> GetNotesSortedAlphabeticallyDesc(IEnumerable<Note> notes);
    public IEnumerable<Note> GetNotesSortedByDateOfCreation(IEnumerable<Note> notes);
    public IEnumerable<Note> GetNotesSortedByDateOfCreationDesc(IEnumerable<Note> notes);
}