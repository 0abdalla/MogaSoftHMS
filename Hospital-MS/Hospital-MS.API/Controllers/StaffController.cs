using Hospital_MS.Core.Contracts.Staff;
using Hospital_MS.Core.Services;
using Hospital_MS.Interfaces.HMS;
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
            return Ok(result);
        }

        //[HttpGet("")]
        //public async Task<IActionResult> GetAllStaff(CancellationToken cancellationToken)
        //{
        //    var result = await _staffService.GetAllAsync(cancellationToken);
        //    return Ok(result);
        //}

        //[HttpGet("all")]
        //public async Task<IActionResult> GetFilteredStaff([FromQuery] GetStaffRequest request, CancellationToken cancellationToken)
        //{
        //    var result = await _staffService.GetFilteredStaffAsync(request, cancellationToken);
        //    int count = await _staffService.GetFilteredStaffCountAsync(request, cancellationToken);
        //    return Ok(result);
        //}

        //[HttpGet("counts")]
        //public async Task<IActionResult> GetStaffCounts(CancellationToken cancellationToken)
        //{
        //    var result = await _staffService.GetStaffCountsAsync(cancellationToken);
        //    return Ok(result);
        //}

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStaff(int id, CancellationToken cancellationToken)
        {
            var result = await _staffService.GetByIdAsync(id, cancellationToken);
            return Ok(result);
        }
    }
}
