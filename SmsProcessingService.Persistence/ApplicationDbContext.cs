using Microsoft.EntityFrameworkCore;
using SmsProcessingService.Domain.Entities;

namespace SmsProcessingService.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<SmsEntity> SmsEntities { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            modelBuilder.Entity<SmsEntity>().HasKey(o => new { o.Id });
        }
    }
}
