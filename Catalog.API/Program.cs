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

            builder.Services.AddAuthorization(options =>
            {
                // Define policies based on roles or permissions
                options.AddPolicy("ManagerPolicy", policy => policy.RequireClaim("permissions", "read:catalog", "create:catalog", "update:catalog", "delete:catalog"));
                options.AddPolicy("CustomerPolicy", policy => policy.RequireClaim("permissions", "read:catalog"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                await app.InitialiseDatabaseAsync();
            }
            else
            {
                // see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHealthChecks("/health");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseOpenApi();
            app.UseSwaggerUi();

            app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");

            app.UseExceptionHandler(options => { });


            app.Map("/", () => Results.Redirect("/api"));

            // Enable authentication and authorization middleware
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapEndpoints();

            app.Run();
        }
    }
}
