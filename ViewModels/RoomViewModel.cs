using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RoomManagementApp.Models;
using RoomManagementApp.Services;
using System.Collections.ObjectModel;

namespace RoomManagementApp.ViewModels;

public partial class RoomViewModel : ObservableObject
{
    private readonly RoomService _service;

    public ObservableCollection<RoomItem> Rooms { get; } = new();

    public List<string> RoomTypes { get; } =
        new() { "Single", "Double", "VIP" };

    public List<string> Statuses { get; } =
        new() { "Available", "Occupied" };

    [ObservableProperty]
    private string roomNum = string.Empty;

    [ObservableProperty]
    private string roomType = "Single";

    [ObservableProperty]
    private decimal price;

    [ObservableProperty]
    private string status = "Available";

    [ObservableProperty]
    private string searchText = string.Empty;

    [ObservableProperty]
    private string message = string.Empty;

    public RoomViewModel(RoomService service)
    {
        _service = service;
    }

    // Initialize the database and load rooms.
    [RelayCommand]
    public async Task LoadRoomsAsync()
    {
        await _service.InitializeAsync();
        await RefreshRoomsAsync();
    }

    // Retrieve all rooms and update the list.
    private async Task RefreshRoomsAsync()
    {
        var list = await _service.RetrieveAsync();

        Rooms.Clear();

        foreach (var room in list)
        {
            Rooms.Add(room);
        }
    }

    // Add a new room.
    [RelayCommand]
    public async Task AddRoomAsync()
    {
        if (string.IsNullOrWhiteSpace(RoomNum))
        {
            Message = "Please enter a room number.";
            return;
        }

        if (string.IsNullOrWhiteSpace(RoomType) || Price <= 0)
        {
            Message = "Please enter a valid room type and price.";
            return;
        }

        await _service.InitializeAsync();

        var room = new RoomItem
        {
            RoomNum = RoomNum.Trim(),
            RoomType = RoomType,
            Price = Price,
            Status = Status
        };

        await _service.InsertAsync(room);

        Message = "Room added successfully!";

        RoomNum = string.Empty;
        RoomType = "Single";
        Price = 0;
        Status = "Available";
        SearchText = string.Empty;

        await RefreshRoomsAsync();
    }

    // Search for a room.
    [RelayCommand]
    public async Task SearchRoomAsync()
    {
        await _service.InitializeAsync();

        var results = await _service.FilterAsync(SearchText);

        Rooms.Clear();

        foreach (var room in results)
        {
            Rooms.Add(room);
        }

        Message = $"{Rooms.Count} room(s) found.";
    }

    // Display every room.
    [RelayCommand]
    public async Task ShowAllRoomsAsync()
    {
        SearchText = string.Empty;

        await RefreshRoomsAsync();

        Message = "Showing all rooms.";
    }
}