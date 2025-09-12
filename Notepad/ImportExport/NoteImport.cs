using CommunityToolkit.Maui.Storage;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Notepad.Interfaces.ImportExport;
using Notepad.Interfaces.Repositories;
using Notepad.Models;

namespace Notepad.ImportExport;

public class NoteImport : INoteImport
{
    private FilePickerFileType _customFileTypes = new FilePickerFileType(
        new Dictionary<DevicePlatform, IEnumerable<string>>
        {
            { DevicePlatform.WinUI, new[] { ".dno", ".dzoniec", ".txt" } },
            { DevicePlatform.MacCatalyst, new[] { "public.data", ".dno", ".dzoniec", ".txt" } }
        });


    public async Task ImportAsync(INoteRepository noteRepository)
    {
        var result = await PickFileAsync();
        if (result == null) return;
        string extension = Path.GetExtension(result.FileName);

        Note? note = extension switch
        {
            ".dno" or ".dzoniec" => await ImportJsonHandlerAsync(result),
            ".txt" => await ImportTxtHandlerAsync(result),
            _ => null
        };
        if (note == null) return;

        Console.WriteLine($"Importing file:  {Path.GetFileName(result.FileName)}");
        await noteRepository.AddAsync(note);
    }


    private async Task<FileResult?> PickFileAsync() => await FilePicker.Default.PickAsync(new PickOptions
    {
        PickerTitle = "Select a note file",
        FileTypes = _customFileTypes
    });


    private (bool isValid, JObject note) VerifyAndParse(Stream stream)
    {
        using var reader = new StreamReader(stream);
        string fileInString = reader.ReadToEnd();
        var schema = JSchema.Parse(@"{
        '$schema': 'https://json-schema.org/draft/2020-12/schema',
        'type': 'object',
        'required': ['Information', 'Version', 'Note'],
        'properties': {
            'Information': {
                'type': 'string'
            },
            'Version': {
                'type': 'integer',
                'minimum': 1
            },
            'Note': {
                'type': 'object',
                'required': ['Title', 'Content', 'Tag', 'Color'],
                'properties': {
                    'Title': {
                        'type': 'string'
                    },
                    'Content': {
                        'type': 'string'
                    },
                    'Tag': {
                        'type': 'string'
                    },
                    'Color': {
                        'type': 'string',
                        'enum': ['Red', 'Green', 'Blue', 'Yellow', 'Black', 'None']
                    }
                },
                'additionalProperties': false
            }
        },
        'additionalProperties': false
        }");
        var note = JObject.Parse(fileInString);
        return (note.IsValid(schema), note);
    }

    private async Task<Note?> ImportJsonHandlerAsync(FileResult fileResult)
    {
        using var stream = await fileResult.OpenReadAsync();
        (bool verifiedFile, JObject noteJObject) = VerifyAndParse(stream);

        if (!verifiedFile) return null;
        return CreateNoteFromJObject(noteJObject);
    }

    private async Task<Note> ImportTxtHandlerAsync(FileResult fileResult)
    {
        using var stream = await fileResult.OpenReadAsync();
        using var reader = new StreamReader(stream);

        string title = Path.GetFileNameWithoutExtension(fileResult.FileName);
        string content = reader.ReadToEnd();

        return new Note(title, content, "", TagColor.None);
    }


    private Note CreateNoteFromJObject(JObject jObject)
    {
        string? title = (string)jObject["Note"]?["Title"]! ?? "";
        string? content = (string)jObject["Note"]?["Content"]! ?? "";
        string? tag = (string)jObject["Note"]?["Tag"]! ?? "";
        string? colorString = (string)jObject["Note"]?["Color"]! ?? "";
        TagColor color = Enum.TryParse<TagColor>(colorString, out TagColor tagColor)
            ? tagColor
            : TagColor.None;

        return new Note(title, content, tag, color);
    }
}