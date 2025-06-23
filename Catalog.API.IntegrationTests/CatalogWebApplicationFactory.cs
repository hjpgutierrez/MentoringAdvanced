using Catalog.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Catalog.API.IntegrationTests
{
    public class CatalogWebApplicationFactory : WebApplicationFactory<Program>
    {

        protected override void ConfigureWebHost(IWebHostBuilder builder) => builder.ConfigureTestServices(services =>
                                                                                      {

                                                                                          var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                                                                                          if (descriptor != null)
                                                                                          {
                                                                                              services.Remove(descriptor);
                                                                                          }

                                                                                          services.AddDbContext<ApplicationDbContext>((sp, options) =>
                                                                                          {
                                                                                              options.UseSqlServer(GetConnectionString(), sqlOptions => sqlOptions.MigrationsAssembly("Catalog.API"));
                                                                                              options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                                                                                              options.EnableDetailedErrors();
                                                                                          });

                                                                                          var serviceProvider = services.BuildServiceProvider();
                                                                                          var scope = serviceProvider.CreateScope();
                                                                                          var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                                                                                          try
                                                                                          {
                                                                                              dbContext.Database.EnsureCreated();
                                                                                          }
                                                                                          catch (Exception ex)
                                                                                          {
                                                                                              var logger = scope.ServiceProvider.GetRequiredService<ILogger<CatalogWebApplicationFactory>>();
                                                                                              logger.LogError(ex, ex.Message);
                                                                                          }
                                                                                      });

        private static string? GetConnectionString()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.Test.json")
                .Build();

            var cn = configuration.GetConnectionString("DefaultConnection");
            return cn;
        }
    }
}
