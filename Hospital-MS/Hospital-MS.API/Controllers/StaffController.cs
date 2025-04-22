using Hospital_MS.Core.Contracts.Staff;
using Hospital_MS.Core.Helpers;
using Hospital_MS.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers
{
    [Authorize]
    public class StaffController(IStaffService staffService) : ApiBaseController
    {
        private readonly IStaffService _staffService = staffService;

        [HttpPost("")]
        public async Task<IActionResult> CreateStaff([FromForm] CreateStaffRequest request, CancellationToken cancellationToken)
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

        [HttpGet("all")]
        public async Task<ActionResult<Pagination<IReadOnlyList<StaffResponse>>>> GetFilteredStaff([FromQuery] GetStaffRequest request, CancellationToken cancellationToken)
        {
            var result = await _staffService.GetFilteredStaffAsync(request, cancellationToken);

            int count = await _staffService.GetFilteredStaffCountAsync(request, cancellationToken);

            return result.IsSuccess
                ? Ok(new Pagination<StaffResponse>(request.PageIndex, request.PageSize, result.Value, count))
                : NotFound(result.Error);
        }

        [HttpGet("counts")]
        public async Task<IActionResult> GetStaffCounts(CancellationToken cancellationToken)
        {
            var result = await _staffService.GetStaffCountsAsync(cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : NotFound(result.Error);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStaff(int id, CancellationToken cancellationToken)
        {
            var result = await _staffService.GetByIdAsync(id, cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : NotFound(result.Error);
        }
    }
}
