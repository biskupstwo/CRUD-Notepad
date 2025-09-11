using Notepad.Interfaces.ImportExport;
using Notepad.Interfaces.Repositories;

namespace Notepad.ImportExport;

public class NoteImport : INoteImport
{
    public Task ImportAsync(INoteRepository noteRepository, Stream file)
    {
        throw new NotImplementedException();
    }
}