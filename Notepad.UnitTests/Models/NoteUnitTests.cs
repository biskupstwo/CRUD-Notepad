using Notepad.Core.Models;

namespace Notepad.UnitTests;

[TestClass]
public class NoteUnitTests
{
    private Note _note;

    [TestInitialize]
    public void Setup()
    {
        string title = "Title";
        string content = "Content";
        string tag = "Tag";
        TagColor color = TagColor.None;
        _note = new Note(title, content, tag, color);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _note = null;
    }

    #region Unit tests for Constructor

    /// <summary>
    /// Positive unit test for Constructor in Note Class
    /// </summary>
    /// <param name="color">Tag color value used in the test</param>
    [TestMethod]
    [DataRow(TagColor.None)]
    [DataRow(TagColor.Red)]
    [DataRow(TagColor.Green)]
    [DataRow(TagColor.Blue)]
    [DataRow(TagColor.Yellow)]
    [DataRow(TagColor.Black)]
    public void NoteConstructorPositiveUnitTest(TagColor color)
    {
        string title = "Title";
        string content = "Content";
        string tag = "Tag";
        DateTime defaultDateTime = default(DateTime);
        int defaultFolderId = 1;
        var note = new Note(title, content, tag, color);

        Assert.AreEqual(title, note.Title);
        Assert.AreEqual(content, note.Content);
        Assert.AreEqual(tag, note.Tag);
        Assert.AreEqual(color, note.Color);
        Assert.AreNotEqual(defaultDateTime, note.CreatedAt);
        Assert.IsNull(note.EditedAt);
        Assert.AreEqual(defaultFolderId, note.FolderId);
    }

    /// <summary>
    /// Negative unit test for Constructor in Note Class,
    /// which checks if null title is properly handled.
    /// </summary>
    [TestMethod]
    public void NoteConstructorNegativeUnitTestNullTitle()
    {
        string expectedTitle = "TITLE PLACEHOLDER";
        string title = null;
        string content = "Content";
        string tag = "Tag";
        TagColor color = TagColor.None;

        var note = new Note(title, content, tag, color);

        Assert.AreEqual(expectedTitle, note.Title);
    }

    /// <summary>
    /// Negative unit test for Constructor in Note Class,
    /// which checks if empty title string is properly handled.
    /// </summary>
    [TestMethod]
    public void NoteConstructorNegativeUnitTestEmptyTitleString()
    {
        string expectedTitle = "TITLE PLACEHOLDER";
        string title = String.Empty;
        string content = "Content";
        string tag = "Tag";
        TagColor color = TagColor.None;

        var note = new Note(title, content, tag, color);

        Assert.AreEqual(expectedTitle, note.Title);
    }

    /// <summary>
    /// Negative unit test for Constructor in Note Class,
    /// which checks if null content is properly handled.
    /// </summary>
    [TestMethod]
    public void NoteConstructorNegativeUnitTestNullContent()
    {
        string expectedContent = String.Empty;
        string title = "Title";
        string content = null;
        string tag = "Tag";
        TagColor color = TagColor.None;

        var note = new Note(title, content, tag, color);

        Assert.AreEqual(expectedContent, note.Content);
    }

    /// <summary>
    /// Negative unit test for Constructor in Note Class,
    /// which checks if null tag is properly handled.
    /// </summary>
    [TestMethod]
    public void NoteConstructorNegativeUnitTestNullTag()
    {
        string expectedTag = String.Empty;
        string title = "Title";
        string content = "Content";
        string tag = null;
        TagColor color = TagColor.None;

        var note = new Note(title, content, tag, color);

        Assert.AreEqual(expectedTag, note.Tag);
    }

    /// <summary>
    /// Negative unit test for Constructor in Note Class,
    /// which checks if invalid TagColor values are properly handled.
    /// </summary>
    /// <param name="color">Tag color value used in the test</param>
    [TestMethod]
    [DataRow(Int32.MinValue)]
    [DataRow(-1)]
    [DataRow(6)]
    [DataRow(Int32.MaxValue)]
    public void NoteConstructorNegativeUnitTestInvalidTagColor(TagColor color)
    {
        string title = "Title";
        string content = "Content";
        string tag = "Tag";
        TagColor expectedColor = TagColor.None;

        var note = new Note(title, content, tag, color);

        Assert.AreEqual(expectedColor, note.Color);
    }

    #endregion

    #region Unit tests for Edit method

    /// <summary>
    /// Positive unit test for Edit method in Note Class
    /// </summary>
    /// <param name="newColor">New tag color value used in the test</param>
    [TestMethod]
    [DataRow(TagColor.None)]
    [DataRow(TagColor.Red)]
    [DataRow(TagColor.Green)]
    [DataRow(TagColor.Blue)]
    [DataRow(TagColor.Yellow)]
    [DataRow(TagColor.Black)]
    public void NoteEditPositiveUnitTest(TagColor newColor)
    {
        string newTitle = "New Title";
        string newContent = "New Content";
        string newTag = "New Tag";

        _note.Edit(newTitle, newContent, newTag, newColor);

        Assert.AreEqual(newTitle, _note.Title);
        Assert.AreEqual(newContent, _note.Content);
        Assert.AreEqual(newTag, _note.Tag);
        Assert.AreEqual(newColor, _note.Color);
        Assert.IsNotNull(_note.EditedAt);
    }

    /// <summary>
    /// Negative unit test for Edit method in Note Class,
    /// which checks if null title is properly handled.
    /// </summary>
    [TestMethod]
    public void NoteEditNegativeUnitTestNullTitle()
    {
        string title = _note.Title;
        string nullTitle = null;
        string newContent = "New Content";
        string newTag = "New Tag";
        TagColor newColor = TagColor.Red;

        _note.Edit(nullTitle, newContent, newTag, newColor);

        Assert.AreEqual(title, _note.Title);
    }

    /// <summary>
    /// Negative unit test for Edit method in Note Class,
    /// which checks if empty title string is properly handled.
    /// </summary>
    [TestMethod]
    public void NoteEditNegativeUnitTestEmptyTitleString()
    {
        string title = _note.Title;
        string emptyStringTitle = String.Empty;
        string newContent = "New Content";
        string newTag = "New Tag";
        TagColor newColor = TagColor.Red;

        _note.Edit(emptyStringTitle, newContent, newTag, newColor);

        Assert.AreEqual(title, _note.Title);
    }

    /// <summary>
    /// Negative unit test for Edit method in Note Class,
    /// which checks if null content is properly handled.
    /// </summary>
    [TestMethod]
    public void NoteEditNegativeUnitTestNullContent()
    {
        string expectedContent = _note.Content;
        string newTitle = "New Title";
        string nullContent = null;
        string newTag = "New Tag";
        TagColor newColor = TagColor.Red;

        _note.Edit(newTitle, nullContent, newTag, newColor);

        Assert.AreEqual(expectedContent, _note.Content);
    }

    /// <summary>
    /// Negative unit test for Edit method in Note Class,
    /// which checks if null tag is properly handled.
    /// </summary>
    [TestMethod]
    public void NoteEditNegativeUnitTestNullTag()
    {
        string expectedTag = _note.Tag;
        string newTitle = "New Title";
        string newContent = "New Content";
        string nullTag = null;
        TagColor newColor = TagColor.Red;

        _note.Edit(newTitle, newContent, nullTag, newColor);

        Assert.AreEqual(expectedTag, _note.Tag);
    }

    /// <summary>
    /// Negative unit test for Edit method in Note Class,
    /// which checks if invalid TagColor values are properly handled.
    /// </summary>
    /// <param name="newColor">New tag color value used in the test</param>
    [TestMethod]
    [DataRow(Int32.MinValue)]
    [DataRow(-1)]
    [DataRow(6)]
    [DataRow(Int32.MaxValue)]
    public void NoteEditNegativeUnitTestInvalidTagColor(TagColor newColor)
    {
        string newTitle = "New Title";
        string newContent = "New Content";
        string newTag = "New Tag";
        TagColor expectedColor = TagColor.None;

        _note.Edit(newTitle, newContent, newTag, newColor);
        Assert.AreEqual(expectedColor, _note.Color);
    }
    #endregion
    
}
