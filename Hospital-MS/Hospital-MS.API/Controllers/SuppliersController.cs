using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Suppliers;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers;
[Authorize]
public class SuppliersController(ISupplierService supplierService) : ApiBaseController
{
    private readonly ISupplierService _supplierService = supplierService;

    [HttpPost("")]
    public async Task<IActionResult> CreateSupplierAsync([FromBody] SupplierRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _supplierService.CreateSupplierAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSupplierAsync(int id, [FromBody] SupplierRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _supplierService.UpdateSupplierAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSupplierByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var result = await _supplierService.GetSupplierByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
    [HttpGet]
    public async Task<IActionResult> GetAllSuppliersAsync([FromQuery] PagingFilterModel pagingFilter, CancellationToken cancellationToken = default)
    {
        var result = await _supplierService.GetAllSuppliersAsync(pagingFilter, cancellationToken);
        return Ok(result);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSupplierAsync(int id, CancellationToken cancellationToken = default)
    {
        var result = await _supplierService.DeleteSupplierAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
}
