using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmsProcessingService.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SmsProcessingService.Persistence
{
    public interface IApplicationDbContext
    {
        DbSet<SmsEntity> SmsEntities { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new());

        DatabaseFacade Database { get; }
    }
}
