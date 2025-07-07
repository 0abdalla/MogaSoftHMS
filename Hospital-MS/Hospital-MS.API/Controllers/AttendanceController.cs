using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Attendance;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Hospital_MS.API.Controllers
{
    [Authorize]
    public class AttendanceController(IAttendanceService attendanceService) : ApiBaseController
    {
        private readonly IAttendanceService _attendanceService = attendanceService;

        [HttpGet("")]
        public async Task<ActionResult<PagedResponseModel<DataTable>>> GetAll([FromQuery] PagingFilterModel filter, CancellationToken cancellationToken)
            => await _attendanceService.GetAllAsync(filter, cancellationToken);

        [HttpPost("import")]
        //[Consumes("multipart/form-data")]
        public async Task<IActionResult> ImportAttendance([FromForm] CreateAttendanceRequest request, CancellationToken cancellationToken)
            => Ok(await _attendanceService.ImportCsvAsync(request.File, cancellationToken));

        [HttpPut("{id}")]
        public async Task<IActionResult> EditAttendance(int id, [FromBody] EditAttendanceRequest request, CancellationToken cancellationToken)
            => Ok(await _attendanceService.EditAttendanceAsync(id, request, cancellationToken));

        [HttpPost("approve")]
        public async Task<IActionResult> ApproveAttendance([FromBody] List<int> attendanceIds, CancellationToken cancellationToken)
            => Ok(await _attendanceService.ApproveAttendanceAsync(attendanceIds, cancellationToken));

        [HttpPost("AddAttendaceSalaries")]
        public async Task<IActionResult> AddAttendaceSalariesAsync(List<AttendanceSalary> Model, CancellationToken cancellationToken = default)
            => Ok(await _attendanceService.AddAttendaceSalariesAsync(Model, cancellationToken));

        [HttpPost("GetAllAttendanceSalaries")]
        public async Task<IActionResult> GetAllAttendanceSalariesAsync(PagingFilterModel filter)
            => Ok(await _attendanceService.GetAllAttendanceSalariesAsync(filter));
    }
}