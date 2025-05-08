using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.JobTitle;
using Hospital_MS.Core.Contracts.JobType;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Services.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers
{
    [Authorize]
    public class JobTypesController(IJobTypeService jobTypeService) : ApiBaseController
    {
        private readonly IJobTypeService _jobTypeService = jobTypeService;

        [HttpPost("")]
        public async Task<IActionResult> CreateAsync([FromBody] JobTypeRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _jobTypeService.CreateAsync(request, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] JobTypeRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _jobTypeService.UpdateAsync(id, request, cancellationToken);
            return Ok(result);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var result = await _jobTypeService.GetAsync(id, cancellationToken);
            return Ok(result);

        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllAsync([FromQuery] PagingFilterModel pagingFilter, CancellationToken cancellationToken = default)
        {
            var result = await _jobTypeService.GetAllAsync(pagingFilter, cancellationToken);
            return Ok(result);
        }
    }
}
