using Microsoft.EntityFrameworkCore;
using TaskTracker.Repositories.Base;
using Task = TaskTracker.Models;


namespace TaskTracker.Repositories;

public class TaskRepository : BaseRepository<Task.Task>
{
    public TaskRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<List<Task.Task>> GetTasksByDashBoard(string id)
    {
        return await Entity.Where(t => t.DashBoard.Id == id).ToListAsync();
    }
    
}
