using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Interfaces.HMS;
public interface IDashboardService
{
    Task<ErrorResponseModel<DashboardMetricsResponse>> GetDashboardMetricsAsync(string? month = null, CancellationToken cancellationToken = default);
    Task<List<WeeklyTopDoctorMetrics>> GetTopDoctorsForMonth(string? month = null, CancellationToken cancellationToken = default);
    Task<Dictionary<string, int>> GetAppointmentCountsByMedicalServiceAsync(CancellationToken cancellationToken = default);
}
