using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Appointments;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Errors;
using Hospital_MS.Core.Helpers;
using Hospital_MS.Core.Models;
using Hospital_MS.Core.Services;
using Hospital_MS.Interfaces.Common;
using Hospital_MS.Interfaces.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Hospital_MS.Services.HMS
{
    public class AppointmentService(IUnitOfWork unitOfWork, ISQLHelper sQLHelper) : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ISQLHelper _sQLHelper = sQLHelper;

        public async Task<ErrorResponseModel<string>> CreateAsync(CreateAppointmentRequest request, CancellationToken cancellationToken = default)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                if (!Enum.TryParse<AppointmentType>(request.AppointmentType, true, out var appointmentType))
                    return ErrorResponseModel<string>.Failure(GenericErrors.InvalidType);

                var patient = new Patient
                {
                    FullName = ArabicNormalizer.NormalizeArabic(request.PatientName),
                    Phone = request.PatientPhone,
                    InsuranceCompanyId = request.InsuranceCompanyId,
                    InsuranceCategoryId = request.InsuranceCategoryId,
                    Status = PatientStatus.Outpatient,
                };

                await _unitOfWork.Repository<Patient>().AddAsync(patient, cancellationToken);
                await _unitOfWork.CompleteAsync(cancellationToken);

                var appointment = new Appointment
                {
                    PatientId = patient.Id,
                    DoctorId = request.DoctorId,
                    AppointmentDateTime = request.AppointmentDate,
                    PaymentMethod = request.PaymentMethod,
                    Type = appointmentType,
                    ClinicId = request.ClinicId,

                    EmergencyLevel = request.EmergencyLevel,
                    CompanionName = request.CompanionName,
                    CompanionNationalId = request.CompanionNationalId,
                    CompanionPhone = request.CompanionPhone
                };

                await _unitOfWork.Repository<Appointment>().AddAsync(appointment, cancellationToken);

                await _unitOfWork.CompleteAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<PagedResponseModel<DataTable>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default)
        {
            try
            {
                var Params = new SqlParameter[4];
                var Type = pagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Type")?.ItemValue;
                Params[0] = new SqlParameter("@SearchText", pagingFilter.SearchText ?? (object)DBNull.Value);
                Params[1] = new SqlParameter("@Type", Type ?? (object)DBNull.Value);
                Params[2] = new SqlParameter("@CurrentPage", pagingFilter.CurrentPage);
                Params[3] = new SqlParameter("@PageSize", pagingFilter.PageSize);
                var dt = await _sQLHelper.ExecuteDataTableAsync("dbo.SP_GetAllAppointments", Params);
                int totalCount = 0;
                if (dt.Rows.Count > 0)
                {
                    int.TryParse(dt.Rows[0]["TotalCount"]?.ToString(), out totalCount);
                }

                return PagedResponseModel<DataTable>.Success(GenericErrors.GetSuccess, totalCount, dt);
            }
            catch (Exception)
            {
                return PagedResponseModel<DataTable>.Failure(GenericErrors.TransFailed);
            }

        }

        public async Task<ErrorResponseModel<AppointmentResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var appointment = await _unitOfWork.Repository<Appointment>().GetAll(i => i.Id == id).Include(x => x.CreatedBy).Include(x => x.UpdatedBy)
                .Include(x => x.Patient).Include(x => x.Doctor).Include(x => x.Patient.InsuranceCompany).Include(x => x.Patient.InsuranceCategory).Include(x => x.Clinic).FirstOrDefaultAsync();

            if (appointment == null)
                return ErrorResponseModel<AppointmentResponse>.Failure(GenericErrors.NotFound);

            var response = new AppointmentResponse
            {
                Id = appointment.Id,
                PatientName = appointment.Patient.FullName,
                DoctorName = appointment?.Doctor?.FullName,
                AppointmentDate = appointment.AppointmentDateTime,
                PaymentMethod = appointment.PaymentMethod,
                Status = appointment.Status.ToString(),
                Type = appointment.Type.ToString(),
                DoctorId = appointment.DoctorId,
                PatientId = appointment.PatientId,
                CreatedOn = appointment.CreatedOn,
                UpdatedOn = appointment.UpdatedOn,
                CreatedBy = $"{appointment.CreatedBy.FirstName} {appointment.CreatedBy.LastName}",
                UpdatedBy = $"{appointment?.UpdatedBy?.FirstName} {appointment?.UpdatedBy?.LastName}" ?? string.Empty,
                PatientPhone = appointment?.Patient?.Phone,
                CompanionName = appointment.CompanionName,
                CompanionNationalId = appointment.CompanionNationalId,
                CompanionPhone = appointment.CompanionPhone,
                EmergencyLevel = appointment.EmergencyLevel,
                ClinicId = appointment.ClinicId,
                ClinicName = appointment?.Clinic?.Name,

            };

            return ErrorResponseModel<AppointmentResponse>.Success(GenericErrors.GetSuccess, response);
        }

        public async Task<PagedResponseModel<DataTable>> GetCountsAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default)
        {
            try
            {
                var Params = new SqlParameter[2];
                var Type = pagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Type")?.ItemValue;
                Params[0] = new SqlParameter("@SearchText", pagingFilter.SearchText ?? (object)DBNull.Value);
                Params[1] = new SqlParameter("@Type", Type ?? (object)DBNull.Value);
                var dt = await _sQLHelper.ExecuteDataTableAsync("dbo.SP_GetAppointmentTypeCountStatistics", Params);
                return PagedResponseModel<DataTable>.Success(GenericErrors.GetSuccess, 6, dt);
            }
            catch (Exception)
            {
                return PagedResponseModel<DataTable>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ErrorResponseModel<string>> UpdateAsync(int id, UpdateAppointmentRequest request, CancellationToken cancellationToken = default)
        {

            var appointment = await _unitOfWork.Repository<Appointment>().GetAll(i => i.Id == id).FirstOrDefaultAsync();

            if (appointment == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);


            if (!Enum.TryParse<AppointmentType>(request.AppointmentType, true, out var appointmentType))
                return ErrorResponseModel<string>.Failure(GenericErrors.InvalidType);

            appointment.DoctorId = request.DoctorId;
            appointment.AppointmentDateTime = request.AppointmentDate;
            appointment.PaymentMethod = request.PaymentMethod;
            appointment.Type = appointmentType;

            _unitOfWork.Repository<Appointment>().Update(appointment);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess);
        }

        public async Task<ErrorResponseModel<string>> UpdateStatusAsync(int id, UpdatePatientStatusInEmergencyRequest request, CancellationToken cancellationToken = default)
        {
            var appointment = await _unitOfWork.Repository<Appointment>().GetAll(i => i.Id == id).Include(x => x.Patient).Include(x => x.UpdatedBy).FirstOrDefaultAsync();

            if (appointment is not { })
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            if (appointment.Type != AppointmentType.Emergency)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotEmergency);

            if (request.NewStatus == "General")
            {
                var newType = Enum.Parse<AppointmentType>(request.NewStatus);
                appointment.Type = newType;
                request.NewStatus = "Outpatient";
            }

            if (!Enum.TryParse<PatientStatus>(request.NewStatus, true, out var newStatus))
                return ErrorResponseModel<string>.Failure(GenericErrors.InvalidStatus);

            appointment.Patient.Status = newStatus;

            _unitOfWork.Repository<Appointment>().Delete(appointment);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess);
        }
    }
}
