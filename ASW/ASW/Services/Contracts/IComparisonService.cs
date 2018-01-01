using System.Threading.Tasks;
using ASW.Entities.Enums;
using ASW.Models;

namespace ASW.Services.Contracts
{
    public interface IComparisonService
    {
        Task<DiffResultModel> Diff(long id);
        Task PostDiffEntry(long id, Side side, string data);
    }
}