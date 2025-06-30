using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.AdditionNotifications;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers;

[Authorize] 
public class AdditionNotificationsController(IAdditionNotificationService additionNotificationService) : ApiBaseController
{
    private readonly IAdditionNotificationService _additionNotificationService = additionNotificationService;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PagingFilterModel pagingFilter, CancellationToken cancellationToken)
    {
        var result = await _additionNotificationService.GetAllAsync(pagingFilter, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _additionNotificationService.GetByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AdditionNotificationRequest request, CancellationToken cancellationToken)
    {
        var result = await _additionNotificationService.CreateAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] AdditionNotificationRequest request, CancellationToken cancellationToken)
    {
        var result = await _additionNotificationService.UpdateAsync(id, request, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _additionNotificationService.DeleteAsync(id, cancellationToken);
        return Ok(result);
    }
}