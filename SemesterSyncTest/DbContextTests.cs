using DataLibrary;
using ModelLibrary;

namespace SemesterSyncTest;

public class DbContextTests
{
    [Fact]
    public async Task InsertItem_return1()
    {
        MockDbContext.db = null;
        Class newClass = new()
        {
            ClassName = "Test",
            Status = "Active",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(1),
            Notes = String.Empty,
            InstructorId = 0,
            CreatedBy = "Tester"
        };

        bool itemAdded = await MockDbContext.AddItemAsync(newClass);

        Assert.True(itemAdded);
        await MockDbContext.db.CloseAsync();
    }

    [Fact]
    public async Task InsertListOfItems_returnListCount()
    {
        MockDbContext.db = null;

        Class newClass1 = new()
        {
            ClassName = "Test",
            Status = "Active",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(1),
            Notes = String.Empty,
            InstructorId = 0,
            CreatedBy = "Tester"
        };

        Class newClass2 = new()
        {
            ClassName = "Test",
            Status = "Active",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(1),
            Notes = String.Empty,
            InstructorId = 0,
            CreatedBy = "Tester"
        };

        List<Class> classes = [newClass1, newClass2];
        int classCount = classes.Count;

        int rowsAdded = await MockDbContext.AddAllItemsAsync(classes);

        Assert.Equal(classCount, rowsAdded);

        await MockDbContext.db.CloseAsync();
    }

    [Fact]
    public async Task GetAll_return2Items()
    {
        MockDbContext.db = null;
        Class newClass1 = new()
        {
            ClassName = "Test",
            Status = "Active",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(1),
            Notes = String.Empty,
            InstructorId = 0,
            CreatedBy = "Tester"
        };
        Class newClass2 = new()
        {
            ClassName = "Test",
            Status = "Active",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(1),
            Notes = String.Empty,
            InstructorId = 0,
            CreatedBy = "Tester"
        };

        List<Class> classes = [newClass1, newClass2];
        int rowsAdded = await MockDbContext.AddAllItemsAsync(classes);

        var retrievedClasses = await MockDbContext.GetAllAsync<Class>();
        int retrievedCount = retrievedClasses.Count;

        Assert.Equal(rowsAdded, retrievedCount);

        await MockDbContext.db.CloseAsync();
    }

    [Fact]
    public async Task GetFilteredItem_return1Item()
    {
        MockDbContext.db = null;
        Class newClass = new()
        {
            ClassName = "Test123",
            Status = "Active",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(1),
            Notes = String.Empty,
            InstructorId = 0,
            CreatedBy = "Tester"
        };
        await MockDbContext.AddItemAsync(newClass);

        Class retrievedClass = await MockDbContext.GetFilteredItemAsync<Class>(c  => c.ClassName == "Test123" && c.CreatedBy == "Tester");

        Assert.NotNull(retrievedClass);

        await MockDbContext.db.CloseAsync();
    }

    [Fact]
    public async Task GetFilteredList_returnListCount()
    {
        MockDbContext.db = null;

        Class newClass1 = new()
        {
            ClassName = "Test456",
            Status = "Active",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(1),
            Notes = String.Empty,
            InstructorId = 0,
            CreatedBy = "Tester"
        };
        Class newClass2 = new()
        {
            ClassName = "Test789",
            Status = "Active",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(1),
            Notes = String.Empty,
            InstructorId = 0,
            CreatedBy = "Tester"
        };
        List<Class> classes = [newClass1, newClass2];
        int rowsAdded = await MockDbContext.AddAllItemsAsync(classes);

        var retrievedClasses = await MockDbContext.GetFilteredListAsync<Class>(c => c.CreatedBy == "Tester");
        int retrievedClassCount = retrievedClasses.Count();

        Assert.Equal(rowsAdded, retrievedClassCount);

        await MockDbContext.db.CloseAsync();
    }

    [Fact]
    public async Task DeleteItem_returnTrue()
    {
        MockDbContext.db = null;
        Class newClass = new()
        {
            ClassName = "Test123",
            Status = "Active",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(1),
            Notes = String.Empty,
            InstructorId = 0,
            CreatedBy = "Tester"
        };
        await MockDbContext.AddItemAsync(newClass);

        bool classDeleted = await MockDbContext.DeleteItemAsync(newClass);

        Assert.True(classDeleted);

        await MockDbContext.db.CloseAsync();
    }

    [Fact]
    public async Task DeleteAllItems_returnDeleteCount()
    {
        MockDbContext.db = null;
        Class newClass1 = new()
        {
            ClassName = "Test",
            Status = "Active",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(1),
            Notes = String.Empty,
            InstructorId = 0,
            CreatedBy = "Tester"
        };
        Class newClass2 = new()
        {
            ClassName = "Test",
            Status = "Active",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(1),
            Notes = String.Empty,
            InstructorId = 0,
            CreatedBy = "Tester"
        };
        List<Class> classes = [newClass1, newClass2];
        int rowsAdded = await MockDbContext.AddAllItemsAsync(classes);

        int rowsDeleted = await MockDbContext.DeleteAllItemsAsync<Class>();

        Assert.Equal(rowsAdded, rowsDeleted);

        await MockDbContext.db.CloseAsync();
    }

    [Fact]
    public async Task UpdateItem_returnTrue()
    {
        MockDbContext.db = null;
        Class newClass = new()
        {
            ClassName = "Test123",
            Status = "Active",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(1),
            Notes = String.Empty,
            InstructorId = 0,
            CreatedBy = "Tester"
        };
        await MockDbContext.AddItemAsync(newClass);

        newClass.ClassName = "Test987";

        await MockDbContext.UpdateItemAsync(newClass);

        Class updatedClass = await MockDbContext.GetFilteredItemAsync<Class>(c => c.Id == newClass.Id);

        Assert.Equal("Test987", updatedClass.ClassName);

        await MockDbContext.db.CloseAsync();
    }

    [Fact]
    public async Task CheckIfHasRows_returTrue()
    {
        MockDbContext.db = null;

        Class newClass1 = new()
        {
            ClassName = "Test",
            Status = "Active",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(1),
            Notes = String.Empty,
            InstructorId = 0,
            CreatedBy = "Tester"
        };

        Class newClass2 = new()
        {
            ClassName = "Test",
            Status = "Active",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(1),
            Notes = String.Empty,
            InstructorId = 0,
            CreatedBy = "Tester"
        };

        List<Class> classes = [newClass1, newClass2];
        await MockDbContext.AddAllItemsAsync(classes);

        bool classTableHasRows = await MockDbContext.CheckIfHasRows<Class>();

        Assert.True(classTableHasRows);

        await MockDbContext.db.CloseAsync();
    }

    [Fact]
    public async Task CheckIfHasRows_returnFalse()
    {
        MockDbContext.db = null;
        bool classTableHasRows = await MockDbContext.CheckIfHasRows<Class>();

        Assert.False(classTableHasRows);
    }
}
