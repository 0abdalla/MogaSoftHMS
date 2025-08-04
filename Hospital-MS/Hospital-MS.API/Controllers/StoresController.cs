using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Stores;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers;

[Authorize]
public class StoresController(IStoreService storeService) : ApiBaseController
{
    private readonly IStoreService _storeService = storeService;

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] CreateStoreRequest request, CancellationToken cancellationToken)
    {
        var result = await _storeService.CreateAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] PagingFilterModel filter, CancellationToken cancellationToken)
    {
        var result = await _storeService.GetAllAsync(filter, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _storeService.GetByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateStoreRequest request, CancellationToken cancellationToken)
    {
        var result = await _storeService.UpdateAsync(id, request, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _storeService.DeleteAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpGet("movements/{storeId}")]
    public async Task<IActionResult> GetStoreMovements(int storeId, [FromQuery] GetStoresMovementsRequest request, CancellationToken cancellationToken)
    {
        var result = await _storeService.GetStoreMovementsAsync(storeId, request, cancellationToken);
        return Ok(result);
    }
}