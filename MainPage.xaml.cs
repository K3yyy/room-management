using RoomManagementApp.Services;
using RoomManagementApp.ViewModels;

namespace RoomManagementApp;

public partial class MainPage : ContentPage
{
    private readonly RoomViewModel _viewModel;

    public MainPage()
    {
        InitializeComponent();

        var service = new RoomService();

        _viewModel = new RoomViewModel(service);

        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.LoadRoomsCommand.ExecuteAsync(null);
    }
}