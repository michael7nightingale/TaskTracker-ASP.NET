using TodoApp.Models.Base;

namespace TodoApp.Models;

public class Invitation : BaseModel
{
    public User Inviter { get; set; }
    public User Invited { get; set; }
    public DashBoard ToDashBoard { get; set; }
    public DateTime TimeCreated { get; set; }

    public Invitation()
    {
        TimeCreated = DateTime.Now;
    }
    
}
