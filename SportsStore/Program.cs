/*  The Program.cs file is used to configure the ASP.NET Core application.
*/
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SportsStore.Models;

var builder = WebApplication.CreateBuilder(args);

/*  The builder.Services property is used to set up objects, known as services, that can be used throughout
the application and that are accessed through a DI.
    The AddControllersWithViews method sets up the shared objects required by applications
using the MVC Framework and the Razor view engine.
    The AddScoped method creates a service where each HTTP request gets its own repository object, which
is the way that Entity Framework Core is typically used.
*/
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<StoreDbContext>(options => {
    options.UseSqlServer(builder.Configuration["ConnectionStrings:SportsStoreConnection"]);
});
builder.Services.AddScoped<IStoreRepository, EFStoreRepository>();
builder.Services.AddScoped<IOrderRepository, EFOrderRepository>();
/*  For variety, let's use Razor Pages application framework to implement shopping cart functionality.
    While Razor Pages are part of the standard ASP.NET Core project template, explicitly calling the
`AddRazorPages` method sets up the services used by Razor Pages, and the `MapRazorPages` method
registers Razor Pages as endpoints that the URL routing system can use to handle requests.
*/
builder.Services.AddRazorPages();
/*  The `AddDistributedMemoryCache` method call sets up the in-memory data store.
    The `AddSession` method registers the services used to access session data, and the `UseSession` method
allows the session system to automatically associate requests with sessions when they arrive from the client.
*/
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
/*  The `AddScoped` method specifies that the same object should be used to satisfy related requests for Cart instances.
How requests are related can be configured, but by default, it means that any Cart required by components handling the
same HTTP request will receive the same object.
    Rather than provide the `AddScoped` method with a type mapping, as I did for the repository, I have specified
a lambda expression that will be invoked to satisfy Cart requests. The expression receives the collection of services
that have been registered and passes the collection to the GetCart method of the SessionCart class. The result is that
requests for the Cart service will be handled by creating SessionCart objects, which will serialize themselves
as session data when they are modified.
    I also added a service using the `AddSingleton` method, which specifies that the same object should
always be used. The service I created tells ASP.NET Core to use the HttpContextAccessor class when
implementations of the IHttpContextAccessor interface are required. This service is required so I can
access the current session in the SessionCart class.
*/
builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
/*  Blazor combines client-side JavaScript code with server-side code executed by ASP.NET Core, connected by a persistent HTTP
connection.
*/
builder.Services.AddServerSideBlazor();

/*  I extended the Entity Framework Core configuration to register the context class and used the `AddIdentity` method to
set up the Identity services using the built-in classes to represent users and roles.
*/
builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:IdentityConnection"]));
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();

var app = builder.Build();

/*  ASP.NET Core receives HTTP requests and passes them along a request pipeline, which is populated
with middleware components registered using the app property. Each middleware component is able to
inspect requests, modify them, generate a response, or modify the responses that other components have
produced.
    The UseStaticFiles method enables support for serving static content from the wwwroot folder and
will be created later in the chapter
*/
app.UseStaticFiles();
app.UseSession();

/*  I called the UseAuthentication and UseAuthorization methods to set up the middleware components that
implement the security policy.
*/
app.UseAuthorization();
app.UseAuthorization();
/*  I can create URLs that are more appealing to user by creating a scheme that follows the pattern of composable
URLs (e.g, "http://localhost/Products/Page2" instead of default "http://localhost/?productPage=2"). The ASP.NET Core routing
feature makes it easy to change the URL scheme in an application.
    Following alterations are required to change the URL schemes for product pagination and filtering them by category.
    ASP.NET Core and the routing function are tightly integrated, so the application automatically reflects a change like
this in the URLs used by the application, including those generated by tag helpers like the one I use to generate the
page navigation links. This ensures that all the URLs in the application are consistent.
*/
app.MapControllerRoute("catpage", "{category}/Page{productPage:int}", new { Controler = "Home", action = "Index"});
app.MapControllerRoute("page", "Page{productPage:int}", new { Controller = "Home", action = "Index", productPage = 1});
app.MapControllerRoute("category", "{category}", new { Controller = "Home", action = "Index", productPage = 1});
app.MapControllerRoute("pagination", "Products/Page{productPage}",
    new { Controller = "Home", action = "Index", productPage = 1 });

/*  One especially important middleware component provides the endpoint routing feature, which
matches HTTP requests to the application features—known as endpoints—able to produce responses for
them.
    The endpoint routing feature is added to the request
pipeline automatically, and the MapDefaultControllerRoute registers the MVC Framework as a source of
endpoints using a default convention for mapping requests to classes and methods.
*/
app.MapDefaultControllerRoute();
app.MapRazorPages();
/*  The `AddServerSideBlazor` method creates the services that Blazor uses, and the `MapBlazorHub` method
registers the Blazor middleware components. The final addition is to finesse the routing system to ensure
that Blazor works seamlessly with the rest of the application.
*/
app.MapBlazorHub();
app.MapFallbackToPage("/admin/{*catchall}", "/Admin/Index");

// Comment out to speed up app building, since migrations are not needed now
// SeedData.EnsurePopulated(app);
// IdentitySeedData.EnsurePopulated(app);

app.Run();