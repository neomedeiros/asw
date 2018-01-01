using System.Threading.Tasks;
using ASW.Entities.Enums;
using ASW.Filters;
using ASW.Models;
using ASW.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ASW.Controllers
{
    /// <summary>
    /// Diff Controller. Provides the diff functionality to compare to sides of string data and retrieve the differences
    /// </summary>
    [Route("")]
    public class DiffController : Controller
    {
        private readonly IDiffService _diffService;

        public DiffController(IDiffService diffService)
        {
            _diffService = diffService;
        }

        /// <summary>
        /// Insert the left side data for for the Asynchronous Diff comparison.
        /// </summary>
        /// <param name="id">Information to identify the Diff Comparison Request</param>
        /// <param name="data">data to be diff-ed</param>
        /// <returns></returns>
        [Route("v1/diff/{id}/left")]
        [HttpPost]
        [CustomExceptionFilter]
        public async Task<ActionResult> PostLeftDiffEntry(long id, [FromBody] string data)
        {
            await _diffService.PostDiffEntry(id, Side.Left, data);
            return Ok();
        }

        /// <summary>
        /// Insert the right side data for for the Asynchronous Diff comparison.
        /// </summary>
        /// <param name="id">Information to identify the Diff Comparison Request</param>
        /// <param name="data">data to be diff-ed</param>
        [Route("v1/diff/{id}/right")]
        [HttpPost]
        [CustomExceptionFilter]
        public async Task<ActionResult> PostRightDiffEntry(long id, [FromBody] string data)
        {
            await _diffService.PostDiffEntry(id, Side.Right, data);
            return Ok();
        }

        /// <summary>
        /// Get the Asynchronous diff process result.
        /// </summary>
        /// <param name="id">identify Information to get the Diff comparison result. It was sent before on the operations to post the data that shoud be diff-ed.</param>        
        [HttpGet("v1/diff/{id}")]
        [CustomExceptionFilter]
        public async Task<DiffResultModel> GetDiff(long id)
        {
            return await _diffService.Diff(id);
        }
    }
}