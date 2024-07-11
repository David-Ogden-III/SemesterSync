using SQLite;
using System.Linq.Expressions;

namespace C971_Ogden.Database;

public static class SchoolDatabase
{
    private static SQLiteAsyncConnection? db = null;
    private static async Task Init()
    {
        if (db != null)
            return;

        db = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        await db.CreateTableAsync<Term>();
        await db.CreateTableAsync<Class>();
        await db.CreateTableAsync<TermSchedule>();
        await db.CreateTableAsync<Instructor>();
        await db.CreateTableAsync<Exam>();
        await db.CreateTableAsync<ExamType>();
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

    public static async Task<bool> DeleteAllItemsAsync<TTable>() where TTable : class, new()
    {
        await Init();
        return await db.DeleteAllAsync<TTable>() > 0;
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
