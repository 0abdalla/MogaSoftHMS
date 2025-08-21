using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.JobDepartment;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers
{
    [Authorize]
    public class JobDepartmentController(IJobDepartmentService jobDepartmentService) : ApiBaseController
    {
        private readonly IJobDepartmentService _jobDepartmentService = jobDepartmentService;

        [HttpPost("")]
        public async Task<IActionResult> CreateAsync([FromBody] JobDepartmentRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _jobDepartmentService.CreateAsync(request, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] JobDepartmentRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _jobDepartmentService.UpdateAsync(id, request, cancellationToken);
            return Ok(result);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var result = await _jobDepartmentService.GetAsync(id, cancellationToken);
            return Ok(result);

        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllAsync([FromQuery] PagingFilterModel pagingFilter, CancellationToken cancellationToken = default)
        {
            var result = await _jobDepartmentService.GetAllAsync(pagingFilter, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var result = await _jobDepartmentService.DeleteAsync(id, cancellationToken);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
