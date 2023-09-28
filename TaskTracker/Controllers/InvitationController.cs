using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Models;
using TaskTracker.Repositories;
using static TaskTracker.Service.HttpContextUserService;

namespace TaskTracker.Controllers;


[Authorize]
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

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var currentUser = await GetHttpContextUser(_userManager, HttpContext);
        var model = await _invitationRepository.GetListSeparative(currentUser);
        return View(model);
        
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        var invitation = await _invitationRepository.Get(id);
        if (invitation is null) return NotFound();
        var currentUser = await GetHttpContextUser(_userManager, HttpContext);
        if (!(invitation.Invited == currentUser || invitation.Inviter == currentUser))
        {
            return BadRequest();
        }
        _invitationRepository.Delete(id);
        
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Accept(string id)
    {
        return RedirectToAction("Index");
    }
    
}
