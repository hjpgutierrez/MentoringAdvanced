namespace Catalog.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services)
        {
            //services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddHttpContextAccessor();

            //services.AddHealthChecks()
            //    .AddDbContextCheck<ApplicationDbContext>();

            services.AddExceptionHandler<CustomExceptionHandler>();

            //services.AddRazorPages();

            // Customise default API behaviour
            //services.Configure<ApiBehaviorOptions>(options =>
            //    options.SuppressModelStateInvalidFilter = true);

            services.AddEndpointsApiExplorer();

            //services.AddOpenApiDocument((configure, sp) =>
            //{
            //    configure.Title = "CleanArchitecture API";
            //});

            return services;
        }
    }

}
