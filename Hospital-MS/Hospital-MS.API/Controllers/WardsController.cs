using Hospital_MS.Core.Contracts.Wards;
using Hospital_MS.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

    }
}
