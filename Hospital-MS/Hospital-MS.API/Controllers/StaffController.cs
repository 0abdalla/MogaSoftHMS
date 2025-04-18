using Hospital_MS.Core.Contracts.Staff;
using Hospital_MS.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers
{
    [Authorize]
    public class StaffController(IStaffService staffService) : ApiBaseController
    {
        private readonly IStaffService _staffService = staffService;

        [HttpPost("")]
        public async Task<IActionResult> CreateStaff([FromBody] CreateStaffRequest request, CancellationToken cancellationToken)
        {
            var result = await _staffService.CreateAsync(request, cancellationToken);

            return result.IsSuccess
                ? Created()
                : BadRequest(result.Error);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllStaff(CancellationToken cancellationToken)
        {
            var result = await _staffService.GetAllAsync(cancellationToken);
            
            return result.IsSuccess
                ? Ok(result.Value)
                : NotFound(result.Error);
        }
    }
}
