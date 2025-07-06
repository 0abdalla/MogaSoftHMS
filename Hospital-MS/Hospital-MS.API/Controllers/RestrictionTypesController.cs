using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.RestrictionTypes;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hospital_MS.API.Controllers;
[Authorize]
[SwaggerTag("انواع القيود اليومية")]
public class RestrictionTypesController(IRestrictionTypeService service) : ApiBaseController
{
    private readonly IRestrictionTypeService _service = service;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RestrictionTypeRequest request, CancellationToken cancellationToken)
        => Ok(await _service.CreateAsync(request, cancellationToken));

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PagingFilterModel filter, CancellationToken cancellationToken)
        => Ok(await _service.GetAllAsync(filter, cancellationToken));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        => Ok(await _service.GetByIdAsync(id, cancellationToken));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] RestrictionTypeRequest request, CancellationToken cancellationToken)
        => Ok(await _service.UpdateAsync(id, request, cancellationToken));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        => Ok(await _service.DeleteAsync(id, cancellationToken));
}