using System.Text;
using System.Text.Json;
using CommunityToolkit.Maui.Storage;
using Notepad.Core.Models;

namespace Notepad.ImportExport;

public class NoteExport
{
    public async Task ExportAsync(Note note, int option)
    {
        await (option switch
        {
            1 => ExportToJsonAsync(note),
            _ => ExportToTxtAsync(note)
        });
    }

    private async Task ExportToJsonAsync(Note note)
    {
        var data = new
        {
            Information = ".dno type format",
            Version = 1,
            Note = new
            {
                Title = note.Title,
                Content = note.Content,
                Tag = note.Tag,
                Color = note.Color.ToString()
            }
        };
        
        var json = JsonSerializer.Serialize(data);
        var bytes = Encoding.UTF8.GetBytes(json); 
        await using var memoryStream = new MemoryStream(bytes);

        await FileSaver.Default.SaveAsync($"{note.Title}.dno",memoryStream);
    }

    private async Task ExportToTxtAsync(Note note)
    {
        var fileName = $"{note.Title}.txt";
        var data = note.Content;

        var bytes = Encoding.UTF8.GetBytes(data);
        await using var memoryStream = new MemoryStream(bytes);

        await FileSaver.Default.SaveAsync(fileName, memoryStream);
    }
}