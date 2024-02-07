using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SportsStore.Models;

public static class IdentitySeedData
{
    public const string adminUser = "Admin";
    public const string adminPassword = "Secret123$";

    /*  This code ensures the database is created and up-to-date and uses the UserManager<T> class, which is provided as 
    a service by ASP.NET Core Identity for managing users. The database is searched for the Admin user account, 
    which is created with a password of `Secret123$` if it is not present.
        Hard-coding the details of an administrator account is often required so that you can log into an application once 
    it has been deployed and start administering it. When you do this, you must remember to change the password for the account 
    you have created.
    */
    public static async void EnsurePopulated(IApplicationBuilder app)
    {
        AppIdentityDbContext context = app.ApplicationServices
            .CreateScope().ServiceProvider.GetRequiredService<AppIdentityDbContext>();
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }

        UserManager<IdentityUser> userManager = app.ApplicationServices
            .CreateScope().ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        
        IdentityUser? user = await userManager.FindByNameAsync(adminUser);

        if (user == null)
        {
            user = new("Admin");
            user.Email = "admin@example.com";
            user.PhoneNumber = "555-1234";
            await userManager.CreateAsync(user, adminPassword);
        }
    }
}