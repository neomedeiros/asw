using System.Threading.Tasks;
using ASW.Entities;

namespace ASW.Repositories.Contracts
{
    /// <summary>
    /// Diff Repository
    /// Provide domain operations needed by business rules (Service Tier)
    /// * Low level coupling, Storage Technology could be easily changed without impact business rules level
    /// Helpful for Mocking in Unit Tests
    /// </summary>
    public interface IDiffRepository: IBaseRepository<ComparisonRequestEntity>
    {
        Task Insert(ComparisonRequestEntity entity);
        void Update(ComparisonRequestEntity entity);
        Task<ComparisonRequestEntity> Get(long id);
    }
}