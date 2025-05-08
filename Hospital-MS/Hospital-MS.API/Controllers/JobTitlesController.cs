using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.JobTitle;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers
{
    [Authorize]
    public class JobTitlesController(IJobTitleService jobTitleService) : ApiBaseController
    {
        private readonly IJobTitleService _jobTitleService = jobTitleService;

        [HttpPost("")]
        public async Task<IActionResult> CreateAsync([FromBody] JobTitleRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _jobTitleService.CreateAsync(request, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] JobTitleRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _jobTitleService.UpdateAsync(id, request, cancellationToken);
            return Ok(result);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var result = await _jobTitleService.GetAsync(id, cancellationToken);
            return Ok(result);

        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllAsync([FromQuery] PagingFilterModel pagingFilter, CancellationToken cancellationToken = default)
        {
            var result = await _jobTitleService.GetAllAsync(pagingFilter, cancellationToken);
            return Ok(result);
        }
    }
}
