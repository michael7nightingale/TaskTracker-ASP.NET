using TaskTracker.Models.Base;

namespace TaskTracker.Models;

public record Task : BaseModel
{
    public string DashBoardId { get; set; }
    public DashBoard? DashBoard { get; set; } = null;
    public string CreatorId { get; set; }
    public User? Creator { get; set; } = null;
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