﻿using Carting.DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Carting.IntegrationTests
{
    public class WebAppFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(ReplaceDbContextWithInMemoryDb);
        }

        /// <summary>
        /// It is necessary to replace the default DbContext with an in-memory database
        /// </summary>
        /// <param name="services"></param>
        private static void ReplaceDbContextWithInMemoryDb(IServiceCollection services)
        {
            var existingDbContextRegistration = services.SingleOrDefault(
                d => d.ServiceType == typeof(DatabaseSettings)
            );

            if (existingDbContextRegistration != null)
            {
                //services.Remove(existingDbContextRegistration);
            }

            //var connectionStringBuilder = new DatabaseSettings { ConnectionString = "mongodb://localhost:27017", CollectionName = "Cart", DatabaseName = "IntegrationTests" };
            //services.Configure<DatabaseSettings>(connectionStringBuilder);
        }
    }
}
