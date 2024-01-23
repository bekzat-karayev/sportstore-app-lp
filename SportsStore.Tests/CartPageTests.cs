using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Models;
using SportsStore.Pages;

namespace SportsStore.Tests;

/*  The complexity in this test is mocking the ISession interface so that the page model class can use extension 
methods to retrieve a JSON representation of a Cart object. 
    The ISession interface only stores byte arrays, and getting and deserializing a string is performed by 
extension methods. Once the mock objects are defined, they can be wrapped in context objects and used to configure 
an instance of the page model class, which can be subjected to tests.
    The process of testing the OnPost method of the page model class means capturing the byte array
that is passed to the ISession interface mock and then deserializing it to ensure that it contains the
expected content.
*/
public class CartPageTests
{
    [Fact]
    public void Can_Load_Cart()
    {
        // Arrange

        Product p1 = new() { ProductID = 1, Name = "P1"};
        Product p2 = new() { ProductID = 2, Name = "P2"};
        Mock<IStoreRepository> mockRepository = new();
        mockRepository.Setup(m => m.Products).Returns((new Product[] { p1, p2}).AsQueryable<Product>());

        Cart testCart = new();
        testCart.AddItem(p1, 2);
        testCart.AddItem(p2, 1);

        Mock<ISession> mockSession = new();
        byte[]? data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(testCart));
        mockSession.Setup(c => c.TryGetValue(It.IsAny<string>(), out data));

        Mock<HttpContext> mockContext = new();
        mockContext.SetupGet(c => c.Session).Returns(mockSession.Object);

        // Act

        CartModel cartModel = new(mockRepository.Object) {
            PageContext = new(new ActionContext {
                HttpContext = mockContext.Object,
                RouteData = new(),
                ActionDescriptor = new()
            }) 
        };
        cartModel.OnGet("myUrl");

        // Assert

        Assert.Equal(2, cartModel.Cart?.Lines.Count);
        Assert.Equal("myUrl", cartModel.ReturnUrl);
    }

    [Fact]
    public void Can_Update_Cart()
    {
        // Arrange

        Product p1 = new() { ProductID = 1, Name = "P1"};
        Mock<IStoreRepository> mockRepository = new();
        mockRepository.Setup(m => m.Products).Returns((new Product[] { p1 }).AsQueryable<Product>());

        Cart? testCart = new();

        Mock<ISession> mockSession = new();
        mockSession.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
            .Callback<string, byte[]>((key, val) => {
                testCart = JsonSerializer.Deserialize<Cart>(Encoding.UTF8.GetString(val));
            });


        Mock<HttpContext> mockContext = new();
        mockContext.SetupGet(c => c.Session).Returns(mockSession.Object);

        // Act

        CartModel cartModel = new(mockRepository.Object) {
            PageContext = new(new ActionContext {
                HttpContext = mockContext.Object,
                RouteData = new(),
                ActionDescriptor = new()
            })
        };
        cartModel.OnPost(1, "myUrl");

        // Assert

        Assert.Single(testCart.Lines);
        Assert.Equal("P1", testCart.Lines.First().Product.Name);
        Assert.Equal(1, testCart.Lines.First().Quantity);
    }
}