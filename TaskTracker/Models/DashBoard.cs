using TaskTracker.Models.Base;

namespace TaskTracker.Models;


public class DashBoard : BaseModel
{   
    public User Creator { get; set; }
    public string Title { get; set; }
    public List<User> Users { get; set; } = new();
    public List<Task> Tasks { get; set; } = new();

}