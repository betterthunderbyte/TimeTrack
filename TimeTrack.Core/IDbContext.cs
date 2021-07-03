using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TimeTrack.Core.Model;

namespace TimeTrack.UseCase
{
    public interface IDbContext
    {

        public DbSet<ActivityEntity> Activities { get; set; }
        public DbSet<ProjectEntity> Projects { get; set; }
        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<ActivityTypeEntity> ActivityTypes { get; set; }
        public DbSet<MemberEntity> Members { get; set; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}