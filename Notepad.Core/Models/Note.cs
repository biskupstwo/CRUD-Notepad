using System.ComponentModel.DataAnnotations.Schema;

namespace Notepad.Core.Models;

[Table("Notes")]
public class Note
{
    public int Id { get; set; }
    public string Title { get; private set; }
    public string Content { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? EditedAt { get; private set; }
    public string Tag { get; private set; }
    public TagColor Color { get; private set; }
    public int FolderId { get; set; }
    

    private Note() {}
    
    public Note(string title, string content, string tag, TagColor color)
    {
        Title = !String.IsNullOrEmpty(title) ? title : "TITLE PLACEHOLDER";
        Content = content != null ? content : String.Empty;
        Tag = tag != null ? tag : String.Empty;
        Color = Enum.IsDefined(typeof(TagColor), color) ? color : TagColor.None;
        CreatedAt = DateTime.Now;
        FolderId = 1;
    }

    public void Edit(string newTitle, string newContent, string newTag, TagColor newColor)
    {
        Title = !String.IsNullOrEmpty(newTitle) ? newTitle : Title;
        Content = newContent ?? Content;
        Tag = newTag ?? Tag;
        Color = Enum.IsDefined(typeof(TagColor), newColor) ? newColor : TagColor.None;
        EditedAt = DateTime.Now;
    }
}