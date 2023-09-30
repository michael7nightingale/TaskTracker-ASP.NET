using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    [Route("/dashboards")]
    public async Task<IActionResult> Index()
    {
        var dashBoards = _dashBoardRepository.GetUserDashBoards(
            await GetHttpContextUser(_userManager, HttpContext)
            );
        ViewBag.CurrentUser = await GetHttpContextUser(_userManager, HttpContext);
        return View(dashBoards);
    }

    [HttpGet]
    [Route("/dashboards/{id}")]
    public async Task<IActionResult> Detail(string id)
    {
        var model = await _dashBoardRepository.Get(id);    
        if (model is null)
        {
            return NotFound();
        }

        var currentUser = await GetHttpContextUser(_userManager, HttpContext);
        ViewBag.CurrentUser = currentUser;
        return View(model);
    }

    [HttpGet]
    [Route("/dashboards/create")]
    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/dashboards/create")]
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
    [Route("/dashboards/create-task/{id}")]
    public async Task<IActionResult> CreateTask(string id)
    {
        var dashBoard = await _dashBoardRepository.Get(id);
        if (dashBoard is null) return NotFound();
        var model = new TaskCreateInfoModel { DashBoard = dashBoard, Users = dashBoard.Users };
        return View(model);
    }
    

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/dashboards/create-task/{id}")]
    public async Task<IActionResult> CreateTask(string id, TaskCreateModel model)
    {
        DashBoard? dashBoard = await _dashBoardRepository.Get(id);
        if (dashBoard is null) return NotFound();
        
        var instance = await _taskRepository.Create(new Task
        {
            Title = model.Title,
            ToUsers = model.ToUsers,
            Description = model.Description,
            CreatorId = (await GetHttpContextUser(_userManager, HttpContext)).Id,
            DashBoardId = id,
        });
        return RedirectToAction(nameof(Detail), "DashBoard", new {id=dashBoard.Id} );
    }

    [HttpPost]
    [Route("/dashboards/invite-user/{id}")]
    public async Task<IActionResult> InviteUser(string id, InviteUserModel model)
    {
        DashBoard? dashBoard = await _dashBoardRepository.Get(id);
        if (dashBoard is null) return NotFound();
        var currentUser = await GetHttpContextUser(_userManager, HttpContext);
        var invitedUser = await _context.Users.Where(u => u.Email == model.Email).FirstOrDefaultAsync();
         if (!dashBoard.Users.Contains(currentUser)) return BadRequest();
         if (dashBoard.Users.Contains(invitedUser)) return BadRequest();
        if (invitedUser is not null)
        { 
                 {
                     var instance = await _invitationRepository.Create(
                         new Invitation
                         {
                             InvitedId = invitedUser.Id,
                             InviterId = currentUser.Id,
                             ToDashBoardId = dashBoard.Id
                                 
                         }
                     );
                 }
            
        }
        return RedirectToAction("Detail", "DashBoard", new {id = id});
    }


    [HttpPost]
    [Route("/dashboards/finish-task/{id}/{taskId}")]
    public async Task<IActionResult> FinishTask(string id, string taskId)
    {
        DashBoard? dashBoard = await _dashBoardRepository.Get(id);
        Console.WriteLine($"dashboard id {dashBoard}");
        if (dashBoard is null) return NotFound();
        Models.Task? task = await _taskRepository.GetByDashBoardId(taskId, id);
        Console.WriteLine($"task id {task}");
        if (task is null) return NotFound();
        var currentUser = await GetHttpContextUser(_userManager, HttpContext);
        if (!dashBoard.Users.Contains(currentUser)) return Forbid();
        _taskRepository.FinishTask(ref task);
        return RedirectToAction("Detail", "DashBoard", new { id = dashBoard.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        DashBoard? dashBoard = await _dashBoardRepository.Get(id);
        if (dashBoard is null) return NotFound();
        var currentUser = await GetHttpContextUser(_userManager, HttpContext);
        if (currentUser != dashBoard.Creator) return BadRequest();
        return View(dashBoard);
    }

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

    [HttpGet]
    [Route("/dashboards/delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        DashBoard? dashBoard = await _dashBoardRepository.Get(id);
        if (dashBoard is null) return NotFound();
        var currentUser = await GetHttpContextUser(_userManager, HttpContext);
        if (currentUser != dashBoard.Creator) return BadRequest();
        return View(dashBoard);
    }

    [HttpPost, ActionName("Delete")]
    [Route("/dashboards/delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        DashBoard? dashBoard = await _dashBoardRepository.Get(id);
        if (dashBoard is null) return NotFound();
        var currentUser = await GetHttpContextUser(_userManager, HttpContext);
        if (currentUser != dashBoard.Creator) return BadRequest();
        _dashBoardRepository.Delete(dashBoard);
        return RedirectToAction(nameof(Index));
    }

}
