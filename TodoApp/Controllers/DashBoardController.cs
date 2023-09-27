using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using TodoApp.Models;
using TodoApp.Repositories;
using TodoApp.ViewModels.DashBoard;
using Task = TodoApp.Models.Task;


namespace TodoApp.Controllers;

[Authorize]
public class DashBoardController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly DashBoardRepository _dashBoardRepository;
    private readonly TaskRepository _taskRepository;
    private readonly UserManager<User> _userManager;
    
    public DashBoardController(ApplicationDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
        _dashBoardRepository = new DashBoardRepository(_context);
        _taskRepository = new TaskRepository(_context);
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var dashBoards = _dashBoardRepository.GetUserDashBoards(
            await _userManager.GetUserAsync(HttpContext.User)
            );
        return View(dashBoards);
    }

    [HttpGet]
    public async Task<IActionResult> Detail(string? id)
    {
        if (id == null || _context.BaseModel == null)
        {
            return NotFound();
        }

        var model = await _dashBoardRepository.Get(id);    
        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DashBoardCreateModel model)
    {
        if (ModelState.IsValid)
        {
            _dashBoardRepository.Create(new DashBoard
            {
                Title = model.Title,
                Creator = await _userManager.GetUserAsync(HttpContext.User)
            });
            return RedirectToAction(nameof(Index));
        }
        return View(model);
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
        
        _taskRepository.Create(new Task
        {
            Title = model.Title,
            ToUsers = model.ToUsers,
            Creator = await _userManager.GetUserAsync(HttpContext.User),
            DashBoard = await _dashBoardRepository.Get(id)
            
        });
        return RedirectToAction(nameof(Detail), "DashBoard", new {id=dashBoard.Id} );
    }


    [HttpGet]
    public async Task<IActionResult> Edit(string? id)
    {
        if (id == null || _context.BaseModel == null)
        {
            return NotFound();
        }

        var model = await _dashBoardRepository.Get(id);
        if (model == null)
        {
            return NotFound();
        }
        return View(model);
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

    public async Task<IActionResult> Delete(string? id)
    {
        if (id is null || _context.BaseModel == null)
        {
            return NotFound();
        }

        var baseModel = await _context.BaseModel
            .FirstOrDefaultAsync(m => m.Id == id);
        if (baseModel == null)
        {
            return NotFound();
        }

        return View(baseModel);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        if (_context.BaseModel == null)
        {
            return Problem("Entity set 'ApplicationDbContext.BaseModel'  is null.");
        }
        var baseModel = await _context.BaseModel.FindAsync(id);
        if (baseModel != null)
        {
            _context.BaseModel.Remove(baseModel);
        }
        
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool BaseModelExists(string id)
    {
      return (_context.BaseModel?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
