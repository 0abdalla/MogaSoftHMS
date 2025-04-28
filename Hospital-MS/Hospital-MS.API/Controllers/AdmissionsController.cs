using Hospital_MS.Core.Contracts.Admissions;
using Hospital_MS.Core.Contracts.Patients;
using Hospital_MS.Core.Helpers;
using Hospital_MS.Core.Services;
using Hospital_MS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers
{
    [Authorize]
    public class AdmissionsController(IAdmissionService admissionService) : ApiBaseController
    {
        private readonly IAdmissionService _admissionService = admissionService;

        [HttpPost("")]
        public async Task<IActionResult> CreateAdmission([FromBody] CreateAdmissionRequest request)
        {
            var result = await _admissionService.CreateAsync(request);
            return Ok(result);
        }

        [HttpGet("{patientId}")]
        public async Task<IActionResult> GetPatientAdmission(int patientId, CancellationToken cancellationToken)
        {
            var result = await _admissionService.GetByIdAsync(patientId, cancellationToken);
            return Ok(result);
        }
    }
}
