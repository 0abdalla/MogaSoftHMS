using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.PurchaseOrder;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers;

[Authorize]
public class PurchaseOrdersController(IPurchaseOrderService purchaseOrderService) : ApiBaseController
{
    private readonly IPurchaseOrderService _purchaseOrderService = purchaseOrderService;

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] PurchaseOrderRequest request, CancellationToken cancellationToken)
    {
        var result = await _purchaseOrderService.CreateAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] PagingFilterModel filter, CancellationToken cancellationToken)
    {
        var result = await _purchaseOrderService.GetAllAsync(filter, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _purchaseOrderService.GetByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] PurchaseOrderRequest request, CancellationToken cancellationToken)
    {
        var result = await _purchaseOrderService.UpdateAsync(id, request, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _purchaseOrderService.DeleteAsync(id, cancellationToken);
        return Ok(result);
    }
}