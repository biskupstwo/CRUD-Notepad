using Notepad.Core.Constants;
using Notepad.Core.Repositories;
using Notepad.Core.Data;
using Notepad.Core.Models;

namespace Notepad.UnitTests.Repositories;

[TestClass]
public class NoteRepositoryUnitTests
{
    private NotepadDatabase _notepadDatabase;
    private NoteRepository _noteRepository;
    
    [TestInitialize]
    public void Setup()
    {
        string tempDir = Path.GetTempPath(); 
        string databasePath = Path.Combine(tempDir, $"TestDatabase_{Guid.NewGuid()}.db");
        if (File.Exists(databasePath)) File.Delete(databasePath);

        _notepadDatabase = new NotepadDatabase(databasePath);
        _notepadDatabase.Database.EnsureCreated();
        if (!_notepadDatabase.Folders.Any(f => f.Id == Constants.DefaultFolderId))
        {
            _notepadDatabase.Folders.Add(new Folder(Constants.DefaultFolderName) { Id = Constants.DefaultFolderId });
            _notepadDatabase.SaveChanges();
        }

        _noteRepository = new NoteRepository(_notepadDatabase);
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (File.Exists(_notepadDatabase.DatabasePath)) File.Delete(_notepadDatabase.DatabasePath);
        _notepadDatabase = null;
    }

    /// <summary>
    /// Helper function for some unit tests, which adds three notes to the Notepad Database
    /// </summary>
    public async Task AddThreeNotesToNotepadDatabaseAsync()
    {
        _ = await _noteRepository.AddAsync("Note1", "Content1", "Tag1", TagColor.None);
        _ = await _noteRepository.AddAsync("Note2", "Content2", "Tag2", TagColor.None);
        _ = await _noteRepository.AddAsync("Note3", "Content3", "Tag3", TagColor.None);
    }
    
    #region GetAllAsync unit tests 
    
    /// <summary>
    /// Positive unit test for GetAllAsync Method in NoteRepository class
    /// </summary>
    [TestMethod]
    public async Task GetAllAsyncUnitTest()
    {
        int expectedSize = 3;
        string expectedFirstNoteTitle = "Note1";
        string expectedSecondNoteTitle = "Note2";
        string expectedThirdNoteTitle = "Note3";
        
        await AddThreeNotesToNotepadDatabaseAsync();
        var notes = await _noteRepository.GetAllAsync();
    
        var titles = notes.Select(n => n.Title).ToList();
        Assert.AreEqual(expectedSize, notes.Count());     
        
        CollectionAssert.AreEquivalent(new[] { expectedFirstNoteTitle, expectedSecondNoteTitle, expectedThirdNoteTitle }, titles);
    }

    /// <summary>
    /// Positive unit test for GetAllAsync Method in NoteRepository class
    /// </summary>
    [TestMethod]
    public async Task GetAllAsyncUnitTestEmptyNotes()
    {
        int expectedSize = 0;
        
        var notes = await _noteRepository.GetAllAsync();
        
        Assert.AreEqual(expectedSize, notes.Count());
    }
    
    #endregion

    #region AddAsync unit tests

    /// <summary>
    /// Positive unit test for AddAsync Method in NoteRepository class,
    /// which checks if AddAsync correctly adds a new note when passing a Note object.
    /// </summary>
    [TestMethod]
    public async Task AddAsyncUnitTest_NoteParameter()
    {
        Note note = new Note("Note1", "Content1", "Tag1", TagColor.None);
        
        var result = await _noteRepository.AddAsync(note);
        var noteInNotepadDatabase = await _noteRepository.ReadAsync(1);
        
        Assert.AreEqual(note, result);
        Assert.AreEqual(note, noteInNotepadDatabase);
    }
    
    /// <summary>
    /// Negative unit test for AddAsync Method in NoteRepository class,
    /// which checks if AddAsync returns null when a null Note object is passed.
    /// </summary>
    [TestMethod]
    public async Task AddAsyncNegativeUnitTest_NoteParameterNullNote()
    {
        Note expectedResult = null;
        var actualResult = await _noteRepository.AddAsync(expectedResult);
        Assert.AreEqual(actualResult, expectedResult);
    }

    /// <summary>
    /// Positive unit test for AddAsync Method in NoteRepository class,
    /// which checks if AddAsync correctly adds a new note when passing individual fields (title, content, tag, color).
    /// </summary>
    [TestMethod]
    public async Task AddAsyncUnitTest_FieldsParameter()
    {
        var note = await _noteRepository.AddAsync("Note1", "Content1", "Tag1", TagColor.None);
        var noteInNotepadDatabase = await _noteRepository.ReadAsync(1);
        
        Assert.AreEqual(note, noteInNotepadDatabase);
    }
    
    /// <summary>
    /// Negative unit test for AddAsync Method in NoteRepository class,
    /// which checks if null title is handled properly.
    /// </summary>
    [TestMethod]
    public async Task AddAsyncNegativeUnitTest_FieldsParameterNullTitle()
    {
        var expectedResult = "TITLE PLACEHOLDER";
        var note = await _noteRepository.AddAsync(null, "Content1", "Tag1", TagColor.None);
        var noteInNotepadDatabase = await _noteRepository.ReadAsync(1);
        
        Assert.AreEqual(expectedResult, note.Title);
        Assert.AreEqual(expectedResult, noteInNotepadDatabase.Title);
    }
    
    /// <summary>
    /// Negative unit test for AddAsync Method in NoteRepository class,
    /// which checks if null content is handled properly.
    /// </summary>
    [TestMethod]
    public async Task AddAsyncNegativeUnitTest_FieldsParameterNullContent()
    {
        var expectedResult = String.Empty;
        var note = await _noteRepository.AddAsync("Title1", null, "Tag1", TagColor.None);
        var noteInNotepadDatabase = await _noteRepository.ReadAsync(1);
        
        Assert.AreEqual(expectedResult, note.Content);
        Assert.AreEqual(expectedResult, noteInNotepadDatabase.Content);
    }
    
    /// <summary>
    /// Negative unit test for AddAsync Method in NoteRepository class,
    /// which checks if null tag is handled properly.
    /// </summary>
    [TestMethod]
    public async Task AddAsyncNegativeUnitTest_FieldsParameterNullTag()
    {
        var expectedResult = String.Empty;
        var note = await _noteRepository.AddAsync("Title1", "Content1", null, TagColor.None);
        var noteInNotepadDatabase = await _noteRepository.ReadAsync(1);
        
        Assert.AreEqual(expectedResult, note.Tag);
        Assert.AreEqual(expectedResult, noteInNotepadDatabase.Tag);
    }
    
    /// <summary>
    /// Negative unit test for AddAsync Method in NoteRepository class,
    /// which checks if invalid parameters are handled properly.
    /// </summary>
    [TestMethod]
    public async Task AddAsyncNegativeUnitTest_FieldsParameterInvalidParameters()
    {
        var expectedTitle = "TITLE PLACEHOLDER";
        var expectedContent = String.Empty;
        var expectedTag = String.Empty;
        var expectedColor = TagColor.None;
        
        var note = await _noteRepository.AddAsync(null, null, null, (TagColor) 6);
        var noteInNotepadDatabase = await _noteRepository.ReadAsync(1);
        
        Assert.AreEqual(expectedTitle, note.Title);
        Assert.AreEqual(expectedTitle, noteInNotepadDatabase.Title);
        Assert.AreEqual(expectedContent, note.Content);
        Assert.AreEqual(expectedContent, noteInNotepadDatabase.Content);
        Assert.AreEqual(expectedTag, note.Tag);
        Assert.AreEqual(expectedTag, noteInNotepadDatabase.Tag);
        Assert.AreEqual(expectedColor, note.Color);
        Assert.AreEqual(expectedColor, noteInNotepadDatabase.Color);
    }
    
    #endregion

    #region ReadAsync unit tests

    /// <summary>
    /// Positive unit test for ReadAsync Method in NoteRepository class
    /// </summary>
    [TestMethod]
    public async Task ReadAsyncUnitTest()
    {
        string expectedNoteTitle = "Note1";
        await AddThreeNotesToNotepadDatabaseAsync();
        
        var note = await _noteRepository.ReadAsync(1);

        Assert.IsNotNull(note);
        Assert.AreEqual(expectedNoteTitle, note.Title);
    }
    
    /// <summary>
    /// Negative unit test for ReadAsync Method in NoteRepository class
    /// </summary>
    [TestMethod]
    public async Task ReadAsyncNegativeUnitTest()
    {
        var note = await _noteRepository.ReadAsync(1);

        Assert.IsNull(note);
    }

    #endregion

    #region UpdateAsync unit tests

    /// <summary>
    /// Positive unit test for UpdateAsync Method in NoteRepository class
    /// </summary>
    [TestMethod]
    public async Task UpdateAsyncUnitTest_NoteParameter()
    {
        var updatedNote = new Note("New title", "New Content", "NewTag", TagColor.Red);
        await AddThreeNotesToNotepadDatabaseAsync();
        
        var noteInDatabase = await _noteRepository.ReadAsync(1);
        
        Assert.AreNotEqual(updatedNote.Title, noteInDatabase.Title);
        Assert.AreNotEqual(updatedNote.Content, noteInDatabase.Content);
        Assert.AreNotEqual(updatedNote.Tag, noteInDatabase.Tag);
        Assert.AreNotEqual(updatedNote.Color, noteInDatabase.Color);
        
        await _noteRepository.UpdateAsync(1, updatedNote);
        var updatedNoteInDatabase = await _noteRepository.ReadAsync(1);
        
        Assert.AreEqual(updatedNote.Title, updatedNoteInDatabase.Title);
        Assert.AreEqual(updatedNote.Content, updatedNoteInDatabase.Content);
        Assert.AreEqual(updatedNote.Tag, updatedNoteInDatabase.Tag);
        Assert.AreEqual(updatedNote.Color, updatedNoteInDatabase.Color);
    }
    
    /// <summary>
    /// Negative unit test for UpdateAsync Method in NoteRepository class,
    /// which checks if null note is handled properly.
    /// </summary>
    [TestMethod]
    public async Task UpdateAsyncNegativeUnitTest_NoteParameterNullNote()
    {
        await AddThreeNotesToNotepadDatabaseAsync();
        Note? updatedNote = null;
        
        string expectedTitle = "Note1";
        string expectedContent = "Content1";
        string expectedTag = "Tag1";
        TagColor expectedColor = TagColor.None;
        
        var noteInDatabase = await _noteRepository.ReadAsync(1);
        
        Assert.AreEqual(expectedTitle, noteInDatabase.Title);
        Assert.AreEqual(expectedContent, noteInDatabase.Content);
        Assert.AreEqual(expectedTag, noteInDatabase.Tag);
        Assert.AreEqual(expectedColor, noteInDatabase.Color);
        
        await _noteRepository.UpdateAsync(1, updatedNote);
        var updatedNoteInDatabase = await _noteRepository.ReadAsync(1);
        
        Assert.AreEqual(expectedTitle, updatedNoteInDatabase.Title);
        Assert.AreEqual(expectedContent, updatedNoteInDatabase.Content);
        Assert.AreEqual(expectedTag, updatedNoteInDatabase.Tag);
        Assert.AreEqual(expectedColor, updatedNoteInDatabase.Color);
    }
    
    /// <summary>
    /// Negative unit test for UpdateAsync Method in NoteRepository class
    /// </summary>
    [TestMethod]
    public async Task UpdateAsyncNegativeUnitTest_NoteParameterNoteDoesntExist()
    {
        var updatedNote = new Note("New title", "New Content", "NewTag", TagColor.Red);
        var noteInDatabase = await _noteRepository.ReadAsync(1);
        
        Assert.IsNull(noteInDatabase);
        
        await _noteRepository.UpdateAsync(1, updatedNote);
        var updatedNoteInDatabase = await _noteRepository.ReadAsync(1);
        
        Assert.IsNull(updatedNoteInDatabase);
    }

    /// <summary>
    /// Positive unit test for UpdateAsync Method in NoteRepository class
    /// </summary>
    [TestMethod]
    public async Task UpdateAsyncUnitTest_FieldsParameter()
    {
        await AddThreeNotesToNotepadDatabaseAsync();
        string newTitle = "New title";
        string newContent = "New Content";
        string newTag = "NewTag";
        TagColor newColor = TagColor.Red;
        
        
        var noteInDatabase = await _noteRepository.ReadAsync(1);
        
        Assert.AreNotEqual(newTitle, noteInDatabase.Title);
        Assert.AreNotEqual(newContent, noteInDatabase.Content);
        Assert.AreNotEqual(newTag, noteInDatabase.Tag);
        Assert.AreNotEqual(newColor, noteInDatabase.Color);
        
        await _noteRepository.UpdateAsync(1, "New title", "New Content", "NewTag", TagColor.Red);
        var updatedNoteInDatabase = await _noteRepository.ReadAsync(1);
        
        Assert.AreEqual(newTitle, updatedNoteInDatabase.Title);
        Assert.AreEqual(newContent, updatedNoteInDatabase.Content);
        Assert.AreEqual(newTag, updatedNoteInDatabase.Tag);
        Assert.AreEqual(newColor, updatedNoteInDatabase.Color);
    }
    
    /// <summary>
    /// Negative unit test for UpdateAsync Method in NoteRepository class,
    /// which checks if null note is handled properly.
    /// </summary>
    [TestMethod]
    public async Task UpdateAsyncNegativeUnitTest_FieldsParameterNoteDoesntExist()
    {
        var noteInDatabase = await _noteRepository.ReadAsync(1);
        
        Assert.IsNull(noteInDatabase);
        
        await _noteRepository.UpdateAsync(1, "New title", "New Content", "NewTag", TagColor.Red);
        var updatedNoteInDatabase = await _noteRepository.ReadAsync(1);
        
        Assert.IsNull(updatedNoteInDatabase);
    }
    
    /// <summary>
    /// Negative unit test for UpdateAsync Method in NoteRepository class,
    /// which checks if null title is handled properly.
    /// </summary>
    [TestMethod]
    public async Task UpdateAsyncNegativeUnitTest_FieldsParameterNullTitleField()
    {
        await AddThreeNotesToNotepadDatabaseAsync();
        
        string expectedTitle = "Note1";
        string expectedContent = "Content2";
        string expectedTag = "Tag2";
        TagColor expectedColor = TagColor.Red;
        
        var noteInDatabase = await _noteRepository.ReadAsync(1);
        
        Assert.AreEqual(expectedTitle, noteInDatabase.Title);
        Assert.AreNotEqual(expectedContent, noteInDatabase.Content);
        Assert.AreNotEqual(expectedTag, noteInDatabase.Tag);
        Assert.AreNotEqual(expectedColor, noteInDatabase.Color);
        
        await _noteRepository.UpdateAsync(1, null, expectedContent, expectedTag, expectedColor);
        var updatedNoteInDatabase = await _noteRepository.ReadAsync(1);
        
        Assert.AreEqual(expectedTitle, updatedNoteInDatabase.Title);
        Assert.AreEqual(expectedContent, updatedNoteInDatabase.Content);
        Assert.AreEqual(expectedTag, updatedNoteInDatabase.Tag);
        Assert.AreEqual(expectedColor, updatedNoteInDatabase.Color);
    }
    
    /// <summary>
    /// Negative unit test for UpdateAsync Method in NoteRepository class,
    /// which checks if null content is handled properly.
    /// </summary>
    [TestMethod]
    public async Task UpdateAsyncNegativeUnitTest_FieldsParameterNullContentField()
    {
        await AddThreeNotesToNotepadDatabaseAsync();
        
        string expectedTitle = "Note2";
        string expectedContent = "Content1";
        string expectedTag = "Tag2";
        TagColor expectedColor = TagColor.Red;
        
        var noteInDatabase = await _noteRepository.ReadAsync(1);
        
        Assert.AreNotEqual(expectedTitle, noteInDatabase.Title);
        Assert.AreEqual(expectedContent, noteInDatabase.Content);
        Assert.AreNotEqual(expectedTag, noteInDatabase.Tag);
        Assert.AreNotEqual(expectedColor, noteInDatabase.Color);
        
        await _noteRepository.UpdateAsync(1, expectedTitle, null, expectedTag, expectedColor);
        var updatedNoteInDatabase = await _noteRepository.ReadAsync(1);
        
        Assert.AreEqual(expectedTitle, updatedNoteInDatabase.Title);
        Assert.AreEqual(expectedContent, updatedNoteInDatabase.Content);
        Assert.AreEqual(expectedTag, updatedNoteInDatabase.Tag);
        Assert.AreEqual(expectedColor, updatedNoteInDatabase.Color);
    }
    
    /// <summary>
    /// Negative unit test for UpdateAsync Method in NoteRepository class,
    /// which checks if null tag is handled properly.
    /// </summary>
    [TestMethod]
    public async Task UpdateAsyncNegativeUnitTest_FieldsParameterNullTagField()
    {
        await AddThreeNotesToNotepadDatabaseAsync();
        
        string expectedTitle = "Note2";
        string expectedContent = "Content2";
        string expectedTag = "Tag1";
        TagColor expectedColor = TagColor.Red;
        
        var noteInDatabase = await _noteRepository.ReadAsync(1);
        
        Assert.AreNotEqual(expectedTitle, noteInDatabase.Title);
        Assert.AreNotEqual(expectedContent, noteInDatabase.Content);
        Assert.AreEqual(expectedTag, noteInDatabase.Tag);
        Assert.AreNotEqual(expectedColor, noteInDatabase.Color);
        
        await _noteRepository.UpdateAsync(1, expectedTitle, expectedContent, null, expectedColor);
        var updatedNoteInDatabase = await _noteRepository.ReadAsync(1);
        
        Assert.AreEqual(expectedTitle, updatedNoteInDatabase.Title);
        Assert.AreEqual(expectedContent, updatedNoteInDatabase.Content);
        Assert.AreEqual(expectedTag, updatedNoteInDatabase.Tag);
        Assert.AreEqual(expectedColor, updatedNoteInDatabase.Color);
    }
    
    /// <summary>
    /// Negative unit test for UpdateAsync Method in NoteRepository class,
    /// which checks if invalid parameters are handled properly.
    /// </summary>
    [TestMethod]
    public async Task UpdateAsyncNegativeUnitTest_FieldsParameterInvalidFields()
    {
        await AddThreeNotesToNotepadDatabaseAsync();
        
        string expectedTitle = "Note1";
        string expectedContent = "Content1";
        string expectedTag = "Tag1";
        TagColor expectedColor = TagColor.None;
        
        var noteInDatabase = await _noteRepository.ReadAsync(1);
        
        Assert.AreEqual(expectedTitle, noteInDatabase.Title);
        Assert.AreEqual(expectedContent, noteInDatabase.Content);
        Assert.AreEqual(expectedTag, noteInDatabase.Tag);
        Assert.AreEqual(expectedColor, noteInDatabase.Color);
        
        await _noteRepository.UpdateAsync(1, null, null, null, (TagColor) 6);
        var updatedNoteInDatabase = await _noteRepository.ReadAsync(1);
        
        Assert.AreEqual(expectedTitle, updatedNoteInDatabase.Title);
        Assert.AreEqual(expectedContent, updatedNoteInDatabase.Content);
        Assert.AreEqual(expectedTag, updatedNoteInDatabase.Tag);
        Assert.AreEqual(expectedColor, updatedNoteInDatabase.Color);
    }
    #endregion


    #region DeleteAsync unit tests

    /// <summary>
    /// Positive unit test for DeleteAsync Method in NoteRepository class
    /// </summary>
    [TestMethod]
    public async Task DeleteAsyncUnitTest()
    {
        await AddThreeNotesToNotepadDatabaseAsync();
        
        var noteInDatabase = await _noteRepository.ReadAsync(1);
        Assert.IsNotNull(noteInDatabase);

        bool result = await _noteRepository.DeleteAsync(1);
        Assert.IsTrue(result);
    }
    
    /// <summary>
    /// Negative unit test for DeleteAsync Method in NoteRepository class
    /// </summary>
    [TestMethod]
    public async Task DeleteAsyncNegativeUnitTestNoteDoesntExist()
    {
        var noteInDatabase = await _noteRepository.ReadAsync(1);
        Assert.IsNull(noteInDatabase);

        bool result = await _noteRepository.DeleteAsync(1);
        Assert.IsFalse(result);
    }

    #endregion
    
}
