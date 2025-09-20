using System.ComponentModel.DataAnnotations.Schema;

namespace Notepad.Core.Models;

[Table("Folders")]
public class Folder
{
    public int Id { get; set; }
    public string Title { get; private set; }
    public List<Note> Notes { get; private set; }

    private Folder()
    {
        Notes = new List<Note>();
    }
    
    public Folder(string title)
    {
        Title = !String.IsNullOrEmpty(title) ? title : "PLACEHOLDER TITLE"; 
        Notes = new List<Note>();
    }

    public void ChangeTitle(string newTitle) => Title = !String.IsNullOrEmpty(newTitle) ? newTitle : Title;

    public void AddNote(Note note)
    {
        note.FolderId = Id;
        Notes.Add(note);
    }

    public void AddNote(string title, string content, string tag, TagColor color)
    {
        Note note = new Note(title, content, tag, color);
        note.FolderId = Id;
        Notes.Add(note);
    }

    public void RemoveNote(Note note) => Notes.Remove(note);
    
    public bool RemoveNoteById(int id)
    {
        Note? note = Notes.FirstOrDefault(n => n.Id == id);
        if (note == null) return false;
        Notes.Remove(note);
        return true;
    }
}