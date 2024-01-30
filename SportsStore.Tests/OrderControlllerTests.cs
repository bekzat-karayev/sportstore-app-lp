using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;

namespace SportsStore.Tests;

public class OrderControllerTests
{
    /*  This test ensures that I cannot check out with an empty cart. I check this by ensuring that the SaveOrder of the mock 
    IOrderRepository implementation is never called, that the view the method returns is the default view 
    (which will redisplay the data entered by customers and give them a chance to correct it), and that the model state 
    being passed to the view has been marked as invalid.  
        This may seem like a belt-and-braces set of assertions, but I need all three to be sure that I have the right behavior.
*/
    [Fact]
    public void Cannot_Checkout_Empty_Cart()
    {
        // Arrange

        Mock<IOrderRepository> mock = new();
        Cart cart = new();
        Order order = new();
        OrderController target = new(mock.Object, cart);

        // Act

        ViewResult? result = target.CheckOut(order) as ViewResult;

        // Assert

        // check that the order hasn't been stored
        mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);

        // check that the method is returning the default view
        Assert.True(string.IsNullOrEmpty(result?.ViewName));

        // check that I am passing an invalid model to the view
        Assert.False(result?.ViewData.ModelState.IsValid);
    }

    [Fact]
    public void Cannot_Checkout_Invalid_ShippingDetails()
    {
        // Arrange

        Mock<IOrderRepository> mock = new();
        Cart cart = new();
        cart.AddItem(new Product(), 1);
        OrderController target = new(mock.Object, cart);
        // add an error to the model
        target.ModelState.AddModelError("error", "error");

        // Act

        ViewResult? result = target.CheckOut(new Order()) as ViewResult;

        // Assert

        // check that the order hasn't been passed stored
        mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);

        // check that the method is returning the default view
        Assert.True(string.IsNullOrEmpty(result?.ViewName));

        // check that I am passing an invalid model to the view
        Assert.False(result?.ViewData.ModelState.IsValid);
    }

    /*  I need to point out here that I do not need to test that I can identify valid shipping details. 
    This is handled for me automatically by the model binder using the attributes applied to the properties of the Order class.
    */
    [Fact]
    public void Can_Checkout_And_Submit_Order()
    {
        // Arrange

        Mock<IOrderRepository> mock = new();
        Cart cart = new();
        cart.AddItem(new Product(), 1);
        OrderController target = new(mock.Object, cart);

        // Act

        RedirectToPageResult? result = target.CheckOut(new Order()) as RedirectToPageResult;

        // Assert

        // check that order has been stored
        mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);

        // check that the method is redirecting to the `Completed` action
        Assert.Equal("/Completed", result?.PageName);
    }
}
