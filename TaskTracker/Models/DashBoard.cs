using TaskTracker.Models.Base;

namespace TaskTracker.Models;


public record DashBoard : BaseModel
{
    public string CreatorId { get; set; }
    public User? Creator { get; set; } = null;
    public string Title { get; set; }
    public List<User> Users { get; set; } = new();
    public List<Task> Tasks { get; set; } = new();

}