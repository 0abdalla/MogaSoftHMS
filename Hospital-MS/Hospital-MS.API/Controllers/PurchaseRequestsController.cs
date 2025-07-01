using Hospital_MS.API.Controllers;
using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.PurchaseRequests;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

[Authorize]
[SwaggerTag("طلب الشراء")]
public class PurchaseRequestsController(IPurchaseRequestService service) : ApiBaseController
{
    private readonly IPurchaseRequestService _service = service;

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] PurchaseRequestRequest request, CancellationToken cancellationToken)
        => Ok(await _service.CreateAsync(request, cancellationToken));

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] PagingFilterModel filter, CancellationToken cancellationToken)
        => Ok(await _service.GetAllAsync(filter, cancellationToken));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        => Ok(await _service.GetByIdAsync(id, cancellationToken));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] PurchaseRequestRequest request, CancellationToken cancellationToken)
        => Ok(await _service.UpdateAsync(id, request, cancellationToken));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        => Ok(await _service.DeleteAsync(id, cancellationToken));

    [HttpPut("{id}/approve")]
    public async Task<IActionResult> Approve(int id, CancellationToken cancellationToken)
        => Ok(await _service.ApprovePurchaseRequestAsync(id, cancellationToken));

    [HttpGet("approved")]
    public async Task<IActionResult> GetAllApproved([FromQuery] PagingFilterModel filter, CancellationToken cancellationToken)
        => Ok(await _service.GetAllApprovedAsync(filter, cancellationToken));
}