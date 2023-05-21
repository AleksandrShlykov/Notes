using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Notes.Application;
using Notes.Application.Interfaces;
using Notes.Application.Middleware;
using Notes.Persistion;
using Notes.WebApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


RegisterServices(builder.Services);


var app = builder.Build();
Configuration(app, app.Environment);

app.Run();

void RegisterServices(IServiceCollection services)
{
    services.AddAutoMapper(config =>
    {
        config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
        config.AddProfile(new AssemblyMappingProfile(typeof(INotesDbContext).Assembly));

    });
    services.AddAplication();
    services.AddPersistance(builder.Configuration);
    services.AddControllers();
    services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowAnyOrigin();
        });
    });
    using (var scope = services.BuildServiceProvider().CreateScope())
    {
        var serviceProvider = scope.ServiceProvider;
        try
        {
            var context = serviceProvider.GetRequiredService<NotesDbContext>();
            DbInitializer.Initialize(context);
        }
        catch (Exception exception)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(exception, "An error occured while app initialize with NotesDbInitialize");
        }
    }
    services.AddAuthentication(config =>
    {
        config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    })
        .AddJwtBearer("Bearer", options =>
        {

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ClockSkew = TimeSpan.FromSeconds(5),
                ValidateAudience = false
            };
            options.Authority = $"https://localhost:7032";

            options.RequireHttpsMetadata = false;

        });

    services.AddApiVersioning(setup =>
    {
        setup.DefaultApiVersion = new ApiVersion(1, 0);
        setup.AssumeDefaultVersionWhenUnspecified = true;
        setup.ReportApiVersions = true;
    });


    services.AddVersionedApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
    });
    services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
    services.AddSwaggerGen();
}

void Configuration(WebApplication app, IWebHostEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwagger();
    app.UseSwaggerUI(config =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {

            config.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
            config.RoutePrefix = string.Empty;
        }
        config.SwaggerEndpoint("swagger/v1/swagger.json", "NotesApi");
    });
    app.UseCustomExceptionHandler();
    app.UseRouting();
    app.UseHttpsRedirection();
    app.UseCors("AllowAll");
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseApiVersioning();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}