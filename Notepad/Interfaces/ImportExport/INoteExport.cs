using Notepad.Models;

namespace Notepad.Interfaces.ImportExport;

public interface INoteExport
{
    public Task ExportAsync(Note note, int option);
}