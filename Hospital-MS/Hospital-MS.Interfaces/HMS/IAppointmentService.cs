using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Appointments;
using System.Data;

namespace Hospital_MS.Interfaces.HMS
{
    public interface IAppointmentService
    {
        Task<ErrorResponseModel<AppointmentToReturnResponse>> CreateAsync(CreateAppointmentRequest request, CancellationToken cancellationToken = default);
        PagedResponseModel<List<AppointmentsGroupResponse>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<AppointmentResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<PagedResponseModel<DataTable>> GetCountsAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> UpdateAsync(int id, UpdateAppointmentRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> UpdateStatusAsync(int id, UpdatePatientStatusInEmergencyRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);

        Task<PagedResponseModel<DataTable>> GetStaffAppointmentsAsync(int staffId, PagingFilterModel pagingFilter, CancellationToken cancellationToken = default);

        Task UpdateAppointmentsToCompletedAsync();

        Task<ErrorResponseModel<ClosedShiftResponse>> CloseShiftAsync(CancellationToken cancellationToken = default);

        Task<ErrorResponseModel<AppointmentToReturnResponse>> CreateAsyncV2(CreateAppointmentRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<List<ShiftResponse>>> GetAllShiftsAsync(CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<ShiftResponse>> GetShiftByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
