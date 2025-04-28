using Hospital_MS.Core.Common;
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
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PatientMedicalHistoryRequest request, CancellationToken cancellationToken)
        {
            var result = await _patientHistoryService.UpdateAsync(id, request, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await _patientHistoryService.DeleteAsync(id, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await _patientHistoryService.GetByIdAsync(id, cancellationToken);
            return Ok(result);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll(PagingFilterModel pagingFilter, CancellationToken cancellationToken)
        {
            var result = await _patientHistoryService.GetAllAsync(pagingFilter);
            return Ok(result);
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatientId(int patientId, CancellationToken cancellationToken)
        {
            var result = await _patientHistoryService.GetByPatientIdAsync(patientId, cancellationToken);
            return Ok(result);
            //
        }
    }
}
