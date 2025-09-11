using System.Collections.ObjectModel;
using Notepad.Data;
using Notepad.Models;
using Notepad.Repositories;
using Notepad.Services;

namespace Notepad.Pages;

public partial class MainPage : ContentPage
{
    private readonly NotepadDatabase _notepadDatabase;
    private readonly NoteRepository _NoteRepository;
    private readonly FolderRepository _folderRepository;
    private readonly DatabaseServices _databaseServices;
    public ObservableCollection<Note> Notes { get; set; } = new();

    public MainPage(NotepadDatabase notepadDatabase, NoteRepository NoteRepository, FolderRepository folderRepository, DatabaseServices databaseServices)
    {
        InitializeComponent();
        _notepadDatabase = notepadDatabase;
        _NoteRepository = NoteRepository;
        _folderRepository = folderRepository;
        _databaseServices = databaseServices;
        BindingContext = this;
        
        LoadNotes();
    }
    
    protected override void OnAppearing()
    {
        Console.WriteLine("Loading notes...");
        SearchBar.Text = "";
        base.OnAppearing();
        LoadNotes();
    }
    
    public async Task LoadNotes(int mode = 0, string input = "")
    {
        Notes.Clear();
        IEnumerable<Note> notes = mode switch
        {
            1 => await _databaseServices.GetNotesCombinedByInput(input),
            _ => await _NoteRepository.GetAllAsync()
        };
        
        foreach (var note in notes)
        {
            Notes.Add(note);
        }
    }
    
    private async void OnAddNoteClicked(object? sender, EventArgs e)
    {
        string response = await DisplayPromptAsync("Create a new note", "Enter a title for the new note:", placeholder: "New note", initialValue: "New note");
        
        if (String.IsNullOrEmpty(response)) response = Constants.Constants.DefaultTitle;
        await _NoteRepository.Add(response, $"", Constants.Constants.DefaultTag, TagColor.Black);
        Console.WriteLine("A new note has been added!");
        
        await LoadNotes();
    }
    
    private async void OnClick(object? sender, EventArgs e)
    {
        if (sender is VerticalStackLayout layout)
        {
            Note? note = layout.BindingContext as Note;
            if (note == null) return;
            Console.WriteLine($"Opening note: {note.Title}");
            await Navigation.PushAsync(new NotePage(note, _notepadDatabase, _NoteRepository));
        }
    }

    private void OnSearchUpdated(object? sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry)
        {
            LoadNotes(1, entry.Text);
        }
    }
}