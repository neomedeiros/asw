using System.Threading.Tasks;
using ASW.Entities.Enums;
using ASW.Models;

namespace ASW.Services.Contracts
{
    /// <summary>
    /// Contract for Diff operation
    /// </summary>
    public interface IDiffService
    {
        /// <summary>
        /// Insert on storage the data for both sides, to be diff-ed later
        /// </summary>
        /// <param name="id">Identity of the Diff Request</param>
        /// <param name="side">Side of the data</param>
        /// <param name="data">the string to be compared</param>
        /// <returns>void</returns>
        Task PostDiffEntry(long id, Side side, string data);

        /// <summary>
        /// Executes and Returns the Diff operation
        /// </summary>
        /// <param name="id">Identity of the Diff Request</param>
        /// <returns>Intance of DiffResultModel</returns>
        Task<DiffResultModel> Diff(long id);        
    }
}