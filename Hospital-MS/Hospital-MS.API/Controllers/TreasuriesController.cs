using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Treasuries;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers;

[Authorize]
public class TreasuriesController(ITreasuryService treasuryService) : ApiBaseController
{
    private readonly ITreasuryService _treasuryService = treasuryService;

    [HttpPost("")]
    public async Task<IActionResult> CreateTreasury([FromBody] TreasuryRequest request, CancellationToken cancellationToken)
    {
        var result = await _treasuryService.CreateTreasuryAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetTreasuries([FromQuery] PagingFilterModel pagingFilter, CancellationToken cancellationToken)
    {
        var result = await _treasuryService.GetTreasuriesAsync(pagingFilter, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTreasuryById(int id, CancellationToken cancellationToken)
    {
        var result = await _treasuryService.GetTreasuryByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTreasury(int id, [FromBody] TreasuryRequest request, CancellationToken cancellationToken)
    {
        var result = await _treasuryService.UpdateTreasuryAsync(id, request, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTreasury(int id, CancellationToken cancellationToken)
    {
        var result = await _treasuryService.DeleteTreasuryAsync(id, cancellationToken);
        return Ok(result);
    }


    [HttpPost("assign-treasury-to-staff/{treasuryId}/{staffId}")]
    [Authorize(Roles = "SystemAdmin")]
    public async Task<IActionResult> AssignTreasury(int treasuryId, int staffId, CancellationToken cancellationToken)
    {
        var result = await _treasuryService.AssignTreasuryToStaffAsync(staffId, treasuryId, cancellationToken);
        return Ok(result);
    }

    [HttpGet("transactions/{treasuryId}")]
    public async Task<IActionResult> GetTreasuryTransactions(int treasuryId, DateOnly fromDate, DateOnly toDate, CancellationToken cancellationToken)
    {
        var result = await _treasuryService.GetTreasuryTransactionsAsync(treasuryId, fromDate, toDate, cancellationToken);
        return Ok(result);
    }

    [HttpGet("Movement/{id}")]
    public async Task<IActionResult> GetTreasuryMovementById(int id, CancellationToken cancellationToken)
    {
        var result = await _treasuryService.GetTreasuryMovementByIdAsyncV1(id, cancellationToken);
        return Ok(result);
    }

    [HttpPut("Movement/{id}/Enable")]
    public async Task<IActionResult> EnableTreasuryMovement(int id, CancellationToken cancellationToken)
    {
        var result = await _treasuryService.EnableTreasuryMovementAsync(id, cancellationToken);
        return Ok(result);
    }


    [HttpPut("Movement/{treasuryId}/Disable")]
    public async Task<IActionResult> DisableTreasuryMovement(int treasuryId, CancellationToken cancellationToken)
    {
        var result = await _treasuryService.DisableTreasuryMovementAsync(treasuryId, cancellationToken);
        return Ok(result);
    }

    [HttpGet("AllMovements")]
    public async Task<IActionResult> GetAllMovements(CancellationToken cancellationToken)
    {
        var result = await _treasuryService.GetAllMovementsAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("Movement/Enabled")]
    public async Task<IActionResult> GetEnabledTreasuries(CancellationToken cancellationToken)
    {
        var result = await _treasuryService.GetEnabledTreasuriesMovementsAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("Movement/Disabled")]
    public async Task<IActionResult> GetDisabledTreasuries(CancellationToken cancellationToken)
    {
        var result = await _treasuryService.GetDisabledTreasuriesMovementsAsync(cancellationToken);
        return Ok(result);
    }
}
