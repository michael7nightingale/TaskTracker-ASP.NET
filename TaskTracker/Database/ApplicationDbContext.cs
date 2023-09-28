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
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DashBoard>()
            .HasMany(d => d.Tasks)
            .WithOne()
            .HasForeignKey(t => t.DashBoardId)
            .IsRequired();
        modelBuilder.Entity<DashBoard>()
            .HasMany(d => d.Users)
            .WithMany(u => u.DashBoards);
        modelBuilder.Entity<DashBoard>()
            .HasOne(d => d.Creator)
            .WithMany(u => u.CreatedDashBoards)
            .HasForeignKey(d => d.CreatorId);
        modelBuilder.Entity<Invitation>()
            .HasOne(i => i.Invited)
            .WithMany(u => u.IncomingInvitations)
            .HasForeignKey(i => i.InvitedId);
        modelBuilder.Entity<Invitation>()
            .HasOne(i => i.Inviter)
            .WithMany(u => u.OutcomingInvitations)
            .HasForeignKey(i => i.InviterId);
        base.OnModelCreating(modelBuilder);
    }
    
}