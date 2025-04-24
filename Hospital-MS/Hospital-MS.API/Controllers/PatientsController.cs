using Hospital_MS.Core.Contracts.Patients;
using Hospital_MS.Core.Helpers;
using Hospital_MS.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers
{
    [Authorize]
    public class PatientsController(IPatientService patientService) : ApiBaseController
    {
        private readonly IPatientService _patientService = patientService;

        [HttpGet("")]
        public async Task<IActionResult> GetPatients([FromQuery] GetPatientsRequest request, CancellationToken cancellationToken)
        {
            var result = await _patientService.GetAllAsync(request, cancellationToken);
            int count = await _patientService.GetPatientsCountAsync(request, cancellationToken);
            return Ok(result);
        }

        [HttpGet("counts")]
        public async Task<IActionResult> GetAdmissionsCounts(CancellationToken cancellationToken)
        {
            var count = await _patientService.GetCountsAsync(cancellationToken);
            return Ok(count);
        }

        [HttpPut("status/{id}")]
        public async Task<IActionResult> UpdatePatientStatus(int id, [FromBody] UpdatePatientStatusRequest request, CancellationToken cancellationToken)
        {
            var result = await _patientService.UpdateStatusAsync(id, request, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientById(int id, CancellationToken cancellationToken)
        {
            var result = await _patientService.GetByIdAsync(id, cancellationToken);
            return Ok(result);
        }

    }
}
