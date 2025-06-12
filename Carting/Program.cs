using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Asp.Versioning.Conventions;
using Carting.BLL.Interfaces;
using Carting.BLL.Services;
using Carting.Configuration;
using Carting.DAL.MessageBrokers;
using Carting.DAL.Persistence;
using Carting.Middleware;
using Carting.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Carting
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
            // Configure Authentication with JWT Bearer
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {                    
                    options.Authority = domain;
                    options.Audience = builder.Configuration["Auth0:Audience"];
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("all:carting", policy => policy.Requirements.Add(new
                HasScopeRequirement("all:carting", domain)));
            });
            builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

            // Add services to the container.
            builder.Services.Configure<DatabaseSettings>(
                builder.Configuration.GetSection("CartDatabase"));

            builder.Services.AddControllers();
            builder.Services.AddApiVersioning(options =>
            {
                
                options.DefaultApiVersion = new ApiVersion(1.0); 
                options.AssumeDefaultVersionWhenUnspecified = true;

                options.ReportApiVersions = true;

                options.ApiVersionReader = ApiVersionReader.Combine(
                               new UrlSegmentApiVersionReader(),
                               new QueryStringApiVersionReader("api-version"),
                               new HeaderApiVersionReader("X-Version"),
                               new MediaTypeApiVersionReader("x-version"));
            }).AddMvc(options =>
            {
                // automatically applies an api version based on the name of
                // the defining controller's namespace
                options.Conventions.Add(new VersionByNamespaceConvention());
            })
            .AddApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });
          
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.ConfigureOptions<NamedSwaggerGenOptions>();

            builder.Services.AddSingleton(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddTransient<ICartService, CartService>();

            builder.Services.AddSingleton<IMessageProcessor, MessageProcessor>();
            builder.Services.AddHostedService<RabbitMqMessageConsumer>();

            var app = builder.Build();
            var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint(
                            $"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                });
            }

            app.UseHttpsRedirection();
            app.UseMiddleware<AccessTokenLoggingMiddleware>();


            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
