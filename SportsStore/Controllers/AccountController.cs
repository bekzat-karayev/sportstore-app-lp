using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers;

/*  When the user is redirected to the /Account/Login URL, the GET version of the Login action method renders the default view for the page, 
providing a view model object that includes the URL to which the browser should be redirected if the authentication request is successful.
    Authentication credentials are submitted to the POST version of the Login method, which uses the UserManager<IdentityUser> and 
SignInManager<IdentityUser> services that have been received through the controller’s constructor to authenticate the user and log them into the system. 
    For now, it is enough to know that if there is an authentication failure, then I create a model validation error and render the default view; 
however, if authentication is successful, then I redirect the user to the URL that they want to access before they are prompted for their credentials.

*/
public class AccountControleer : Controller
{
    private UserManager<IdentityUser> userManager;
    private SignInManager<IdentityUser> signInManager;

    public AccountControleer(UserManager<IdentityUser> userMgr, SignInManager<IdentityUser> signInMgr)
    {
        userManager = userMgr;
        signInManager = signInMgr;
    }

    public ViewResult Login(string returnUrl)
    {
        return View(new LoginModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        if (ModelState.IsValid)
        {
            // to disable Intellisense warnings on null reference
            #pragma warning disable CS8604 // Possible null reference argument.
            IdentityUser? user = await userManager.FindByNameAsync(loginModel.Name);

            if (user != null)
            {
                await signInManager.SignOutAsync();
                if ((await signInManager.PasswordSignInAsync(user, loginModel.Password, false, false)).Succeeded)
                {
                    return Redirect(loginModel?.ReturnUrl ?? "/Admin");
                }
            }

            ModelState.AddModelError("", "Invalid name or password");
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        return View(loginModel);
    }

    [Authorize]
    public async Task<RedirectResult> Logout(string returnUrl = "/")
    {
        await signInManager.SignOutAsync();

        return Redirect(returnUrl);
    }
}
