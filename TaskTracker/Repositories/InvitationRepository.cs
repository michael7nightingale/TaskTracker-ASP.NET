using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TaskTracker.Models;
using TaskTracker.Repositories.Base;
using TaskTracker.ViewModels.Invitation;
using Task = System.Threading.Tasks.Task;

namespace TaskTracker.Repositories;

public class InvitationRepository : BaseRepository<Invitation>
{
    public InvitationRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public new async Task<EntityEntry<Invitation>> Create(Invitation model)
    {
        if (model.ToDashBoard.Users.Contains(model.Invited)) throw new ValidationException();
        return await base.Create(model);
    }

    public async Task<InvitationListModel> GetListSeparative(User user)
    {
        return new InvitationListModel
        {
            IncomingInvitations = await Entity.Where(i => i.Invited == user).ToListAsync(),
            OutcomingInvitations = await Entity.Where(i => i.Inviter == user).ToListAsync()
        };
    }
    
}

