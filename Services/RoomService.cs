using SQLite;
using RoomManagementApp.Models;

namespace RoomManagementApp.Services;

public class RoomService
{
    private readonly SQLiteAsyncConnection _database;

    public RoomService()
    {
        string dbPath = Path.Combine(
            FileSystem.AppDataDirectory,
            "rooms.db3");

        _database = new SQLiteAsyncConnection(dbPath);
    }

    // Create the Rooms table if it does not exist.
    public async Task InitializeAsync()
    {
        await _database.CreateTableAsync<RoomItem>();
    }

    // Insert a new room.
    public async Task<int> InsertAsync(RoomItem room)
    {
        return await _database.InsertAsync(room);
    }

    // Retrieve all rooms.
    public async Task<List<RoomItem>> RetrieveAsync()
    {
        return await _database.Table<RoomItem>()
            .OrderBy(r => r.RoomNum)
            .ToListAsync();
    }

    // Search rooms by room number.
    public async Task<List<RoomItem>> FilterAsync(string roomNum)
    {
        if (string.IsNullOrWhiteSpace(roomNum))
        {
            return await RetrieveAsync();
        }

        string search = roomNum.Trim();

        return await _database.Table<RoomItem>()
            .Where(r => r.RoomNum.Contains(search))
            .OrderBy(r => r.RoomNum)
            .ToListAsync();
    }
}