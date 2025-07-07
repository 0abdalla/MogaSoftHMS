using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.MainGroups;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hospital_MS.API.Controllers;
[Authorize]
[SwaggerTag("المجموعات الرئيسية")]
public class MainGroupsController(IMainGroupService mainGroupService) : ApiBaseController
{
    private readonly IMainGroupService _mainGroupService = mainGroupService;

    [HttpGet("")]
    public async Task<IActionResult> GetAllAsync([FromQuery] PagingFilterModel filter, CancellationToken cancellationToken = default)
    {
        var result = await _mainGroupService.GetAllAsync(filter, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var result = await _mainGroupService.GetByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateAsync([FromBody] MainGroupRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _mainGroupService.CreateAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] MainGroupRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _mainGroupService.UpdateAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var result = await _mainGroupService.DeleteAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
