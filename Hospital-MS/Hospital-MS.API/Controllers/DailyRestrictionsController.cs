using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.DailyRestrictions;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hospital_MS.API.Controllers;

[Authorize]
[SwaggerTag("القيود اليومية")]
public class DailyRestrictionsController(IDailyRestrictionService service) : ApiBaseController
{
    private readonly IDailyRestrictionService _dailyRestrictionService = service;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DailyRestrictionRequest request, CancellationToken cancellationToken)
        => Ok(await _dailyRestrictionService.CreateAsync(request, cancellationToken));

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PagingFilterModel filter, CancellationToken cancellationToken)
        => Ok(await _dailyRestrictionService.GetAllAsync(filter, cancellationToken));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        => Ok(await _dailyRestrictionService.GetByIdAsync(id, cancellationToken));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] DailyRestrictionRequest request, CancellationToken cancellationToken)
        => Ok(await _dailyRestrictionService.UpdateAsync(id, request, cancellationToken));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        => Ok(await _dailyRestrictionService.DeleteAsync(id, cancellationToken));

    [HttpGet("report/{accountId}")]
    public async Task<IActionResult> GetAccountReport(int accountId, [FromQuery] DateOnly fromDate, [FromQuery] DateOnly toDate, CancellationToken cancellationToken)
    {
        var result = await _dailyRestrictionService.GetAccountReportAsync(accountId, fromDate, toDate, cancellationToken);
        return Ok(result);
    }


}