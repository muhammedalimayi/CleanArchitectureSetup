using Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace WebAPI.Middlewares;
public static class ExtensionsMiddleware
{
    public static void CreateFirstUser(WebApplication app)
    {
        using (var scoped = app.Services.CreateScope())
        {
            var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            if (!userManager.Users.Any(p => p.UserName == "admin"))
            {
                AppUser user = new()
                { 
                    UserName = "admin",
                    Email = "admin@admin.com",
                    FirstName = "Muhammed Ali",
                    LastName = "MAYI",
                    EmailConfirmed = true,
                    CreateAt = DateTime.Now,
                };

                user.CreateUserId = user.Id;

                userManager.CreateAsync(user, "1234").Wait();


            }
        }
    }
}