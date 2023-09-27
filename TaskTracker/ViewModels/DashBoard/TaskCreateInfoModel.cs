using TaskTracker.Models;

namespace TaskTracker.ViewModels.DashBoard;

public class TaskCreateInfoModel
{
    public List<User> Users { get; set; } = new();
    public Models.DashBoard DashBoard { get; set; }
}