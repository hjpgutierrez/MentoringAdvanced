using Catalog.Infrastructure;
using Catalog.Application;
using Catalog.Infrastructure.Persistence;

namespace Catalog.API
{
    public partial class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddWebServices();
            builder.Services.AddControllers();
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

            app.MapEndpoints();

            app.Run();
        }
    }
}
