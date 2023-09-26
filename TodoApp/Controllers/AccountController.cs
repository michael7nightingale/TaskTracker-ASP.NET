using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using TodoApp.Models;

using Microsoft.AspNetCore.Mvc;
using TodoApp.Data;
using TodoApp.ViewModels;

namespace TodoApp.Controllers;

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
    [AllowAnonymous]
    public IActionResult Login() => View();

    [HttpPost]
    [AllowAnonymous]
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
    [AllowAnonymous]
    public IActionResult Register() => View();


    [HttpPost]
    [AllowAnonymous]
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
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
    
}
