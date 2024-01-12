using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Components;

/*  I could build the HTML for the categories programmatically, as I did for the page tag helper, but one of the 
benefits of working with view components is they can render Razor partial views. That means I can use the view component to
generate the list of components and then use the more expressive Razor syntax to render the HTML that will display them.
*/
public class NavigationMenuViewComponent : ViewComponent
{
    private IStoreRepository repository;

    public NavigationMenuViewComponent(IStoreRepository storeRepository)
    {
        repository = storeRepository;
    }
    /*  The view component’s Invoke method is called when the component is used in a Razor view, and the
    result of the Invoke method is inserted into the HTML sent to the browser.  
        I use the RouteData property to access the request data to get the value for the currently
    selected category. I could pass the category to the view by creating another view model class (and that’s
    what I would do in a real project), but for variety, I am going to use the view bag feature, which allows
    unstructured data to be passed to a view alongside the view model object.
        The ViewBag is a dynamic object that allows me to define new properties simply by assigning values to them.
    */
    public IViewComponentResult Invoke()
    {
        ViewBag.SelectedCategory = RouteData?.Values["category"];
        return View(repository.Products.Select(x => x.Category).Distinct().OrderBy(x => x));
    }
}