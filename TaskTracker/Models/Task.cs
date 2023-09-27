using TaskTracker.Models.Base;

namespace TaskTracker.Models;

public class Task : BaseModel
{
    public DashBoard DashBoard { get; set; }
    public User Creator { get; set; }
    public string Title { get; set; }
    public string Description { get; set; } = "";
    public DateTime TimeCreated { get; set; }
    public DateTime TimeFinished { get; set; }
    public bool IsFinished { get; set; } = false;
    public List<User> ToUsers { get; set; } = new();

    public Task()
    {
        TimeCreated = DateTime.Now;
    }
        
}