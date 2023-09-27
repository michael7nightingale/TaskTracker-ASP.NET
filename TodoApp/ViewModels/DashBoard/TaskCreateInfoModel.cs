using TodoApp.Models;

namespace TodoApp.ViewModels.DashBoard;

public class TaskCreateInfoModel
{
    public List<User> Users { get; set; } = new();
    public Models.DashBoard DashBoard { get; set; }
}