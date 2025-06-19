using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Attendance;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Interfaces.HMS;
public interface IAttendanceService
{
    Task<ErrorResponseModel<string>> ImportCsvAsync(IFormFile file, CancellationToken cancellationToken = default);
    Task<PagedResponseModel<DataTable>> GetAllAsync(PagingFilterModel filter, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> EditAttendanceAsync(int id, EditAttendanceRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> ApproveAttendanceAsync(List<int> attendanceIds, CancellationToken cancellationToken = default);
}