using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers 
{
    /*  The MapDefaultControllerRoute method in Program.cs tells ASP.NET Core how to match URLs to
    controller classes. The configuration applied by that method declares that the Index action method defined
    by the Home controller will be used to handle requests.
        Returning the result of calling the View method tells ASP.NET Core to render the default view associated 
    with the action method.
    */
    /*  When ASP.NET Core needs to create a new instance of the HomeController class to handle an HTTP request, 
    it will inspect the constructor and see that it requires an object that implements the IStoreRepository interface. 
    To determine what implementation class should be used, ASP.NET Core consults the configuration created in the 
    Program.cs file, which tells it that EFStoreRepository should be used and that a new instance should be created 
    for every request (scoped). ASP.NET Core creates a new EFStoreRepository object and uses it to invoke the 
    HomeController constructor to create the controller object that will process the HTTP request.
        This is known as dependency injection, and its approach allows the HomeController object to access the
    application’s repository through the IStoreRepository interface without knowing which implementation
    class has been configured. I could reconfigure the service to use a different implementation class—one that
    doesn’t use Entity Framework Core, for example—and dependency injection means that the controller will
    continue to work without changes.
    */
    public class HomeController : Controller 
    {
        private IStoreRepository repository;
        /*  Support for pagination is added, so that the view displays a smaller amount of products on a page
        and so the user can move from page to page to view the overall catalog.  
            The PageSize field specifies that I want four products per page. Index action method now passes 
        a ProductsListViewModel object as the model data to the view.
        */
        public int PageSize = 4;

        public HomeController(IStoreRepository storeRepository)
        {
            repository = storeRepository;
        }

        /*  When null is used for the `category` argument, all the Product objects that the controller gets
        from the repository are returned, which is the same situation I had before adding the new `category` parameter.  
        */
        public ViewResult Index(string? category, int productPage = 1)
        {
            return View(new ProductListViewModel() {
                Products = repository.Products
                    .Where(p => category == null || p.Category == category)
                    .OrderBy(p => p.ProductID)
                    .Skip((productPage - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new(){
                    TotalItems = repository.Products.Count(),
                    ItemsPerPage = PageSize,
                    CurrentPage = productPage
                },
                CurrentCategory = category
            });
        }
    }
}