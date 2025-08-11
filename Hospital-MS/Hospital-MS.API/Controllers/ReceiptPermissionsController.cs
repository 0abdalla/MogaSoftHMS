using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.PurchasePermission;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hospital_MS.API.Controllers;

[Authorize]
[SwaggerTag("اذن استلام")]
public class ReceiptPermissionsController(IReceiptPermissionService purchasePermissionService) : ApiBaseController
{
    private readonly IReceiptPermissionService _receiptPermissionService = purchasePermissionService;

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] ReceiptPermissionRequest request, CancellationToken cancellationToken)
    {
        var result = await _receiptPermissionService.CreateAsyncV2(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] PagingFilterModel filter, CancellationToken cancellationToken)
    {
        var result = await _receiptPermissionService.GetAllAsync(filter, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _receiptPermissionService.GetByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ReceiptPermissionRequest request, CancellationToken cancellationToken)
    {
        var result = await _receiptPermissionService.UpdateAsync(id, request, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _receiptPermissionService.DeleteAsync(id, cancellationToken);
        return Ok(result);
    }
}