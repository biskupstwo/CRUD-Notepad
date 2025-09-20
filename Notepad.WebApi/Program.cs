using Notepad.Core.Constants;
using Notepad.Core.Data;
using Notepad.Core.Interfaces.Repositories;
using Notepad.Core.Interfaces.Services;
using Notepad.Core.Models;
using Notepad.Core.Repositories;
using Notepad.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var databasePath = Path.Combine(builder.Environment.ContentRootPath, "NotepadDatabase.db");
builder.Services.AddScoped<NotepadDatabase>(_ => new NotepadDatabase(databasePath));
builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<IFolderRepository, FolderRepository>();
builder.Services.AddScoped<IDatabaseServices, DatabaseServices>();
builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<NotepadDatabase>();
    db.Database.EnsureCreated();

    if (!db.Folders.Any(f => f.Id == Constants.DefaultFolderId))
    {
        db.Folders.Add(new Folder(Constants.DefaultFolderName) { Id = Constants.DefaultFolderId });
        db.SaveChanges();
    }
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();