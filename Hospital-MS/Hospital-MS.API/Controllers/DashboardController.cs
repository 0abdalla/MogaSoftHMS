using Hospital_MS.API.Controllers;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers;

[Authorize]
public class DashboardController(IDashboardService dashboardService) : ApiBaseController
{
    private readonly IDashboardService _dashboardService = dashboardService;

    [HttpGet("metrics")]
    public async Task<IActionResult> GetDashboardMetrics([FromQuery] string? month, CancellationToken cancellationToken)
    {
        var metrics = await _dashboardService.GetDashboardMetricsAsync(month, cancellationToken);
        return Ok(metrics);
    }

    [HttpGet("topDoctors")]
    public async Task<IActionResult> GetTopDoctorsMetrics([FromQuery] string? month, CancellationToken cancellationToken)
    {
        var metrics = await _dashboardService.GetTopDoctorsForMonth(month, cancellationToken);
        return Ok(metrics);
    }

    [HttpGet("medicalServices")]
    public async Task<IActionResult> GetTopMedicalServicesMetrics(CancellationToken cancellationToken)
    {
        var metrics = await _dashboardService.GetAppointmentCountsByMedicalServiceAsync(cancellationToken);
        return Ok(metrics);
    }
}