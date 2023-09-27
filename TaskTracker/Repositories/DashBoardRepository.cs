using TodoApp.Models;
using TodoApp.Repositories.Base;

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

    public async void Create(DashBoard model)
    {
        model.Users.Add(model.Creator);
        base.Create(model);
    }
    

}