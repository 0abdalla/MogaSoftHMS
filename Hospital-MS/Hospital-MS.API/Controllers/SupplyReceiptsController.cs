using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.SupplyReceipts;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers;

[Authorize]
public class SupplyReceiptsController(ISupplyReceiptService supplyReceiptService) : ApiBaseController
{
    private readonly ISupplyReceiptService _supplyReceiptService = supplyReceiptService;

    [HttpPost("")]
    public async Task<IActionResult> CreateSupplyReceiptAsync([FromBody] SupplyReceiptRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _supplyReceiptService.CreateSupplyReceiptAsync(request, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetSupplyReceiptByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var result = await _supplyReceiptService.GetSupplyReceiptByIdAsync(id, cancellationToken);

        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateSupplyReceiptAsync(int id, [FromBody] SupplyReceiptRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _supplyReceiptService.UpdateSupplyReceiptAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteSupplyReceiptAsync(int id, CancellationToken cancellationToken = default)
    {
        var result = await _supplyReceiptService.DeleteSupplyReceiptAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllAsync([FromQuery] PagingFilterModel pagingFilter, CancellationToken cancellationToken = default)
    {
        var result = await _supplyReceiptService.GetAllAsync(pagingFilter, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
