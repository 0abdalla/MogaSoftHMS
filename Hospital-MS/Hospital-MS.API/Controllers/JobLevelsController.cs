using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.JobLevel;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers
{
    [Authorize]
    public class JobLevelsController(IJobLevelService jobLevelService) : ApiBaseController
    {
        private readonly IJobLevelService _jobLevelService = jobLevelService;

        [HttpPost("")]
        public async Task<IActionResult> CreateAsync([FromBody] JobLevelRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _jobLevelService.CreateAsync(request, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] JobLevelRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _jobLevelService.UpdateAsync(id, request, cancellationToken);
            return Ok(result);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var result = await _jobLevelService.GetAsync(id, cancellationToken);
            return Ok(result);

        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllAsync([FromQuery] PagingFilterModel pagingFilter, CancellationToken cancellationToken = default)
        {
            var result = await _jobLevelService.GetAllAsync(pagingFilter, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var result = await _jobLevelService.DeleteAsync(id, cancellationToken);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
