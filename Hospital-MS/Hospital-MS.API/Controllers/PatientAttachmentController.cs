using Hospital_MS.Core.Contracts.Patients;
using Hospital_MS.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers
{
    [Authorize]
    public class PatientAttachmentController(IPatientAttachmentService patientAttachmentService) : ApiBaseController
    {
        private readonly IPatientAttachmentService _patientAttachmentService = patientAttachmentService;

        [HttpPost("{patientId}")]
        public async Task<IActionResult> CreateAsync(int patientId, [FromForm] PatientAttachmentRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _patientAttachmentService.CreateAsync(patientId, request, cancellationToken);

            return result.IsSuccess
                ? Created()
                : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var result = await _patientAttachmentService.DeleteAsync(id, cancellationToken);
            return result.IsSuccess
                ? NoContent()
                : BadRequest(result);
        }

        [HttpGet("{patientId}")]
        public async Task<IActionResult> GetAllAsync(int patientId, CancellationToken cancellationToken = default)
        {
            var result = await _patientAttachmentService.GetAllAsync(patientId, cancellationToken);
            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result);
        }
    }
}
