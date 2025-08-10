using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Items;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers;
[Authorize]
public class ItemsController(IItemService itemService) : ApiBaseController
{
    private readonly IItemService _itemService = itemService;

    [HttpPost]
    public async Task<IActionResult> CreateItem([FromBody] ItemRequest request, CancellationToken cancellationToken)
    {
        var result = await _itemService.CreateItemAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetItems([FromQuery] PagingFilterModel pagingFilter, CancellationToken cancellationToken)
    {
        var result = await _itemService.GetItemsAsync(pagingFilter, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetItemById(int id, CancellationToken cancellationToken)
    {
        var result = await _itemService.GetItemByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateItem(int id, [FromBody] ItemRequest request, CancellationToken cancellationToken)
    {
        var result = await _itemService.UpdateItemAsync(id, request, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(int id, CancellationToken cancellationToken)
    {
        var result = await _itemService.DeleteItemAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpGet("movement/{id}")]
    public async Task<IActionResult> GetItemMovement(int id, [FromQuery] GetItemMovementRequest request, CancellationToken cancellationToken)
    {
        var result = await _itemService.GetItemMovementAsyncV2(id, request, cancellationToken);
        return Ok(result);
    }
}
