using Microsoft.AspNetCore.Builder;
using SmsProcessingService.Infrastructure.Middlewares;

namespace SmsProcessingService.Infrastructure.Extensions
{
    public static class ConfigureContainer
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomExceptionMiddleware>();
        }

        public static void ConfigureSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint("/swagger/OpenAPISpecification/swagger.json", "OpenAPI");
                setupAction.RoutePrefix = "OpenAPI";
            });
        }
    }
}