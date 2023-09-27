using Microsoft.EntityFrameworkCore.ChangeTracking;
using TodoApp.Models;
using TodoApp.Repositories.Base;
using Task = System.Threading.Tasks.Task;

namespace TodoApp.Repositories;

public class DashBoardRepository: BaseRepository<DashBoard>
{
    public DashBoardRepository(ApplicationDbContext context) : base(context)
    {
        ;
    }

    public List<DashBoard> GetUserDashBoards(User user)
    {
        return _db.DashBoards.Where(d => d.Users.Contains(user)).ToList();
    }

    public async Task<EntityEntry<DashBoard>> Create(DashBoard model)
    {
        model.Users.Add(model.Creator);
        return await base.Create(model);
    }
    

}