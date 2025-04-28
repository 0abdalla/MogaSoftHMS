using Hospital_MS.Core.Contracts.Beds;
using Hospital_MS.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers
{
    [Authorize]
    public class BedsController(IBedService bedService) : ApiBaseController
    {
        private readonly IBedService _bedService = bedService;

        [HttpPost("")]
        public async Task<IActionResult> CreateBed([FromBody] CreateBedRequest request, CancellationToken cancellationToken)
        {
            var result = await _bedService.CreateAsync(request, cancellationToken);
            return Ok(result);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllBeds(CancellationToken cancellationToken)
        {
            var result = await _bedService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

    }
}
