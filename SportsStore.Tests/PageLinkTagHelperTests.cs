using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using SportsStore.infrastructure;

namespace SportsStore.Tests;

/*  The complexity in this test is in creating the objects that are required to create and use a tag helper.
    Tag helpers use IUrlHelperFactory objects to generate URLs that target different parts of the application, 
and I have used Moq to create an implementation of this interface and the related IUrlHelper interface that provides test data.
    The core part of the test verifies the tag helper output by using a literal string value that contains double
quotes. C# is perfectly capable of working with such strings, as long as the string is prefixed with @
and uses two sets of double quotes ("") in place of one set of double quotes. 
*/
public class PageLinkTagHelperTests 
{
    [Fact]
    public void Can_Generate_Page_Links()
    {
        // Arrange

        Mock<IUrlHelper> urlHelper = new();
        urlHelper.SetupSequence(x => x.Action(It.IsAny<UrlActionContext>()))
            .Returns("Test/Page1")
            .Returns("Test/Page2")
            .Returns("Test/Page3");

        Mock<IUrlHelperFactory> urlHelperFactory = new();
        urlHelperFactory.Setup(f => f.GetUrlHelper(It.IsAny<ActionContext>()))
            .Returns(urlHelper.Object);

        Mock<ViewContext> viewContext = new();

        PageLinkTagHelper helper = new(urlHelperFactory.Object) {
            PageModel = new Models.ViewModels.PagingInfo { CurrentPage = 2, TotalItems = 28, ItemsPerPage = 10},
            ViewContext = viewContext.Object,
            PageAction = "Test"
        };

        TagHelperContext context = new(new TagHelperAttributeList(), new Dictionary<object, object>(), "");

        Mock<TagHelperContent> content = new();
        TagHelperOutput output = new("div", new TagHelperAttributeList(), (cache, encoder) => Task.FromResult(content.Object));
                                    
        // Act

        helper.Process(context, output);

        // Assert

        Assert.Equal(@"<a href=""Test/Page1"">1</a>" 
            + @"<a href=""Test/Page2"">2</a>" 
            + @"<a href=""Test/Page3"">3</a>", output.Content.GetContent());
    }
}
