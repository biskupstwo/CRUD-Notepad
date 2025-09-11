using Notepad.Models;

namespace Notepad.Interfaces.Repositories;

public interface IFolderRepository
{
    public Task Add(Folder folder);
    public Task Add(string title);
    public Task<Folder?> Read(int id);
    public Task Update(int id, string newTitle);
    public Task Delete(int id);
}