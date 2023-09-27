using TodoApp.Models;

namespace TodoApp.ViewModels.DashBoard;

public class TaskCreateModel
{
    public string Title { get; set; }
    public string Desciprion { get; set; }
    public List<User> ToUsers { get; set; } = new();
}