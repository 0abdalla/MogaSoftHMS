using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.ItemUnits;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers;
[Authorize]
public class ItemUnitsController(IItemUnitService itemUnitService) : ApiBaseController
{
    private readonly IItemUnitService _itemUnitService = itemUnitService;

    [HttpPost("")]
    public async Task<IActionResult> CreateAsync([FromBody] ItemUnitRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _itemUnitService.CreateAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] ItemUnitRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _itemUnitService.UpdateAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var result = await _itemUnitService.DeleteAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var result = await _itemUnitService.GetByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllAsync([FromQuery] PagingFilterModel filter, CancellationToken cancellationToken = default)
    {
        var result = await _itemUnitService.GetAllAsync(filter, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
