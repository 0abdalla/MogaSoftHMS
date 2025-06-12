using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.DebitNotices;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers
{
    [Authorize]
    public class DebitNoticesController(IDebitNoticeService debitNoticeService) : ApiBaseController
    {
        private readonly IDebitNoticeService _debitNoticeService = debitNoticeService;

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] DebitNoticeRequest request, CancellationToken cancellationToken)
        {
            var result = await _debitNoticeService.CreateAsync(request, cancellationToken);
            return Ok(result);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll([FromQuery] PagingFilterModel filter, CancellationToken cancellationToken)
        {
            var result = await _debitNoticeService.GetAllAsync(filter, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await _debitNoticeService.GetByIdAsync(id, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DebitNoticeRequest request, CancellationToken cancellationToken)
        {
            var result = await _debitNoticeService.UpdateAsync(id, request, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await _debitNoticeService.DeleteAsync(id, cancellationToken);
            return Ok(result);
        }
    }
}