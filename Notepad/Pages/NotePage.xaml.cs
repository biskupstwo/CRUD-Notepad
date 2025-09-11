using Notepad.Data;
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
    
    public NotePage(Note note, NotepadDatabase notepadDatabase, NoteRepository noteRepository)
    {
        _note = note;
        _notepadDatabase = notepadDatabase;
        _NoteRepository = noteRepository;
        InitializeComponent();
        NoteTitle.Text = _note.Title;
        NoteContent.Text = _note.Content;
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
            
            await _NoteRepository.Update(id, textBoxText, _note.Content, _note.Tag, _note.Color);
            LastEditedUpdate();
        }
    }
    
    private async void OnContentChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is Editor editor)
        {
            string textBoxText = editor.Text;
            int id = _note.Id;
            
            await _NoteRepository.Update(id, _note.Title, textBoxText, _note.Tag, _note.Color);
            LastEditedUpdate();
        }
    }

    private async void OnDeleteClicked(object? sender, EventArgs e)
    {
        if (sender is Button button)
        {
            int id = _note.Id;
            Console.WriteLine($"Removing note: {_note.Id}");
            await _NoteRepository.Delete(id);
            Console.WriteLine("Note deleted.");
            await Navigation.PopAsync();
        }
    }

    private void OnExportClicked(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}