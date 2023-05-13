using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Configuration;
using System.Web.Http;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews(options =>
{
    options.EnableEndpointRouting =false;
});

builder.Services.AddHttpContextAccessor();              
builder.Services.AddHttpClient();


builder.Services.AddAuthentication(config=>
{
    config.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = "oidc";
})
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddOpenIdConnect("oidc", config=>
    {
        config.Authority = "https://localhost:7032";
        config.ClientSecret = "notesWebapp_secret";
        config.ClientId = "notes-web-api";
        config.SaveTokens = true;
        config.ResponseType = "code";

    });
builder.Services.AddApiVersioning(options=>{
      options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});
var app = builder.Build();
var config = new HttpConfiguration();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=note}/{action=loginpage}");
app.Run();