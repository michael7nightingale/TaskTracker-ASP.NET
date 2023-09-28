using Microsoft.EntityFrameworkCore;
using TaskTracker.Models;
using TaskTracker.Repositories.Base;
using TaskTracker.ViewModels.Invitation;

namespace TaskTracker.Repositories;

public class InvitationRepository : BaseRepository<Invitation>
{
    public InvitationRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<InvitationListModel> GetListSeparative(User user)
    {
        return new InvitationListModel
        {
            IncomingInvitations = await Entity
                .Where(i => i.Invited == user)
                .Select(d => new Invitation
                {
                    Id = d.Id,
                    ToDashBoardId = d.ToDashBoardId,
                    InviterId = d.InviterId,
                    InvitedId = d.InvitedId,
                    ToDashBoard = _db.DashBoards.First(dash => dash.Id == d.ToDashBoardId),
                    Invited = _db.Users.First(u => u.Id == d.InvitedId),
                    Inviter = _db.Users.First(u => u.Id == d.InviterId)
                    
                })
                .ToListAsync(),
            OutcomingInvitations = await Entity
                .Where(i => i.Inviter == user)
                .Select(d => new Invitation
                {
                    Id = d.Id,
                    ToDashBoardId = d.ToDashBoardId,
                    InviterId = d.InviterId,
                    InvitedId = d.InvitedId,
                    Invited = _db.Users.First(u => u.Id == d.InvitedId),
                    Inviter = _db.Users.First(u => u.Id == d.InviterId)
                    
                })
                .ToListAsync(),
        };
    }

    public async Task<Invitation?> Get(string id)
    {
        return await Entity
            .Select(i => new Invitation
            {
                Id = i.Id,
                ToDashBoardId = i.ToDashBoardId,
                ToDashBoard = _db.DashBoards.First(d => d.Id == i.ToDashBoardId),
                InviterId = i.InviterId,
                InvitedId = i.InvitedId,
                Invited = _db.Users.First(u => u.Id == i.InvitedId),
                Inviter = _db.Users.First(u => u.Id == i.InviterId),
            })
            .FirstAsync(i => i.Id == id);
    }

    public User Accept(Invitation model)
    { 
        model.ToDashBoard.Users.Add(model.Invited); 
        Delete(model.Id);
        return model.Invited;
    }
    
}

