using Microsoft.AspNetCore.Identity;

namespace TaskTracker.Models;

public class User : IdentityUser
{
    public List<DashBoard> DashBoards { get; set; } = new();
    public List<DashBoard> CreatedDashBoards { get; set; } = new();
    public List<Invitation> IncomingInvitations { get; set; } = new();
    public List<Invitation> OutcomingInvitations { get; set; } = new();
}