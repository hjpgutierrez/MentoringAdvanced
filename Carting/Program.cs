using Carting.BLL.Interfaces;
using Carting.BLL.Services;
using Asp.Versioning;
using Asp.Versioning.Conventions;
using Carting.Configuration;
using Asp.Versioning.ApiExplorer;
using Carting.DAL.Persistence;
using Carting.DAL.MessageBrokers;

namespace Carting
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
