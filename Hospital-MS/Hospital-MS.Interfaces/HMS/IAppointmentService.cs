using Hospital_MS.Core.Abstractions;
using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Appointments;
using Hospital_MS.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Services
{
    public interface IAppointmentService
    {
        Task<ErrorResponseModel<string>> CreateAsync(CreateAppointmentRequest request, CancellationToken cancellationToken = default);
        Task<PagedResponseModel<DataTable>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<AppointmentResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<PagedResponseModel<DataTable>> GetCountsAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> UpdateAsync(int id, UpdateAppointmentRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> UpdateStatusAsync(int id, UpdatePatientStatusInEmergencyRequest request, CancellationToken cancellationToken = default);
    }
}
