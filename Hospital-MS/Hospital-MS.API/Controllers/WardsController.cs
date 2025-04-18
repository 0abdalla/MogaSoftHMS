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
            return result.IsSuccess
                ? Created()
                : BadRequest(result.Error);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll( CancellationToken cancellationToken)
        {
            var result = await _wardService.GetAllAsync(cancellationToken);
           
            return result.IsSuccess
                ? Ok(result.Value)
                : NotFound(result.Error);
        }

    }
}
