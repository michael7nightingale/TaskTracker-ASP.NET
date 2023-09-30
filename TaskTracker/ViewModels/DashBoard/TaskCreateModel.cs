using TaskTracker.Models;

namespace TaskTracker.ViewModels.DashBoard;

public class TaskCreateModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public List<User> ToUsers { get; set; } = new();
}