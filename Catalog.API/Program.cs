using Catalog.Application;
using Catalog.Infrastructure;
using Catalog.Infrastructure.Persistence;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Catalog.API
{
    public partial class Program
    {
        public static async Task Main(string[] args)
        {
            // Set FluentValidation to use English culture globally
            ValidatorOptions.Global.LanguageManager.Culture = new System.Globalization.CultureInfo("en");

            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables();

            // Add services to the container.
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddWebServices();
            builder.Services.AddControllers();

            // Configure Authentication with JWT Bearer
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}/";
                    options.Audience = builder.Configuration["Auth0:Audience"];
                });


            var app = builder.Build();
            await app.InitialiseDatabaseAsync();
            

            app.UseHealthChecks("/health");
            app.UseStaticFiles();
            app.UseOpenApi();
            app.UseSwaggerUi();

            app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");

            app.UseExceptionHandler(options => { });


            app.Map("/", () => Results.Redirect("/api"));

            // Enable authentication middleware
            app.UseAuthentication();

            app.MapEndpoints();

            app.Run();
        }
    }
}
