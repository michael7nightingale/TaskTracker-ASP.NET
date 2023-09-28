using TaskTracker.Models.Base;

namespace TaskTracker.Models;

public record Invitation : BaseModel
{
    public string InviterId { get; set; }
    public User? Inviter { get; set; } = null;
    public string InvitedId { get; set; }
    public User? Invited { get; set; } = null;
    public string ToDashBoardId { get; set; }
    public DashBoard ToDashBoard { get; set; }
    public DateTime TimeCreated { get; set; }

    public Invitation()
    {
        TimeCreated = DateTime.Now;
    }
    
}
