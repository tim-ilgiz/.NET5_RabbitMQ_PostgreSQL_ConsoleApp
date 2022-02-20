using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmsProcessingService.Domain.Settings;
using SmsProcessingService.Persistence;
using SmsProcessingService.Service.Contract;
using SmsProcessingService.Service.Implementation;

namespace ConsoleApplication
{
    public class Program
    {
        private static IConfigurationRoot? _configurationRoot;

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static void LoadConfiguration(HostBuilderContext host, IConfigurationBuilder builder)
        {
            builder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true);

            _configurationRoot = builder.Build();
        }

        private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
            services
                .AddTransient<ISmsService, SmsService>()
                .AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(_configurationRoot["ConnectionStrings:PG_CONNECTION_STRING"]
                        , b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)))
                .AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>())
                .Configure<RabbitMqConfiguration>(a =>
                    _configurationRoot.GetSection(nameof(RabbitMqConfiguration)).Bind(a))
                .AddSingleton<IRabbitMqService, RabbitMqService>()
                .AddSingleton<IConsumerService, ConsumerService>()
                .AddHostedService<ConsumerHostedService>();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(LoadConfiguration)
                .ConfigureServices(ConfigureServices);
        }
    }
}