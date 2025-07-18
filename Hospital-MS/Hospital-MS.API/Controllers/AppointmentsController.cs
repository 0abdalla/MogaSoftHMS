using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Appointments;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Services.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers
{
    [Authorize]
    public class AppointmentsController(IAppointmentService appointmentService) : ApiBaseController
    {
        private readonly IAppointmentService _appointmentService = appointmentService;

        [HttpPost("")]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentRequest request, CancellationToken cancellationToken)
        {
            var result = await _appointmentService.CreateAsync(request, cancellationToken);
            return Ok(result);
        }

        [HttpPost("GetAppointments")]
        public IActionResult GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default)
        {
            var result = _appointmentService.GetAllAsync(pagingFilter, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointment(int id, CancellationToken cancellationToken)
        {
            var result = await _appointmentService.GetByIdAsync(id, cancellationToken);
            return Ok(result);
        }

        [HttpPost("GetAppointmentsCounts")]
        public async Task<IActionResult> GetAppointmentsCounts(PagingFilterModel pagingFilter, CancellationToken cancellationToken)
        {
            var count = await _appointmentService.GetCountsAsync(pagingFilter, cancellationToken);
            return Ok(count);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] UpdateAppointmentRequest request, CancellationToken cancellationToken)
        {
            var result = await _appointmentService.UpdateAsync(id, request, cancellationToken);
            return Ok(result);
        }

        [HttpPut("emergency/{id}")]
        public async Task<IActionResult> UpdatePatientStatusInEmergency(int id, [FromBody] UpdatePatientStatusInEmergencyRequest request, CancellationToken cancellationToken)
        {
            var result = await _appointmentService.UpdateStatusAsync(id, request, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("")]
        public async Task<IActionResult> DeleteAppointment(int id, CancellationToken cancellationToken)
        {
            var result = await _appointmentService.DeleteAsync(id, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{staffId}/appointments")]
        public async Task<IActionResult> GetStaffAppointments(int staffId, [FromQuery] PagingFilterModel pagingFilter, CancellationToken cancellationToken)
        {
            var result = await _appointmentService.GetStaffAppointmentsAsync(staffId, pagingFilter, cancellationToken);
            return Ok(result);
        }
    }
}
