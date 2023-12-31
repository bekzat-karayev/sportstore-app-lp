using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

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
        ProductListViewModel result = controller.Index(null).ViewData.Model as ProductListViewModel ?? new();

        // Assert

        Product[] productArray = result.Products.ToArray() ?? Array.Empty<Product>();

        Assert.True(productArray.Length == 2);
        Assert.Equal("P1", productArray[0].Name);
        Assert.Equal("P2", productArray[1].Name);
    }

    [Fact]
    public void Can_Paginate()
    {
        // Arrange

        Mock<IStoreRepository> mock = new();
        mock.Setup(m => m.Products).Returns(
            (new Product[] {
                new Product { ProductID = 1, Name = "P1"},
                new Product { ProductID = 2, Name = "P2"},
                new Product { ProductID = 3, Name = "P3"},
                new Product { ProductID = 4, Name = "P4"},
                new Product { ProductID = 5, Name = "P5"}
            }).AsQueryable());

        HomeController controller = new(mock.Object);

        controller.PageSize = 3;

        // Act

        ProductListViewModel result = controller.Index(null, 2)?.ViewData.Model as ProductListViewModel ?? new(); 

        // Assert

        Product[] productArray = result.Products.ToArray();
        Assert.True(productArray.Length == 2);
        Assert.Equal("P4", productArray[0].Name);
        Assert.Equal("P5", productArray[1].Name);
    }

    [Fact]
    public void Can_Send_Pagination_View_Model()
    {
        // Arrange

        Mock<IStoreRepository> mock = new();
        mock.Setup(m => m.Products).Returns(
            (new Product[] {
                new Product { ProductID = 1, Name = "P1"},
                new Product { ProductID = 2, Name = "P2"},
                new Product { ProductID = 3, Name = "P3"},
                new Product { ProductID = 4, Name = "P4"},
                new Product { ProductID = 5, Name = "P5"}
            }).AsQueryable());
        
        HomeController controller = new(mock.Object) { PageSize = 3 };

        // Act

        ProductListViewModel result = controller.Index(null, 2)?.ViewData.Model as ProductListViewModel ?? new();

        // Assert

        PagingInfo pageInfo = result.PagingInfo;
        Assert.Equal(2, pageInfo.CurrentPage);
        Assert.Equal(3, pageInfo.ItemsPerPage);
        Assert.Equal(5, pageInfo.TotalItems);
        Assert.Equal(2, pageInfo.TotalPages);
    }

    [Fact]
    public void Can_Filter_Products()
    {
        // Arrange

        Mock<IStoreRepository> mock = new();
        mock.Setup(m => m.Products).Returns((new Product[] {
            new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
            new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
            new Product {ProductID = 3, Name = "P3", Category = "Cat1"},
            new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
            new Product {ProductID = 5, Name = "P5", Category = "Cat3"}
        }).AsQueryable());

        HomeController controller = new(mock.Object) { PageSize = 3 };

        // Act

        Product[] result = (controller.Index("Cat2", 1)?.ViewData.Model 
            as ProductListViewModel ?? new()).Products.ToArray();

        // Assert

        Assert.Equal(2, result.Length);
        Assert.True(result[0].Name == "P2" && result[0].Category == "Cat2");
        Assert.True(result[1].Name == "P4" && result[1].Category == "Cat2");
    }
}