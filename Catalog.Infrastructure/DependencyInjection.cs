using Catalog.Application.Common.Interfaces;
using Catalog.Infrastructure.Persistence;
using Catalog.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            //Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {            
                options.UseSqlServer(connectionString);
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            });

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            //services.AddScoped<ApplicationDbContextInitialiser>();

//#if (UseApiOnly)
//        services.AddAuthentication()
//            .AddBearerToken(IdentityConstants.BearerScheme);

//        services.AddAuthorizationBuilder();

//        services
//            .AddIdentityCore<ApplicationUser>()
//            .AddRoles<IdentityRole>()
//            .AddEntityFrameworkStores<ApplicationDbContext>()
//            .AddApiEndpoints();
//#else
//            services
//                .AddDefaultIdentity<ApplicationUser>()
//                .AddRoles<IdentityRole>()
//                .AddEntityFrameworkStores<ApplicationDbContext>();
//#endif

            services.AddSingleton(TimeProvider.System);


            return services;
        }
    }

}
