using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using TaskTracker.Models;
using Task = TaskTracker.Models.Task;


public class ApplicationDbContext : IdentityDbContext<User>
{
    public new DbSet<User> Users { get; set; } 
    public DbSet<DashBoard> DashBoards { get; set; }
    public DbSet<Invitation> Invitations { get; set; }
    public DbSet<Task> Tasks { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
    
    public DbSet<TaskTracker.Models.Base.BaseModel>? BaseModel { get; set; }
}