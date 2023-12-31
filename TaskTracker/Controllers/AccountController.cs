using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using TaskTracker.Models;

using Microsoft.AspNetCore.Mvc;
using TaskTracker.ViewModels;

namespace TaskTracker.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{
    private ApplicationDbContext _db;

    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AccountController(ApplicationDbContext context, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _db = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }


    [HttpGet]
    [Route("/account/login")]
    public IActionResult Login() => View();

    [HttpPost]
    [Route("/account/login")]
    public async Task<IActionResult> Login(UserLoginModel userLoginModelData)
    {
        var user = await _signInManager.PasswordSignInAsync(userLoginModelData.Email, userLoginModelData.Password, true, false);
        if (user.Succeeded)
        {
            return RedirectToAction("Index", "Home"); 
        }

        return View();
    }
    
    
    [HttpGet]
    [Route("/account/register")]
    public IActionResult Register() => View();


    [HttpPost]
    [Route("/account/register")]
    public async Task<IActionResult> Register(UserRegisterModel userRegisterData)
    {
        if (userRegisterData.ConfirmPassword != userRegisterData.Password)
        {
            return RedirectToAction("Register");
        }
        var user = new User { Email = userRegisterData.Email, UserName = userRegisterData.Email};
        var result = await _userManager.CreateAsync(user, userRegisterData.Password);
        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [HttpPost]
    [Authorize]
    [Route("/account/logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
    
}
