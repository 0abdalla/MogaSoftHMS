using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.StoreTypes;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hospital_MS.API.Controllers;

[Authorize]
[SwaggerTag("أنواع المخازن")]
public class StoreTypesController(IStoreTypeService storeTypeService) : ApiBaseController
{
    private readonly IStoreTypeService _storeTypeService = storeTypeService;

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] StoreTypeRequest request, CancellationToken cancellationToken)
    {
        var result = await _storeTypeService.CreateAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] PagingFilterModel filter, CancellationToken cancellationToken)
    {
        var result = await _storeTypeService.GetAllAsync(filter, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _storeTypeService.GetByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] StoreTypeRequest request, CancellationToken cancellationToken)
    {
        var result = await _storeTypeService.UpdateAsync(id, request, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _storeTypeService.DeleteAsync(id, cancellationToken);
        return Ok(result);
    }
}
