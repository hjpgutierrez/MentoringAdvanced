﻿using Catalog.Application.Common.Interfaces;
using Catalog.Infrastructure.Persistence;
using Catalog.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ardalis.GuardClauses;
using CleanArchitecture.Infrastructure.Data.Interceptors;
using Catalog.Infrastructure.MessageBrokers;

namespace Catalog.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {            
                options.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly("Catalog.API"));
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                options.EnableDetailedErrors();                
            });

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<ApplicationDbContextInitialiser>();
            services.AddSingleton(TimeProvider.System);
            services.AddSingleton<IMessagePublisher, RabbitMqMessagePublisher>();
            return services;
        }
    }

}
