using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TaskTracker.Models;
using TaskTracker.Repositories.Base;

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
    
}