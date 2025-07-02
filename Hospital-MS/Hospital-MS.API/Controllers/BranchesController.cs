using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Branches;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hospital_MS.API.Controllers;

[Authorize]
[SwaggerTag("الفروع")]
public class BranchesController(IBranchService branchService) : ApiBaseController
{
    private readonly IBranchService _branchService = branchService;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BranchRequest request, CancellationToken cancellationToken)
    {
        var result = await _branchService.CreateAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PagingFilterModel filter, CancellationToken cancellationToken)
    {
        var result = await _branchService.GetAllAsync(filter, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _branchService.GetByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] BranchRequest request, CancellationToken cancellationToken)
    {
        var result = await _branchService.UpdateAsync(id, request, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _branchService.DeleteAsync(id, cancellationToken);
        return Ok(result);
    }
}