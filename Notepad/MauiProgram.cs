
using Microsoft.Extensions.Logging;
using Notepad.Data;
using Notepad.Models;
using Notepad.Services;
using Notepad.Repositories;

namespace Notepad;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        var databasePath = Path.Combine(FileSystem.Current.AppDataDirectory, "NotepadDatabase.db");
        
        var database = new NotepadDatabase(databasePath);
        var noteRepository = new NoteRepository(database);
        var folderRepository = new FolderRepository(database);
        var databaseServices = new DatabaseServices(noteRepository);
        database.Database.EnsureCreated(); 
        if (!database.Folders.Any(f => f.Id == 1))
        {
            database.Folders.Add(new Folder("Default") { Id = 1 });
            database.SaveChanges();
        }
        
        //tutaj byl singleton dla db
        builder.Services.AddSingleton(database);
        builder.Services.AddSingleton(noteRepository);
        builder.Services.AddSingleton(folderRepository);
        builder.Services.AddSingleton(databaseServices);
        
        return builder.Build();
    }
}