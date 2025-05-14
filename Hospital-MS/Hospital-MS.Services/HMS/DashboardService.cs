using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Dashboard;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Hospital_MS.Services.HMS;
public class DashboardService(IUnitOfWork unitOfWork) : IDashboardService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorResponseModel<DashboardMetricsResponse>> GetDashboardMetricsAsync(string? month, CancellationToken cancellationToken = default)
    {
        var totalBeds = await _unitOfWork.Repository<Bed>().CountAsync(cancellationToken);

        var occupiedBeds = await _unitOfWork.Repository<Bed>().CountAsync(
            b => b.Status == BedStatus.NotAvailable || b.Status == BedStatus.UnderMaintenance,
            cancellationToken
        );

        var bedOccupancyRate = totalBeds > 0 ? (decimal)occupiedBeds / totalBeds * 100 : 0;

        var activeDoctors = await _unitOfWork.Repository<Doctor>().CountAsync(
            d => d.Status == StaffStatus.Active,
            cancellationToken
        );

        var currentPatients = await _unitOfWork.Repository<Patient>().CountAsync(cancellationToken);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var thisMonth = DateOnly.FromDateTime(DateTime.UtcNow).Month;

        var appointmentsCount = await _unitOfWork.Repository<Appointment>().CountAsync(
            a => a.AppointmentDate == today,
            cancellationToken
        );

        var completedAppointmentsCount = await _unitOfWork.Repository<Appointment>().CountAsync(
            a => a.AppointmentDate.HasValue && a.AppointmentDate.Value.Month == thisMonth && a.Status == AppointmentStatus.Completed,
            cancellationToken
        );

        var totalAppointmentsCount = await _unitOfWork.Repository<Appointment>().CountAsync(
            a => a.AppointmentDate.HasValue && a.AppointmentDate.Value.Month == thisMonth,
            cancellationToken
        );

        var completedAppointmentsPercentage = totalAppointmentsCount > 0
            ? Math.Round((decimal)completedAppointmentsCount / totalAppointmentsCount * 100, 1)
            : 0;

        DateTime parsedMonth;

        if (string.IsNullOrEmpty(month))
        {
            parsedMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        }

        else if (!DateTime.TryParseExact(month, "MMMM", null, System.Globalization.DateTimeStyles.None, out parsedMonth))
        {
            return ErrorResponseModel<DashboardMetricsResponse>.Failure(
               new Error("برجاء ادخال الشهر بطريقه صحيحه", Status.Failed)
            );
        }

        var specifiedMonthAppointments = await GetDailyAppointmentsForMonth(parsedMonth, cancellationToken);
        var previousMonthAppointments = await GetDailyAppointmentsForMonth(parsedMonth.AddMonths(-1), cancellationToken);

        var response = new DashboardMetricsResponse
        {
            BedOccupancyRate = bedOccupancyRate,
            ActiveDoctors = activeDoctors,
            CurrentPatients = currentPatients,
            AppointmentsCount = appointmentsCount,
            CompletedAppointmentsRate = completedAppointmentsPercentage,
            CurrentMonthAppointments = specifiedMonthAppointments,
            PreviousMonthAppointments = previousMonthAppointments
        };

        return ErrorResponseModel<DashboardMetricsResponse>.Success(GenericErrors.GetSuccess, response);
    }


    private async Task<Dictionary<string, int>> GetDailyAppointmentsForMonth(DateTime date, CancellationToken cancellationToken)
    {
        var startOfMonth = new DateTime(date.Year, date.Month, 1);
        var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

        var dailyAppointments = await _unitOfWork.Repository<Appointment>()
            .GetAll(a => a.AppointmentDate >= DateOnly.FromDateTime(startOfMonth) && a.AppointmentDate <= DateOnly.FromDateTime(endOfMonth))
            .GroupBy(a => a.AppointmentDate)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var dailyCounts = new Dictionary<string, int>();
        for (var day = startOfMonth; day <= endOfMonth; day = day.AddDays(1))
        {
            var count = dailyAppointments.FirstOrDefault(a => a.Date == DateOnly.FromDateTime(day))?.Count ?? 0;
            dailyCounts.Add(day.ToString("d/M"), count);
        }

        return dailyCounts;
    }

    public async Task<List<WeeklyTopDoctorMetrics>> GetTopDoctorsForMonth(string? month, CancellationToken cancellationToken)
    {
        DateTime parsedMonth;
        if (string.IsNullOrEmpty(month))
        {
            parsedMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        }
        else if (!DateTime.TryParseExact(month, "MMMM", null, System.Globalization.DateTimeStyles.None, out parsedMonth))
        {
            return [];
        }

        var startOfMonth = new DateTime(parsedMonth.Year, parsedMonth.Month, 1);
        var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

        var weeks = new List<(DateOnly Start, DateOnly End, string WeekName)>();
        var currentDate = startOfMonth;

        for (int weekNumber = 1; weekNumber <= 3; weekNumber++)
        {
            var weekStart = DateOnly.FromDateTime(currentDate);
            var weekEnd = DateOnly.FromDateTime(currentDate.AddDays(6));
            weeks.Add((weekStart, weekEnd, $"Week {weekNumber}"));
            currentDate = currentDate.AddDays(7);
        }

        var lastWeekStart = DateOnly.FromDateTime(currentDate);
        var lastWeekEnd = DateOnly.FromDateTime(endOfMonth);
        weeks.Add((lastWeekStart, lastWeekEnd, "Week 4"));

        var weeklyTopDoctors = new List<WeeklyTopDoctorMetrics>();

        var doctors = await _unitOfWork.Repository<Doctor>()
            .GetAll()
            .Include(d => d.Admissions)
            .Include(d => d.Appointments)
            .ToListAsync(cancellationToken);

        var topDoctors = doctors.Select(d => new TopDoctorMetric
        {
            DoctorId = d.Id,
            DoctorName = d.FullName,
            WeeklyActivityCounts = weeks.ToDictionary(
                w => w.WeekName,
                w => d.Appointments.Count(a => a.AppointmentDate >= w.Start && a.AppointmentDate <= w.End) +
                     d.Admissions.Count(a => DateOnly.FromDateTime(a.AdmissionDate) >= w.Start &&
                                           DateOnly.FromDateTime(a.AdmissionDate) <= w.End)
            )
        })
        .Select(d => {
            d.TotalActivityCount = d.WeeklyActivityCounts.Values.Sum();
            return d;
        })
        .OrderByDescending(d => d.TotalActivityCount)
        .Take(3)
        .ToList();

        weeklyTopDoctors.Add(new WeeklyTopDoctorMetrics
        {
            StartDate = DateOnly.FromDateTime(startOfMonth),
            EndDate = DateOnly.FromDateTime(endOfMonth),
            TopDoctors = topDoctors
        });

        return weeklyTopDoctors;
    }

    public async Task<Dictionary<string, int>> GetAppointmentCountsByMedicalServiceAsync(CancellationToken cancellationToken = default)
    {
        var startOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

        var appointmentCounts = await _unitOfWork.Repository<Appointment>()
            .GetAll(a => a.AppointmentDate >= DateOnly.FromDateTime(startOfMonth) &&
                         a.AppointmentDate <= DateOnly.FromDateTime(endOfMonth) &&
                         a.MedicalServiceId != null)
            .GroupBy(a => a.MedicalService!.Name)
            .Select(g => new
            {
                ServiceName = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(x => x.Count)
            .Take(12)
            .ToDictionaryAsync(
                x => x.ServiceName,
                x => x.Count,
                cancellationToken
            );

        return appointmentCounts;
    }
}