using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Patients;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers
{
    public class PatientsController(IPatientService patientService) : ApiBaseController
    {
        private readonly IPatientService _patientService = patientService;

        [HttpPost("")]
        public async Task<IActionResult> GetPatients(PagingFilterModel pagingFilter, CancellationToken cancellationToken)
        {
            var result = await _patientService.GetAllAsync(pagingFilter, cancellationToken);
            return Ok(result);
        }

        [HttpGet("counts")]
        public async Task<IActionResult> GetAdmissionsCounts([FromQuery]PagingFilterModel pagingFilter, CancellationToken cancellationToken)
        {
            var count = await _patientService.GetCountsAsync(pagingFilter, cancellationToken);
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

        [HttpGet("medical-history/{id}")]
        public async Task<IActionResult> GetPatientMedicalHistory(int id, CancellationToken cancellationToken)
        {
            var result = await _patientService.GetPatientAdmissionsByIdAsync(id, cancellationToken);
            return Ok(result);
        }

    }
}
