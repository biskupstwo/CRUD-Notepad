using Notepad.Models;

namespace Notepad.Interfaces.Services;

public interface IDatabaseServices
{
    public Task<IEnumerable<Note>> GetNotesByTitle(string title);
    public Task<IEnumerable<Note>> GetNotesByTag(string tag);
    public Task<IEnumerable<Note>> GetNotesBySnippet(string snippet);
    public Task<IEnumerable<Note>> GetNotesCombinedByInput(string input);
    public IEnumerable<Note> GetNotesSortedAlphabetically(IEnumerable<Note> notes);
    public IEnumerable<Note> GetNotesSortedAlphabeticallyDesc(IEnumerable<Note> notes);
    public IEnumerable<Note> GetNotesSortedByDateOfCreation(IEnumerable<Note> notes);
    public IEnumerable<Note> GetNotesSortedByDateOfCreationDesc(IEnumerable<Note> notes);
}