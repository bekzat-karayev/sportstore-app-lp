using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers;

public class OrderController : Controller
{
    private IOrderRepository repository;
    private Cart cart;

    public OrderController(IOrderRepository repositoryService, Cart cartService)
    {
        repository = repositoryService;
        cart = cartService;
    }

    public ViewResult Checkout() => View(new Order());

    /*  The `Checkout` action method is decorated with the HttpPost attribute, which means that it will be used
    to handle POST requests—in this case, when the user submits the form.
        When a request is processed, the model binding system tries to find values for the properties defined by the Order
    class. This works on a best-effort basis, which means I may receive an Order object lacking property values if
    there is no corresponding data item in the request.
        To ensure I have the data I require, I applied validation attributes to the Order class. ASP.NET Core checks the 
    validation constraints that I applied to the Order class and provides details of the result through the ModelState property. 
    I can see whether there are any problems by checking the ModelState.IsValid property. 
        I call the `ModelState.AddModelError` method to register an error message if there are no items in the cart.
    */
    [HttpPost]
    public IActionResult CheckOut(Order order)
    {
        if (cart.Lines.Count() == 0)
        {
            ModelState.AddModelError("", "Sorry, your cart is empty!");
        }

        if (ModelState.IsValid)
        {
            order.Lines = cart.Lines.ToArray();
            repository.SaveOrder(order);
            cart.Clear();

            return RedirectToPage("/Completed", new { orderId = order.OrderID });
        }
        else
        {
            return View();
        }
    }
}
