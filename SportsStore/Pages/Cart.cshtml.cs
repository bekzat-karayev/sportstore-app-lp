using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportsStore.infrastructure;
using SportsStore.Models;

namespace SportsStore.Pages;

/*  The class associated with a Razor Page is known as its page model class, and it defines handler methods
that are invoked for different types of HTTP requests, which update state before rendering the view. 
    Specifically, this page model class defines an OnPost hander method, which is invoked to handle HTTP POST requests. 
It does this by retrieving a Product from the database, retrieving the user’s Cart from the session data, and updating 
its content using the Product. The modified Cart is stored, and the browser is redirected to the same Razor Page, 
which it will do using a GET request (which prevents reloading the browser from triggering a duplicate POST request).
    The GET request is handled by the OnGet handler method, which sets the values of the ReturnUrl and
Cart properties, after which the Razor content section of the page is rendered. The expressions in the HTML
content are evaluated using the CartModel as the view model object, which means that the values assigned
to the ReturnUrl and Cart properties can be accessed within the expressions. The content generated by the
Razor Page details the products added to the user’s cart and provides a button to navigate back to the point
where the product was added to the cart.
    The handler methods use parameter names that match the input elements in the HTML forms produced by the 
ProductSummary.cshtml view. This allows ASP.NET Core to associate incoming form POST variables with those parameters, 
meaning I do not need to process the form directly. 
    This is known as model binding and is a powerful tool for simplifying development.
*/
public class CartModel : PageModel
{
    private IStoreRepository repository;

    public CartModel(IStoreRepository storeRepository)
    {
        repository = storeRepository;
    }

    public Cart? Cart { get; set; }
    public string ReturnUrl { get; set; } = "/";

    public void OnGet(string returnUrl)
    {
        ReturnUrl = returnUrl ?? "/";
        Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new();
    }

    public IActionResult OnPost(long productId, string returnUrl)
    {
        Product? product = repository.Products.FirstOrDefault(p => p.ProductID == productId);
        Console.WriteLine(product?.Name);

        if (product != null)
        {
            Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
            Cart.AddItem(product, 1);
            HttpContext.Session.SetJson("cart", Cart);
        }

        return RedirectToPage(new { returnUrl = returnUrl});
    }
}
