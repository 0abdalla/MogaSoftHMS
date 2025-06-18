using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Treasuries;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
}
