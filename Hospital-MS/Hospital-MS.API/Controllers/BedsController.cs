using Hospital_MS.Core.Contracts.Beds;
using Hospital_MS.Interfaces.HMS;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBedById(int id, CancellationToken cancellationToken)
        {
            var result = await _bedService.GetByIdAsync(id, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBed(int id, [FromBody] CreateBedRequest request, CancellationToken cancellationToken)
        {
            var result = await _bedService.UpdateAsync(id, request, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBed(int id, CancellationToken cancellationToken)
        {
            var result = await _bedService.DeleteAsync(id, cancellationToken);
            return Ok(result);
        }
    }
}
