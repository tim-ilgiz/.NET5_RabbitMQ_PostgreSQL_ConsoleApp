using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SmsProcessingService.Persistence;

namespace SmsProcessingService.Infrastructure.Extensions
{
    public static class ConfigureServiceContainer
    {
        public static void AddDbContext(this IServiceCollection serviceCollection,
            IConfiguration configuration, IConfigurationRoot configRoot)
        {
            serviceCollection
                .AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("PG_CONNECTION_STRING") ??
                                      configRoot["ConnectionStrings:PG_CONNECTION_STRING"]
                        , b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }

        public static void AddSwaggerOpenApi(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc(
                    "OpenAPISpecification",
                    new OpenApiInfo
                    {
                        Title = "SmsProcessingService API",
                        Version = "1",
                        Description = "Through this API you can access customer details"
                    });
            });
        }
    }
}