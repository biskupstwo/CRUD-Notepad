using Microsoft.EntityFrameworkCore;
using Notepad.Core.Data;
using Notepad.Core.Interfaces.Repositories;
using Notepad.Core.Models;

namespace Notepad.Core.Repositories;

public class FolderRepository : IFolderRepository
{
    private readonly NotepadDatabase _notepadDatabase;

    public FolderRepository(NotepadDatabase notepadDatabase)
    {
        _notepadDatabase = notepadDatabase;
    }

    public async Task AddAsync(Folder folder)
    {
        _notepadDatabase.Add(folder);
        await _notepadDatabase.SaveChangesAsync();
    }

    public async Task AddAsync(string title)
    {
        Folder folder = new Folder(title);
        _notepadDatabase.Add(folder);
        await _notepadDatabase.SaveChangesAsync();
    }

    public async Task<Folder?> ReadAsync(int id) => await _notepadDatabase.Folders.Include(f => f.Notes).FirstOrDefaultAsync(f => f.Id == id);

    public async Task UpdateAsync(int id, string newTitle)
    {
        Folder? folder = await ReadAsync(id);
        if (folder == null) return;
        folder.ChangeTitle(newTitle);
        await _notepadDatabase.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        if (id == Constants.Constants.DefaultFolderId) return;
        Folder? folder = await ReadAsync(id);
        if (folder == null) return;

        foreach (var note in folder.Notes)
        {
            note.FolderId = Constants.Constants.DefaultFolderId;
        }

        _notepadDatabase.Remove(folder);
        await _notepadDatabase.SaveChangesAsync();
    }
}