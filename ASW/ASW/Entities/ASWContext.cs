using ASW.Entities;
using Microsoft.EntityFrameworkCore;

namespace ASW.Repository
{
    public class ASWContext : DbContext
    {
        public ASWContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ComparisonRequestEntity> ComparisonRequests { get; set; }
    }
}