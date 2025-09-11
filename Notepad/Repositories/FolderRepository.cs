using Microsoft.EntityFrameworkCore;
using Notepad.Data;
using Notepad.Interfaces.Repositories;
using Notepad.Models;

namespace Notepad.Repositories;

public class FolderRepository : IFolderRepository
{
    private readonly NotepadDatabase _notepadDatabase;

    public FolderRepository(NotepadDatabase notepadDatabase)
    {
        _notepadDatabase = notepadDatabase;
    }

    public async Task Add(Folder folder)
    {
        _notepadDatabase.Add(folder);
        await _notepadDatabase.SaveChangesAsync();
    }

    public async Task Add(string title)
    {
        Folder folder = new Folder(title);
        _notepadDatabase.Add(folder);
        await _notepadDatabase.SaveChangesAsync();
    }

    public async Task<Folder?> Read(int id) => await _notepadDatabase.Folders.Include(f => f.Notes).FirstOrDefaultAsync(f => f.Id == id);

    public async Task Update(int id, string newTitle)
    {
        Folder? folder = await Read(id);
        if (folder == null) return;
        folder.ChangeTitle(newTitle);
        await _notepadDatabase.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        if (id == 1) return;
        Folder? folder = await Read(id);
        if (folder == null) return;

        foreach (var note in folder.Notes)
        {
            note.FolderId = 1;
        }

        _notepadDatabase.Remove(folder);
        await _notepadDatabase.SaveChangesAsync();
    }
}