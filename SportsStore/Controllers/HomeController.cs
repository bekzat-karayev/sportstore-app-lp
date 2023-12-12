using Microsoft.AspNetCore.Mvc;


namespace SportsStore.Controllers 
{
    /*  The MapDefaultControllerRoute method in Program.cs tells ASP.NET Core how to match URLs to
    controller classes. The configuration applied by that method declares that the Index action method defined
    by the Home controller will be used to handle requests.
        Returning the result of calling the View method tells ASP.NET Core to render the default view associated 
    with the action method.
    */
    public class HomeController: Controller 
    {
        public IActionResult Index() => View();
    }
}