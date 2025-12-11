using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Admissions;
using Hospital_MS.Core.Contracts.Patients;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Extensions;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.Common;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Hospital_MS.Services.HMS
{
    public class PatientService(IUnitOfWork unitOfWork, ISQLHelper sQLHelper) : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ISQLHelper _sQLHelper = sQLHelper;

        public async Task<PagedResponseModel<List<PatientListResponse>>> GetAllAsync(PagingFilterModel pagingFilter,CancellationToken cancellationToken = default)
        {
            pagingFilter ??= new PagingFilterModel();
            pagingFilter.FilterList ??= new List<FilterModel>();
            try
            {
                var repo = _unitOfWork.Repository<Patient>();

                var query = repo.GetAll().AsQueryable();

                if (!string.IsNullOrWhiteSpace(pagingFilter.SearchText))
                {
                    query = query.Where(p =>
                        p.FullName.Contains(pagingFilter.SearchText) ||
                        p.Phone.Contains(pagingFilter.SearchText) ||
                        p.NationalId.Contains(pagingFilter.SearchText));
                }

                var statusStr = pagingFilter.FilterList
                    .FirstOrDefault(f => f.CategoryName == "Status")?.ItemValue;

                if (!string.IsNullOrEmpty(statusStr) &&
                    Enum.TryParse(typeof(PatientStatus), statusStr, out var statusObj))
                {
                    var statusEnum = (PatientStatus)statusObj;
                    query = query.Where(p => p.Status == statusEnum);
                }

                var dateFilter = pagingFilter.FilterList
                    .FirstOrDefault(f => f.CategoryName == "Date");

                if (dateFilter?.FromDate != null)
                {
                    query = query.Where(p => p.CreatedOn.Date >= dateFilter.FromDate.Value.Date);
                }

                if (dateFilter?.ToDate != null)
                {
                    query = query.Where(p => p.CreatedOn.Date <= dateFilter.ToDate.Value.Date);
                }
                var totalCount = await query.CountAsync(cancellationToken);

                var patients = await query
                    .OrderByDescending(p => p.CreatedOn)
                    .Skip((pagingFilter.CurrentPage - 1) * pagingFilter.PageSize)
                    .Take(pagingFilter.PageSize)
                    .Select(p => new PatientListResponse
                    {
                        Id = p.Id,
                        FullName = p.FullName,
                        Phone = p.Phone,
                        Gender = p.Gender.ToString(),
                        Status = p.Status.ToString(),
                        DateOfBirth = p.DateOfBirth,
                        NationalId = p.NationalId,
                        CreatedOn = p.CreatedOn
                    })
                    .ToListAsync(cancellationToken);

                foreach (var p in patients)
                {
                    p.Status = p.Status.TryTranslateEnum<PatientStatus>();
                    p.Gender = p.Gender.TryTranslateEnum<Gender>();
                }

                return PagedResponseModel<List<PatientListResponse>>
                    .Success(GenericErrors.GetSuccess, totalCount, patients);
            }
            catch (Exception)
            {
                return PagedResponseModel<List<PatientListResponse>>
                    .Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ErrorResponseModel<PatientListResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var patient = await _unitOfWork.Repository<Patient>()
                .GetAll(i => i.Id == id)
                .Include(i => i.InsuranceCompany)
                .Include(i => i.InsuranceCategory)
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .FirstOrDefaultAsync(cancellationToken);

            if (patient is not { })
                return ErrorResponseModel<PatientListResponse>.Failure(GenericErrors.NotFound);

            var response = new PatientListResponse
            {
                Id = patient.Id,
                FullName = patient.FullName,
                Address = patient.Address,
                DateOfBirth = patient.DateOfBirth,
                PatientStatus = patient?.Status?.GetArabicValue(),
                PatientGender = patient?.Gender?.GetArabicValue(),
                Phone = patient.Phone,
                CreatedOn = patient.CreatedOn,
                CreatedBy = $"{patient.CreatedBy?.FirstName} {patient.CreatedBy?.LastName}",
                UpdatedOn = patient.UpdatedOn,
                UpdatedBy = patient.UpdatedBy != null ? $"{patient.UpdatedBy.FirstName} {patient.UpdatedBy.LastName}" : string.Empty
            };

            return ErrorResponseModel<PatientListResponse>.Success(GenericErrors.GetSuccess, response);
        }

        public async Task<PagedResponseModel<DataTable>> GetCountsAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default)
        {
            try
            {
                var Params = new SqlParameter[4];
                var Status = pagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Status")?.ItemValue;
                var FromDate = pagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Date")?.FromDate;
                var ToDate = pagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Date")?.ToDate;
                Params[0] = new SqlParameter("@SearchText", pagingFilter.SearchText ?? (object)DBNull.Value);
                Params[1] = new SqlParameter("@Status", Status ?? (object)DBNull.Value);
                Params[2] = new SqlParameter("@FromDate", FromDate ?? (object)DBNull.Value);
                Params[3] = new SqlParameter("@ToDate", ToDate ?? (object)DBNull.Value);
                var dt = await _sQLHelper.ExecuteDataTableAsync("dbo.SP_GetPatientStatusCountStatistics", Params);

                return PagedResponseModel<DataTable>.Success(GenericErrors.GetSuccess, 6, dt);
            }
            catch (Exception)
            {
                return PagedResponseModel<DataTable>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ErrorResponseModel<string>> UpdateStatusAsync(int id, UpdatePatientStatusRequest request, CancellationToken cancellationToken = default)
        {
            var patient = await _unitOfWork.Repository<Patient>().GetAll(i => i.Id == id).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (patient is not { })
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            if (!Enum.TryParse<PatientStatus>(request.NewStatus, true, out var patientStatus))

                return ErrorResponseModel<string>.Failure(GenericErrors.InvalidStatus);

            patient.Status = patientStatus;
            patient.Notes = request.Notes;

            _unitOfWork.Repository<Patient>().Update(patient);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess);
        }


        public async Task<ErrorResponseModel<PatientAdmissionsWithAppointmentsResponse>> GetPatientAdmissionsByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var admissions = await _unitOfWork.Repository<Admission>()
                .GetAll(i => i.PatientId == id)
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .Include(x => x.Patient)
                .Include(x => x.Bed)
                .Include(x => x.Room)
                .Include(x => x.Doctor)
                .Include(x => x.Department)
                .ToListAsync(cancellationToken);

            var appointments = await _unitOfWork.Repository<Appointment>()
                .GetAll(a => a.PatientId == id)
                .Include(a => a.Doctor)
                .Include(a => a.Clinic)
                .Include(a => a.MedicalService)
                .ToListAsync(cancellationToken);

            var admissionsResponse = admissions.Select(x => new PatientAdmissionsResponse
            {
                PatientName = x.Patient.FullName,
                PatientId = x.PatientId,
                PatientStatus = x.Patient.Status.ToString(),
                AdmissionDate = x.AdmissionDate,
                RoomNumber = x.Room.Number,
                BedNumber = x.Bed.Number,
                DepartmentName = x.Department.Name,
                DoctorName = x.Doctor.FullName,
                HealthStatus = x.HealthStatus,
                Notes = x.Notes,
                PatientPhoneNumber = x.Patient.Phone,
            }).ToList().AsReadOnly();

            var appointmentsResponse = appointments.Select(a => new PatientAppointmentResponse
            {
                AppointmentId = a.Id,
                AppointmentDate = a.AppointmentDate,
                DoctorName = a.Doctor?.FullName,
                ClinicName = a.Clinic?.Name,
                MedicalServiceName = a.MedicalService?.Name,
                Status = a.Status.ToString(),
                Type = a.Type.ToString(),

            }).ToList().AsReadOnly();

            var response = new PatientAdmissionsWithAppointmentsResponse
            {
                Admissions = admissionsResponse,
                Appointments = appointmentsResponse,
                HasAppointments = appointmentsResponse.Any()
            };

            return ErrorResponseModel<PatientAdmissionsWithAppointmentsResponse>.Success(GenericErrors.GetSuccess, response);
        }
    }
}
