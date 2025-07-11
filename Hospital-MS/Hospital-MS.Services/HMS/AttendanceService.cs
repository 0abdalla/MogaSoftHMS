using CsvHelper;
using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Attendance;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.Common;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Globalization;

namespace Hospital_MS.Services.HMS;
public class AttendanceService(IUnitOfWork unitOfWork, ISQLHelper sQLHelper) : IAttendanceService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ISQLHelper _sQLHelper = sQLHelper;

    public async Task<ErrorResponseModel<string>> ApproveAttendanceAsync(List<int> attendanceIds, CancellationToken cancellationToken = default)
    {
        try
        {
            var attendances = await _unitOfWork.Repository<Attendance>()
                .GetAll(x => attendanceIds.Contains(x.Id) && x.Status == AttendanceStatus.PendingApproval)
                .ToListAsync(cancellationToken);

            if (!attendances.Any())
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            foreach (var att in attendances)
            {
                att.Status = AttendanceStatus.Approved;
                _unitOfWork.Repository<Attendance>().Update(att);
            }

            await _unitOfWork.CompleteAsync(cancellationToken);


            return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess);
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<string>> EditAttendanceAsync(int id, EditAttendanceRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var attendance = await _unitOfWork.Repository<Attendance>().GetByIdAsync(id, cancellationToken);
            if (attendance == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            if (attendance.Status == AttendanceStatus.Approved)
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed, "لا يمكن تعديل سجل حضور معتمد.");

            attendance.InTime = request.InTime;
            attendance.OutTime = request.OutTime;
            attendance.Notes = request.Notes;
            attendance.Status = AttendanceStatus.PendingApproval;

            _unitOfWork.Repository<Attendance>().Update(attendance);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess);
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<PagedResponseModel<DataTable>> GetAllAsync(PagingFilterModel filter, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _unitOfWork.Repository<Attendance>().GetAll();

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                query = query.Where(x => x.Notes.Contains(filter.SearchText));
            }

            if (filter.FilterList != null)
            {
                var status = filter.FilterList.FirstOrDefault(f => f.CategoryName == "Status")?.ItemValue;
                if (!string.IsNullOrEmpty(status) && Enum.TryParse<AttendanceStatus>(status, out var statusEnum))
                {
                    query = query.Where(x => x.Status == statusEnum);
                }

                var dateFilter = filter.FilterList.FirstOrDefault(f => f.CategoryName == "Date");
                if (dateFilter != null)
                {
                    if (dateFilter.FromDate.HasValue)
                        query = query.Where(x => x.Date >= DateOnly.FromDateTime(dateFilter.FromDate.Value));
                    if (dateFilter.ToDate.HasValue)
                        query = query.Where(x => x.Date <= DateOnly.FromDateTime(dateFilter.ToDate.Value));
                }
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var attendances = await query
                .OrderByDescending(x => x.Date)
                .Skip((filter.CurrentPage - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync(cancellationToken);

            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("StaffId", typeof(int));
            dt.Columns.Add("Date", typeof(DateOnly));
            dt.Columns.Add("InTime", typeof(TimeOnly));
            dt.Columns.Add("OutTime", typeof(TimeOnly));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("Notes", typeof(string));
            dt.Columns.Add("TotalCount", typeof(int));

            foreach (var att in attendances)
            {
                dt.Rows.Add(
                    att.Id,
                    att.StaffId,
                    att.Date,
                    att.InTime,
                    att.OutTime,
                    att.Status.ToString(),
                    att.Notes,
                    totalCount
                );
            }

            return PagedResponseModel<DataTable>.Success(GenericErrors.GetSuccess, totalCount, dt);
        }
        catch (Exception)
        {
            return PagedResponseModel<DataTable>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<string>> ImportCsvAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        try
        {
            if (file == null || file.Length == 0)
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed, "لم يتم رفع ملف.");

            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<AttendanceCsvRow>().ToList();
            var attendances = new List<Attendance>();

            foreach (var row in records)
            {
                attendances.Add(new Attendance
                {
                    StaffId = row.StaffId,
                    Date = DateOnly.Parse(row.Date),
                    InTime = TimeOnly.TryParse(row.InTime, out var inTime) ? inTime : null,
                    OutTime = TimeOnly.TryParse(row.OutTime, out var outTime) ? outTime : null,
                    Status = AttendanceStatus.PendingEdit,
                    Notes = row.Notes
                });
            }

            await _unitOfWork.Repository<Attendance>().AddRangeAsync(attendances, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess);
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<PagedResponseModel<DataTable>> GetAllAttendanceSalariesAsync(PagingFilterModel filter)
    {
        try
        {
            var searchText = filter.FilterList?.FirstOrDefault(i => i.CategoryName == "SearchText")?.ItemValue;

            var parameters = new[]
            {
                new SqlParameter("@SearchText", searchText ?? (object)DBNull.Value),
                new SqlParameter("@CurrentPage", filter.CurrentPage),
                new SqlParameter("@PageSize", filter.PageSize)
            };

            var dt = await _sQLHelper.ExecuteDataTableAsync("[finance].[SP_GetAllAttendanceSalaries]", parameters);

            int totalCount = dt.Rows.Count > 0 && dt.Columns.Contains("TotalCount")
                ? dt.Rows[0].Field<int?>("TotalCount") ?? 0
                : dt.Rows.Count;

            return PagedResponseModel<DataTable>.Success(GenericErrors.GetSuccess, totalCount, dt);
        }
        catch (Exception)
        {
            return PagedResponseModel<DataTable>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<string>> AddAttendaceSalariesAsync(List<AttendanceSalary> Model, CancellationToken cancellationToken = default)
    {
        try
        {
            await _unitOfWork.Repository<AttendanceSalary>().AddRangeAsync(Model, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess);
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    private class AttendanceCsvRow
    {
        public int StaffId { get; set; }
        public string Date { get; set; } = string.Empty;
        public string InTime { get; set; } = string.Empty;
        public string OutTime { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }
}