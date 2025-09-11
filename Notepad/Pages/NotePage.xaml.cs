using Notepad.Data;
using Notepad.ImportExport;
using Notepad.Models;
using Notepad.Services;
using Notepad.Repositories;

namespace Notepad.Pages;

public partial class NotePage : ContentPage
{
    private readonly NotepadDatabase _notepadDatabase;
    private readonly NoteRepository _NoteRepository;
    private readonly FolderRepository _folderRepository;
    private Note _note;
    private NoteExport _noteExport;
    
    public NotePage(Note note, NotepadDatabase notepadDatabase, NoteRepository noteRepository)
    {
        _note = note;
        _notepadDatabase = notepadDatabase;
        _NoteRepository = noteRepository;
        InitializeComponent();
        NoteTitle.Text = _note.Title;
        NoteContent.Text = _note.Content;
        _noteExport = new();
        LastEditedUpdate();
    }

    private void LastEditedUpdate()
    {
        DateTime dateTime = _note.EditedAt ?? _note.CreatedAt;
        string lastEditedString = $"Last modified: {dateTime:dd.MM.yyyy HH:mm}";
        LastModifiedLabel.Text = lastEditedString;
    }
    
    private async void OnTitleChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is Editor editor)
        {
            string textBoxText = editor.Text;
            int id = _note.Id;
            
            await _NoteRepository.UpdateAsync(id, textBoxText, _note.Content, _note.Tag, _note.Color);
            LastEditedUpdate();
        }
    }
    
    private async void OnContentChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is Editor editor)
        {
            string textBoxText = editor.Text;
            int id = _note.Id;
            
            await _NoteRepository.UpdateAsync(id, _note.Title, textBoxText, _note.Tag, _note.Color);
            LastEditedUpdate();
        }
    }

    private async void OnDeleteClicked(object? sender, EventArgs e)
    {
        if (sender is Button button)
        {
            int id = _note.Id;
            Console.WriteLine($"Removing note: {_note.Id}");
            await _NoteRepository.DeleteAsync(id);
            Console.WriteLine("Note deleted.");
            await Navigation.PopAsync();
        }
    }

    private async void OnExportClicked(object? sender, EventArgs e)
    {
        if (sender is Button button)
        {
            Console.WriteLine($"Saving note: {_note.Id}");
            await _noteExport.ExportAsync(_note, 1);
            Console.WriteLine("Done.");
        }
    }
}