using Notepad.Core.Models;

namespace Notepad.Core.Interfaces.Repositories;

public interface IFolderRepository
{
    public Task AddAsync(Folder folder);
    public Task AddAsync(string title);
    public Task<Folder?> ReadAsync(int id);
    public Task UpdateAsync(int id, string newTitle);
    public Task DeleteAsync(int id);
}