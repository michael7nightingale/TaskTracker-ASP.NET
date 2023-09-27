using TodoApp.Repositories.Base;
using Task = TodoApp.Models;


namespace TodoApp.Repositories;

public class TaskRepository : BaseRepository<Task.Task>
{
    public TaskRepository(ApplicationDbContext context)
        : base(context)
    {
    }
    
    
}
