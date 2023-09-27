using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Models;
using TaskTracker.Repositories;

namespace TaskTracker.Controllers;

public class InvitationController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly InvitationRepository _invitationRepository;
    private readonly UserManager<User> _userManager;
    
    public InvitationController(ApplicationDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
        _invitationRepository = new InvitationRepository(_context);
    }
    
        
}
