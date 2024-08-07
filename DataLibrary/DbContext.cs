using ModelLibrary;
using SQLite;
using System.Linq.Expressions;

namespace DataLibrary;

public static class DbContext
{
    private static SQLiteAsyncConnection? db = null;

    public const string DatabaseFilename = "WGUScheduler.db3";

    public const SQLiteOpenFlags Flags =
        // open the database in read/write mode
        SQLiteOpenFlags.ReadWrite |
        // create the database if it doesn't exist
        SQLiteOpenFlags.Create |
        // enable multi-threaded database access
        SQLiteOpenFlags.SharedCache;

    public static string DatabasePath =>
        Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);


    private static async Task Init()
    {
        if (db != null)
            return;

        db = new SQLiteAsyncConnection(DatabasePath, Flags);
        await db.CreateTableAsync<Term>();
        await db.CreateTableAsync<Class>();
        await db.CreateTableAsync<TermSchedule>();
        await db.CreateTableAsync<Instructor>();
        await db.CreateTableAsync<Exam>();
        await db.CreateTableAsync<ExamType>();
        await db.CreateTableAsync<User>();
        await InsertExamTypes();

    }

    private static async Task InsertExamTypes()
    {
        var table = db.Table<ExamType>();
        List<ExamType> examTypesResult = await table.ToListAsync();

        if (examTypesResult.Count == 0)
        {
            ExamType oa = new ExamType() { Type = "Objective Assessment" };
            ExamType pa = new ExamType() { Type = "Performance Assessment" };
            List<ExamType> examTypes = [oa, pa];

            await db.InsertAllAsync(examTypes);
        }
    }

    private static async Task<AsyncTableQuery<TTable>> GetTableAsync<TTable>() where TTable : class, new()
    {
        await Init();
        return db.Table<TTable>();
    }

    public static async Task<List<TTable>> GetAllAsync<TTable>() where TTable : class, new()
    {
        var table = await GetTableAsync<TTable>();
        var resultingList = await table.ToListAsync();
        return resultingList;
    }

    public static async Task<IEnumerable<TTable>> GetFilteredListAsync<TTable>(Expression<Func<TTable, bool>> predicate) where TTable : class, new()
    {
        var table = await GetTableAsync<TTable>();
        var items = await table.Where(predicate).ToListAsync();
        return items;
    }

    public static async Task<TTable> GetFilteredItemAsync<TTable>(Expression<Func<TTable, bool>> predicate) where TTable : class, new()
    {
        var table = await GetTableAsync<TTable>();
        var item = await table.Where(predicate).FirstOrDefaultAsync();
        return item;
    }

    public static async Task<bool> DeleteItemAsync<TTable>(TTable item) where TTable : class, new()
    {
        await Init();
        return await db.DeleteAsync(item) > 0;
    }

    public static async Task<int> DeleteAllItemsAsync<TTable>() where TTable : class, new()
    {
        await Init();
        return await db.DeleteAllAsync<TTable>();
    }


    public static async Task<bool> UpdateItemAsync<TTable>(TTable item) where TTable : class, new()
    {
        await Init();
        return await db.UpdateAsync(item) > 0;
    }
    public static async Task<bool> AddItemAsync<TTable>(TTable item) where TTable : class, new()
    {
        await Init();
        return await db.InsertAsync(item) > 0;
    }

    public static async Task<int> AddAllItemsAsync<TTable>(List<TTable> items) where TTable : class, new()
    {
        await Init();
        return await db.InsertAllAsync(items);
    }

    public static async Task<bool> CheckIfHasRows<TTable>() where TTable : class, new()
    {
        var table = await GetTableAsync<TTable>();
        List<TTable> list = await table.ToListAsync();

        return list != null && list.Count > 0;
    }
}
