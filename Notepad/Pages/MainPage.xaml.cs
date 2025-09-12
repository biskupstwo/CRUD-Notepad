using System.Collections.ObjectModel;
using Notepad.Data;
using Notepad.ImportExport;
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
    private NoteImport _noteImport;

    public MainPage(NotepadDatabase notepadDatabase, NoteRepository NoteRepository, FolderRepository folderRepository, DatabaseServices databaseServices)
    {
        InitializeComponent();
        _notepadDatabase = notepadDatabase;
        _NoteRepository = NoteRepository;
        _folderRepository = folderRepository;
        _databaseServices = databaseServices;
        _noteImport = new NoteImport();
        BindingContext = this;
        
        LoadNotesAsync();
    }
    
    protected override void OnAppearing()
    {
        Console.WriteLine("Loading notes...");
        SearchBar.Text = "";
        base.OnAppearing();
        LoadNotesAsync();
    }
    
    public async Task LoadNotesAsync(int mode = 0, string input = "")
    {
        Notes.Clear();
        IEnumerable<Note> notes = mode switch
        {
            1 => await _databaseServices.GetNotesCombinedByInputAsync(input),
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
        
        if (String.IsNullOrEmpty(response)) return;
        await _NoteRepository.AddAsync(response, $"", Constants.Constants.DefaultTag, TagColor.Black);
        Console.WriteLine("A new note has been added!");
        
        await LoadNotesAsync();
    }
    
    private async void OnNoteClicked(object? sender, EventArgs e)
    {
        if (sender is VerticalStackLayout layout)
        {
            Note? note = layout.BindingContext as Note;
            if (note == null) return;
            Console.WriteLine($"Opening note: {note.Title}");
            await Navigation.PushAsync(new NotePage(note, _notepadDatabase, _NoteRepository));
        }
    }

    private async void OnSearchUpdated(object? sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry)
        {
            await LoadNotesAsync(1, entry.Text);
        }
    }

    private async void OnImportClicked(object? sender, EventArgs e)
    {
        if (sender is Button button)
        {
            await _noteImport.ImportAsync(_NoteRepository);
            LoadNotesAsync();
        }
    }
}