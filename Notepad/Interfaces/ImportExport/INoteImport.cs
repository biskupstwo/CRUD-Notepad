using Notepad.Interfaces.Repositories;

namespace Notepad.Interfaces.ImportExport;

public interface INoteImport
{
    Task ImportAsync(INoteRepository noteRepository);
}