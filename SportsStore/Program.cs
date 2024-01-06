/*  The Program.cs file is used to configure the ASP.NET Core application.
*/
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

var app = builder.Build();

// app.MapGet("/", () => "Hello World!");

/*  ASP.NET Core receives HTTP requests and passes them along a request pipeline, which is populated
with middleware components registered using the app property. Each middleware component is able to
inspect requests, modify them, generate a response, or modify the responses that other components have
produced. 
    The UseStaticFiles method enables support for serving static content from the wwwroot folder and
will be created later in the chapter
*/
app.UseStaticFiles();

/*  One especially important middleware component provides the endpoint routing feature, which
matches HTTP requests to the application features—known as endpoints—able to produce responses for
them.  
    The endpoint routing feature is added to the request
pipeline automatically, and the MapDefaultControllerRoute registers the MVC Framework as a source of
endpoints using a default convention for mapping requests to classes and methods.
*/
app.MapDefaultControllerRoute();

/*  To perform initial migration and populate database with some sample data 
*/
// SeedData.EnsurePopulated(app);

app.Run();