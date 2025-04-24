using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Appointments;
using Hospital_MS.Core.Helpers;
using Hospital_MS.Core.Services;
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

        [HttpGet("")]
        public async Task<IActionResult> GetAppointments([FromQuery] GetAppointmentsRequest request, CancellationToken cancellationToken)
        {
            var result = await _appointmentService.GetAllAsync(request, cancellationToken);
            int count = await _appointmentService.GetAppointmentsCountAsync(request, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointment(int id, CancellationToken cancellationToken)
        {
            var result = await _appointmentService.GetByIdAsync(id, cancellationToken);
            return Ok(result);
        }

        [HttpGet("counts")]
        public async Task<IActionResult> GetAppointmentsCounts(CancellationToken cancellationToken)
        {
            var count = await _appointmentService.GetCountsAsync(cancellationToken);
            return Ok(count);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] UpdateAppointmentRequest request, CancellationToken cancellationToken)
        {
            var result = await _appointmentService.UpdateAsync(id, request, cancellationToken);
            return Ok(result);
        }

        [HttpPut("emergency/{id}")]
        public async Task<IActionResult> UpdatePatientStatusInEmergency(int id,[FromBody] UpdatePatientStatusInEmergencyRequest request,CancellationToken cancellationToken)
        {
            var result = await _appointmentService.UpdateStatusAsync(id, request, cancellationToken);
            return Ok(result);
        }

    }
}
