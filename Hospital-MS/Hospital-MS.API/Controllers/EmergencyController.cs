using Hospital_MS.Core.Contracts.Emergency;
using Hospital_MS.Core.Enums;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers
{
    [Authorize]
    public class EmergencyController : ApiBaseController
    {
        private readonly IEmergencyService _emergencyService;

        public EmergencyController(IEmergencyService emergencyService)
        {
            _emergencyService = emergencyService;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateEmergencyVisit(
            [FromBody] EmergencyVisitCreateDto request,
            CancellationToken cancellationToken)
        {
            var result = await _emergencyService.CreateEmergencyVisitAsync(request, cancellationToken);
            return Ok(result);
        }

        [HttpPost("triage")]
        public async Task<IActionResult> AddTriage(
            [FromBody] EmergencyTriageDto request,
            CancellationToken cancellationToken)
        {
            var result = await _emergencyService.AddTriageAsync(request, cancellationToken);
            return Ok(result);
        }

        [HttpPost("assign-doctor")]
        public async Task<IActionResult> AssignDoctor(
            [FromBody] EmergencyAssignDoctorDto request,
            CancellationToken cancellationToken)
        {
            var result = await _emergencyService.AssignDoctorAsync(request, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(
            int id,
            [FromQuery] EmergencyStatus newStatus,
            CancellationToken cancellationToken)
        {
            var result = await _emergencyService.UpdateStatusAsync(id, newStatus, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await _emergencyService.GetByIdAsync(id, cancellationToken);
            return Ok(result);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveVisits(CancellationToken cancellationToken)
        {
            var result = await _emergencyService.GetActiveVisitsAsync(cancellationToken);
            return Ok(result);
        }
    }
}
