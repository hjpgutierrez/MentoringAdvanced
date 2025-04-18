
using Catalog.Infrastructure;
using Catalog.Application;

namespace Catalog.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddWebServices();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHealthChecks("/health");

            app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");

            app.UseExceptionHandler(options => { });

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapEndpoints();

            app.MapControllers();

            app.Run();
        }
    }
}
