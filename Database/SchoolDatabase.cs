using SQLite;
using System.Linq.Expressions;
using System.Diagnostics;

namespace C971_Ogden.Database;

public class SchoolDatabase : IAsyncDisposable
{
    private SQLiteAsyncConnection _connection;
    private SQLiteAsyncConnection Database =>
        (_connection ??= new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags));


    private async Task CreateTableIfNotExists<TTable>() where TTable : class, new()
    {
        var result = await Database.CreateTableAsync<TTable>();

        Debug.WriteLine($"{typeof(TTable)} Table: {result}");
    }

    private async Task<AsyncTableQuery<TTable>> GetTableAsync<TTable>() where TTable : class, new()
    {
        await CreateTableIfNotExists<TTable>();
        return Database.Table<TTable>();
    }

    public async Task<IEnumerable<TTable>> GetAllAsync<TTable>() where TTable : class, new()
    {
        var table = await GetTableAsync<TTable>();
        return await table.ToListAsync();
    }

    public async Task<IEnumerable<TTable>> GetFileteredListAsync<TTable>(Expression<Func<TTable, bool>> predicate) where TTable : class, new()
    {
        var table = await GetTableAsync<TTable>();
        return await table.Where(predicate).ToListAsync();
    }

    public async Task<TTable> GetFileteredItemAsync<TTable>(Expression<Func<TTable, bool>> predicate) where TTable : class, new()
    {
        var table = await GetTableAsync<TTable>();
        return await table.Where(predicate).FirstOrDefaultAsync();
    }

    public async Task<bool> DeleteItemAsync<TTable>(TTable item) where TTable : class, new()
    {
        await CreateTableIfNotExists<TTable>();
        return await Database.DeleteAsync(item) > 0;
    }

    public async Task<bool> DeleteAllItemsAsync<TTable>() where TTable : class, new()
    {
        await CreateTableIfNotExists<TTable>();
        return await Database.DeleteAllAsync<TTable>() > 0;
    }


    public async Task<bool> UpdateItemAsync<TTable>(TTable item) where TTable : class, new()
    {
        await CreateTableIfNotExists<TTable>();
        return await Database.UpdateAsync(item) > 0;
    }
    public async Task<bool> AddItemAsync<TTable>(TTable item) where TTable : class, new()
    {
        await CreateTableIfNotExists<TTable>();
        return await Database.InsertAsync(item) > 0;
    }

    public async Task<int> AddAllItemsAsync<TTable>(List<TTable> items) where TTable : class, new()
    {
        await CreateTableIfNotExists<TTable>();
        return await Database.InsertAllAsync(items);
    }

    public async Task<bool> CheckIfHasRows<TTable>() where TTable : class, new()
    {
        var table = await GetTableAsync<TTable>();
        List<TTable> list = await table.ToListAsync();

        return list != null && list.Count > 0;
    }



    // Extra
    public async ValueTask DisposeAsync() => await _connection?.CloseAsync();
}
