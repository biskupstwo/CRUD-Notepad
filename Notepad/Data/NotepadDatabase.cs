using Notepad.Models;
using Microsoft.EntityFrameworkCore;

namespace Notepad.Data;

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