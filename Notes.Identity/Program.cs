using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Notes.Identity;
using Notes.Identity.Data;
using Notes.Identity.Models;
using System.IO;
var builder = WebApplication.CreateBuilder(args);
RegisterService(builder.Services);

var app = builder.Build();
Configure(app);
app.Run();

void RegisterService(IServiceCollection services)
{

    var connectionString = builder.Configuration.GetValue<string>("DbConnection");
    services.AddDbContext<AuthDbContext>(options =>
    {
        options.UseSqlite(connectionString);
    });
    using (var scope = services.BuildServiceProvider().CreateScope())
    {
        var serviceProvider = scope.ServiceProvider;
        try
        {
            var context = serviceProvider.GetRequiredService<AuthDbContext>();
            DbInitializer.Initialize(context);
        }
        catch (Exception exception)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(exception, "An error occured while app initialize");
        }
    }
    services.AddIdentity<AppUser, IdentityRole>(config =>
    {
        config.Password.RequiredLength = 4;
        config.Password.RequireDigit = false;
        config.Password.RequireUppercase = false;
        config.Password.RequireNonAlphanumeric = false;
    })
        .AddEntityFrameworkStores<AuthDbContext>()
        .AddDefaultTokenProviders();

    services.AddIdentityServer()
        .AddAspNetIdentity<AppUser>()
        .AddInMemoryApiResources(Configuration.ApiResources)
        .AddInMemoryIdentityResources(Configuration.IdentityResources)
        .AddInMemoryApiScopes(Configuration.ApiScopes)
        .AddInMemoryClients(Configuration.Clients)
        .AddDeveloperSigningCredential();

    services.ConfigureApplicationCookie(config =>
    {
        config.Cookie.Name = "Notes.Identity.Cookie";
        config.LoginPath = "/Auth/login";
        config.LogoutPath = "/Auth/logout";
    });
    services.AddControllersWithViews();
    //services.AddMvc();
}
void Configure(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, "Styles")),
        RequestPath = "/Styles"
    });
    app.UseRouting();
    app.UseIdentityServer();
    //app.MapGet("/", () => "identity!");
    app.MapDefaultControllerRoute();
}
