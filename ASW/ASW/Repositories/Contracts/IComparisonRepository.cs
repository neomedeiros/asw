using System.Threading.Tasks;
using ASW.Entities;

namespace ASW.Repositories.Contracts
{
    public interface IComparisonRepository: IBaseRepository<ComparisonRequestEntity>
    {
        Task Insert(ComparisonRequestEntity entity);
        void Update(ComparisonRequestEntity entity);
        Task<ComparisonRequestEntity> Get(long id);
    }
}