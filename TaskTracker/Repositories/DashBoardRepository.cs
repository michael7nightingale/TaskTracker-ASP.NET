using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TaskTracker.Models;
using TaskTracker.Repositories.Base;

namespace TaskTracker.Repositories;

public class DashBoardRepository: BaseRepository<DashBoard>
{
    public DashBoardRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async new Task<DashBoard?> Get(string id)
    {
        return await Entity
            .Include(d => d.Tasks)
            .Include(d => d.Users)
            .Select(d => new DashBoard
                {
                    Id = d.Id,
                    Title = d.Title,
                    Creator = _db.Users.First(u => u.Id == d.CreatorId),
                    CreatorId = d.CreatorId,
                    Users = d.Users,
                    Tasks = d.Tasks,
                    
                }
            )
            .Where(d => d.Id == id).FirstOrDefaultAsync();
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