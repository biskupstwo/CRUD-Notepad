using Notepad.Core.Interfaces.Repositories;
using Notepad.Core.Interfaces.Services;
using Notepad.Core.Models;

namespace Notepad.Core.Services;

public class DatabaseServices : IDatabaseServices
{
    private readonly INoteRepository _noteRepository;

    public DatabaseServices(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }


    public async Task<IEnumerable<Note>> GetNotesByTitleAsync(string title)
    {
        string search = title.ToLower();
        IEnumerable<Note> notes = await _noteRepository.GetAllAsync();
        return notes.Where(n => n.Title.ToLower().Contains(search));
    }


    public async Task<IEnumerable<Note>> GetNotesByTagAsync(string tag)
    {
        IEnumerable<Note> notes = await _noteRepository.GetAllAsync();
        return notes.Where(n => n.Tag.ToLower() == tag.ToLower());
    }
        

    public async Task<IEnumerable<Note>> GetNotesBySnippetAsync(string snippet)
    {
        string search = snippet.ToLower();
        IEnumerable<Note> notes = await _noteRepository.GetAllAsync();
        return notes.Where(n => n.Content.ToLower().Contains(search));
    }

    public async Task<IEnumerable<Note>> GetNotesCombinedByInputAsync(string input)
    {
        IEnumerable<Note> notesByTitle = await GetNotesByTitleAsync(input);
        IEnumerable<Note> notesByTag = await GetNotesByTagAsync(input);
        IEnumerable<Note> notesBySnippet = await GetNotesBySnippetAsync(input);
        return notesByTitle.Concat(notesByTag).Concat(notesBySnippet).Distinct();
    } 


    public IEnumerable<Note> GetNotesSortedAlphabetically(IEnumerable<Note> notes) => notes.OrderBy(n => n.Title);
    public IEnumerable<Note> GetNotesSortedAlphabeticallyDesc(IEnumerable<Note> notes) => notes.OrderByDescending(n => n.Title);
    public IEnumerable<Note> GetNotesSortedByDateOfCreation(IEnumerable<Note> notes) => notes.OrderBy(n => n.CreatedAt);
    public IEnumerable<Note> GetNotesSortedByDateOfCreationDesc(IEnumerable<Note> notes) => notes.OrderByDescending(n => n.CreatedAt);
}