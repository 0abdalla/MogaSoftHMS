using Hospital_MS.Core.Contracts.Patients;
using Hospital_MS.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers
{
    [Authorize]
    public class PatientMedicalHistoryController(IPatientHistoryService patientHistoryService) : ApiBaseController
    {
        private readonly IPatientHistoryService _patientHistoryService = patientHistoryService;

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] PatientMedicalHistoryRequest request, CancellationToken cancellationToken)
        {
            var result = await _patientHistoryService.CreateAsync(request, cancellationToken);

            return result.IsSuccess ? Created() : BadRequest(result.Error);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PatientMedicalHistoryRequest request, CancellationToken cancellationToken)
        {
            var result = await _patientHistoryService.UpdateAsync(id, request, cancellationToken);

            return result.IsSuccess ? NoContent() : NotFound(result.Error);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await _patientHistoryService.DeleteAsync(id, cancellationToken);
            return result.IsSuccess ? NoContent() : NotFound(result.Error);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await _patientHistoryService.GetByIdAsync(id, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _patientHistoryService.GetAllAsync(cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatientId(int patientId, CancellationToken cancellationToken)
        {
            var result = await _patientHistoryService.GetByPatientIdAsync(patientId, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }
    }
}
