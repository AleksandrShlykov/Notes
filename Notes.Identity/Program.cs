using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Notes.Identity;
using Notes.Identity.Data;
using Notes.Identity.Models;
var builder = WebApplication.CreateBuilder(args);
RegisterService(builder.Services);

var app = builder.Build();

Configure(app);
using (var scope = app.Services.CreateScope())
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
app.Run();

void RegisterService(IServiceCollection services)
{

    var connectionString = builder.Configuration.GetValue<string>("DbConnection");
    services.AddDbContext<AuthDbContext>(options =>
    {
        options.UseSqlite(connectionString);
    });

    services.AddIdentity<AppUser, IdentityRole>(config =>
    {
        config.Password.RequiredLength = 4;
        config.Password.RequireDigit = false;
        config.Password.RequireUppercase = false;
        config.Password.RequireNonAlphanumeric = false;
    })
        .AddEntityFrameworkStores<AuthDbContext>()
        .AddDefaultTokenProviders();

    services.AddIdentityServer(options =>
    {
        options.UserInteraction.LoginUrl = "/Auth/Login";
        options.UserInteraction.LogoutUrl = "/Auth/Logout";
    })
        .AddAspNetIdentity<AppUser>()
        .AddInMemoryApiResources(Configuration.ApiResources)
        .AddInMemoryIdentityResources(Configuration.IdentityResources)
        .AddInMemoryApiScopes(Configuration.ApiScopes)
        .AddInMemoryClients(Configuration.Clients)
        .AddDeveloperSigningCredential();

    services.ConfigureApplicationCookie(config =>
    {
    });
    services.AddControllersWithViews();
}
void Configure(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseIdentityServer();
    app.MapGet("/Index", () => "Login succes");
    app.MapDefaultControllerRoute();
}
