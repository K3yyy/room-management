using SQLite;

namespace RoomManagementApp.Models;

[Table("Rooms")]
public class RoomItem
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string RoomNum { get; set; } = string.Empty;

    public string RoomType { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public string Status { get; set; } = "Available";
}