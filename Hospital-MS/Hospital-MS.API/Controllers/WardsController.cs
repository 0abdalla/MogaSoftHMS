using Hospital_MS.Core.Contracts.Wards;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers
{
    [Authorize]
    public class WardsController(IWardService wardService) : ApiBaseController
    {
        private readonly IWardService _wardService = wardService;

        [HttpPost("")]
        public async Task<IActionResult> CreateWard([FromBody] CreateWardRequest request, CancellationToken cancellationToken)
        {
            var result = await _wardService.CreateAsync(request, cancellationToken);
            return Ok(result);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _wardService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWardById(int id, CancellationToken cancellationToken)
        {
            var result = await _wardService.GetByIdAsync(id, cancellationToken);
            return Ok(result);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWard(int id, [FromBody] CreateWardRequest request, CancellationToken cancellationToken)
        {
            var result = await _wardService.UpdateAsync(id, request, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWard(int id, CancellationToken cancellationToken)
        {
            var result = await _wardService.DeleteAsync(id, cancellationToken);
            return Ok(result);
        }
    }
}
