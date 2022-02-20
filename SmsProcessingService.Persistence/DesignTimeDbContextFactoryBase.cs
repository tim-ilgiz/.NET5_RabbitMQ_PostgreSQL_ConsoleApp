using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SmsProcessingService.Persistence
{
    public abstract class DesignTimeDbContextFactoryBase<TContext> : IDesignTimeDbContextFactory<TContext>
        where TContext : DbContext
    {
        private const string ConnectionStringName = "PG_CONNECTION_STRING";
        private const string AspNetCoreEnvironment = "ASPNETCORE_ENVIRONMENT";

        public TContext CreateDbContext(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable(AspNetCoreEnvironment);
            var basePath = AppContext.BaseDirectory;
            return Create(basePath, environmentName);
        }

        protected abstract TContext CreateNewInstance(DbContextOptions<TContext> options);

        private TContext Create(string basePath, string environmentName)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString(ConnectionStringName);

            return Create(connectionString);
        }

        private TContext Create(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException($"Connection string '{ConnectionStringName}' is null or empty.",
                    nameof(connectionString));

            Console.WriteLine(
                $"DesignTimeDbContextFactoryBase.Create(string): Connection string: '{connectionString}'.");

            var optionsBuilder = new DbContextOptionsBuilder<TContext>();

            optionsBuilder.UseNpgsql(connectionString
                , b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));

            return CreateNewInstance(optionsBuilder.Options);
        }
    }
}