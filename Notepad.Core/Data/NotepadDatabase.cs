using Notepad.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Notepad.Core.Data;

public class NotepadDatabase : DbContext
{
    public DbSet<Note> Notes { get; set; }
    public DbSet<Folder> Folders { get; set; }
    public string DatabasePath { get; }
    
    public NotepadDatabase(string databasePath)
    {
        DatabasePath = databasePath;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseSqlite($"Data Source={DatabasePath}");
    
}