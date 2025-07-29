using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Disbursement;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hospital_MS.API.Controllers
{
    [Authorize]
    [SwaggerTag("طلب صرف")]
    public class DisbursementRequestController(IDisbursementRequestService disbursementRequestService) : ApiBaseController
    {
        private readonly IDisbursementRequestService _disbursementRequestService = disbursementRequestService;

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] DisbursementReq request, CancellationToken cancellationToken)
        {
            var result = await _disbursementRequestService.CreateAsync(request, cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] DisbursementReq request, CancellationToken cancellationToken)
        {
            var result = await _disbursementRequestService.UpdateAsync(id, request, cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _disbursementRequestService.DeleteAsync(id, cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _disbursementRequestService.GetByIdAsync(id, cancellationToken);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PagingFilterModel filter, CancellationToken cancellationToken)
        {
            var result = await _disbursementRequestService.GetAllAsync(filter, cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _disbursementRequestService.ApproveDisbursementRequestAsync(id, cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("approved")]
        public async Task<IActionResult> GetAllApprovedAsync(CancellationToken cancellationToken)
        {
            var result = await _disbursementRequestService.GetAllApprovedAsync(cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}