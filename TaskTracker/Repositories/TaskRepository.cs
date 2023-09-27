using Microsoft.EntityFrameworkCore;
using TodoApp.Repositories.Base;
using Task = TodoApp.Models;


namespace TodoApp.Repositories;

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
