using System.Threading.Tasks;
using ASW.Entities;
using ASW.Repositories.Contracts;
using ASW.Repository;

namespace ASW.Repositories
{
    public class ComparisonRepository : BaseRepository<ComparisonRequestEntity>, IComparisonRepository
    {        
        public ComparisonRepository(ASWContext context) : base(context) {}

        public Task<ComparisonRequestEntity> Get(long id)
        {
            return FirstOrDefault(m => m.Id == id);
        }
    }
}