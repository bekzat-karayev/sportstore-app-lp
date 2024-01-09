using Microsoft.AspNetCore.Mvc;

namespace SportsStore.Components;

public class NavigationMenuViewComponent : ViewComponent
{
    /*  The view component’s Invoke method is called when the component is used in a Razor view, and the
    result of the Invoke method is inserted into the HTML sent to the browser.  
    */
    public string Invoke()
    {
        return "Hello mazafaka, i am view componentn";
    }
}