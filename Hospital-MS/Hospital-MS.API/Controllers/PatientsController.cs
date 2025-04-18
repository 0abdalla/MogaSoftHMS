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
        public async Task<ActionResult<Pagination<IReadOnlyList<PatientResponse>>>> GetPatients([FromQuery] GetPatientsRequest request, CancellationToken cancellationToken)
        {
            var result = await _patientService.GetAllAsync(request, cancellationToken);

            int count = await _patientService.GetPatientsCountAsync(request, cancellationToken);

            return result.IsSuccess
                ? Ok(new Pagination<PatientResponse>(request.PageIndex, request.PageSize, result.Value, count))
                : NotFound(result.Error);
        }

        [HttpGet("counts")]
        public async Task<IActionResult> GetAdmissionsCounts(CancellationToken cancellationToken)
        {
            var count = await _patientService.GetCountsAsync(cancellationToken);

            return Ok(count.Value);
        }

        [HttpPut("status/{id}")]
        public async Task<IActionResult> UpdatePatientStatus(int id, [FromBody] UpdatePatientStatusRequest request, CancellationToken cancellationToken)
        {
            var result = await _patientService.UpdateStatusAsync(id, request, cancellationToken);

            return result.IsSuccess
                ? NoContent()
                : NotFound(result.Error);
        }



    }
}
