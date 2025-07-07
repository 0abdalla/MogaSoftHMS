using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.FiscalYears;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hospital_MS.API.Controllers;
[Authorize]
[SwaggerTag("السنه الماليه")]
public class FiscalYearsController(IFiscalYearService fiscalYearService) : ApiBaseController
{
    private readonly IFiscalYearService _fiscalYearService = fiscalYearService;

    [HttpPost("")]
    public async Task<IActionResult> CreateAsync([FromBody] FiscalYearRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _fiscalYearService.CreateAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var result = await _fiscalYearService.GetByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllAsync([FromQuery] PagingFilterModel filter, CancellationToken cancellationToken = default)
    {
        var result = await _fiscalYearService.GetAllAsync(filter, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
