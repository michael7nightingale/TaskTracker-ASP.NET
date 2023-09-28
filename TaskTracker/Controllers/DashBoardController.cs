using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Scaffolding;
using TaskTracker.Models;
using TaskTracker.Repositories;
using TaskTracker.ViewModels.DashBoard;
using static TaskTracker.Service.HttpContextUserService;
using Task = TaskTracker.Models.Task;


namespace TaskTracker.Controllers;

[Authorize]
public class DashBoardController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly DashBoardRepository _dashBoardRepository;
    private readonly TaskRepository _taskRepository;
    private readonly InvitationRepository _invitationRepository;
    private readonly UserManager<User> _userManager;
    
    public DashBoardController(ApplicationDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
        _dashBoardRepository = new DashBoardRepository(_context);
        _taskRepository = new TaskRepository(_context);
        _invitationRepository = new InvitationRepository(_context);
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var dashBoards = _dashBoardRepository.GetUserDashBoards(
            await GetHttpContextUser(_userManager, HttpContext)
            );
        return View(dashBoards);
    }

    [HttpGet]
    public async Task<IActionResult> Detail(string id)
    {
        var model = await _dashBoardRepository.Get(id);    
        if (model is null)
        {
            return NotFound();
        }

        // model.Tasks = await _taskRepository.GetTasksByDashBoard(id);
        return View(model);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DashBoardCreateModel model)
    {
       await _dashBoardRepository.Create(new DashBoard
        {
            Title = model.Title,
            Creator = await GetHttpContextUser(_userManager, HttpContext)
        });
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> CreateTask(string id)
    {
        var dashBoard = await _dashBoardRepository.Get(id);
        if (dashBoard is null) return NotFound();
        var model = new TaskCreateInfoModel { DashBoard = dashBoard, Users = dashBoard.Users };
        return View(model);
    }
    

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateTask(string id, TaskCreateModel model)
    {
        DashBoard? dashBoard = await _dashBoardRepository.Get(id);
        if (dashBoard is null) return NotFound();
        
        var instance = await _taskRepository.Create(new Task
        {
            Title = model.Title,
            ToUsers = model.ToUsers,
            Creator = await GetHttpContextUser(_userManager, HttpContext),
            DashBoardId = id,
            DashBoard = await _dashBoardRepository.Get(id)
            
        });
        return RedirectToAction(nameof(Detail), "DashBoard", new {id=dashBoard.Id} );
    }

    [HttpPost]
    public async Task<IActionResult> InviteUser(string id, InviteUserModel model)
    {
        DashBoard? dashBoard = await _dashBoardRepository.Get(id);
        if (dashBoard is null) return NotFound();
        var currentUser = await GetHttpContextUser(_userManager, HttpContext);
        var invitedUser = await _context.Users.Where(u => u.Email == model.Email).FirstOrDefaultAsync();
         if (!dashBoard.Users.Contains(currentUser)) return BadRequest();
        if (invitedUser is not null)
        {  try
                 {
                     var instance = await _invitationRepository.Create(
                         new Invitation
                         {
                             Invited = invitedUser,
                             Inviter = currentUser,
                             ToDashBoard = dashBoard
                                 
                         }
                     );
                 }
                 catch 
                 {
                 }
            
        }
        return RedirectToAction("Detail", "DashBoard", new {id = id});
    }
    

    // [HttpGet]
    // public async Task<IActionResult> Edit(string? id)
    // {
    //     if (id == null || _context.BaseModel == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     var model = await _dashBoardRepository.Get(id);
    //     if (model == null)
    //     {
    //         return NotFound();
    //     }
    //     return View(model);
    // }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("Id")]DashBoardEditModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var instance = _context.Update(new DashBoard {Title = model.Title});
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }
        return View(await _dashBoardRepository.Get(id));
    }

    // public async Task<IActionResult> Delete(string? id)
    // {
    //     if (id is null || _context.BaseModel == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     var baseModel = await _context.BaseModel
    //         .FirstOrDefaultAsync(m => m.Id == id);
    //     if (baseModel == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     return View(baseModel);
    // }

    // [HttpPost, ActionName("Delete")]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> DeleteConfirmed(string id)
    // {
    //     if (_context.BaseModel == null)
    //     {
    //         return Problem("Entity set 'ApplicationDbContext.BaseModel'  is null.");
    //     }
    //     var baseModel = await _context.BaseModel.FindAsync(id);
    //     if (baseModel != null)
    //     {
    //         _context.BaseModel.Remove(baseModel);
    //     }
    //     
    //     await _context.SaveChangesAsync();
    //     return RedirectToAction(nameof(Index));
    // }

    // private bool BaseModelExists(string id)
    // {
    //   return (_context.BaseModel?.Any(e => e.Id == id)).GetValueOrDefault();
    // }
}
