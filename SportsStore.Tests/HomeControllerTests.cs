using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;

namespace SportsStore.Tests;

public class HomeControllerTests
{
    [Fact]
    public void Can_Use_Repository()
    {
        // Arrange

        Mock<IStoreRepository> mock = new();
        mock.Setup(m => m.Products).Returns(
            (new Product[] {
                new Product { ProductID = 1, Name = "P1"},
                new Product { ProductID = 2, Name = "P2"}
            }).AsQueryable<Product>());

        HomeController controller = new(mock.Object);

        // Act

        /*  It is a little awkward to get the data returned from the action method. The result is a ViewResult
        object, and I have to cast the value of its ViewData.Model property to the expected data type. 
        */
        IEnumerable<Product>? result = (controller.Index() as ViewResult)?.ViewData.Model as IEnumerable<Product>;

        // Assert

        Product[] productArray = result?.ToArray() ?? Array.Empty<Product>();

        Assert.True(productArray.Length == 2);
        Assert.Equal("P1", productArray[0].Name);
        Assert.Equal("P2", productArray[1].Name);
    }
}