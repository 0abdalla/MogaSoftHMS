using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.DispensePermission;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hospital_MS.API.Controllers;
[Authorize]
[SwaggerTag("اذن صرف خزينه")]
public class DispensePermissionController(IDispensePermissionService dispensePermissionService) : ApiBaseController
{
    private readonly IDispensePermissionService _dispensePermissionService = dispensePermissionService;

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] DispensePermissionRequest request,CancellationToken cancellationToken)   
    {
        var result = await _dispensePermissionService.CreateAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] PagingFilterModel pagingFilter,CancellationToken cancellationToken)        
    {
        var result = await _dispensePermissionService.GetAllAsync(pagingFilter, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id,CancellationToken cancellationToken)         
    {
        var result = await _dispensePermissionService.GetByIdAsync(id, cancellationToken);
        return Ok(result);
    }
}
