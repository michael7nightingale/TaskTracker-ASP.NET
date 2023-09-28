using Microsoft.AspNetCore.Identity;
using TaskTracker.Models;

namespace TaskTracker.Service;

public static class HttpContextUserService
{
    public static async Task<User> GetHttpContextUser(UserManager<User> userManager, HttpContext httpContext)
    {
        return await userManager.GetUserAsync(httpContext.User);
    }
}