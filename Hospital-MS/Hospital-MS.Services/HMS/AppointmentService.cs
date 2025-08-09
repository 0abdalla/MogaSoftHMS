using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Appointments;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Extensions;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.Common;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Hospital_MS.Services.HMS
{
    public class AppointmentService(IUnitOfWork unitOfWork, ISQLHelper sQLHelper, IHttpContextAccessor httpContextAccessor) : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ISQLHelper _sQLHelper = sQLHelper;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<ErrorResponseModel<AppointmentToReturnResponse>> CreateAsync(CreateAppointmentRequest request, CancellationToken cancellationToken = default)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                if (!Enum.TryParse<Gender>(request.Gender, true, out var gender))
                    return ErrorResponseModel<AppointmentToReturnResponse>.Failure(GenericErrors.InvalidType);

                var existingPatient = await _unitOfWork.Repository<Patient>()
                        .GetAll(p => p.Phone == request.PatientPhone)
                        .FirstOrDefaultAsync(cancellationToken);

                Patient patient;

                if (existingPatient != null)
                {
                    existingPatient.FullName = ArabicNormalizer.NormalizeArabic(request.PatientName);
                    existingPatient.InsuranceCompanyId = request.InsuranceCompanyId;
                    existingPatient.InsuranceCategoryId = request.InsuranceCategoryId;
                    existingPatient.Gender = gender;

                    _unitOfWork.Repository<Patient>().Update(existingPatient);
                    patient = existingPatient;
                }
                else
                {
                    patient = new Patient
                    {
                        FullName = ArabicNormalizer.NormalizeArabic(request.PatientName),
                        Phone = request.PatientPhone,
                        InsuranceCompanyId = request.InsuranceCompanyId,
                        InsuranceCategoryId = request.InsuranceCategoryId,
                        Status = PatientStatus.Outpatient,
                        Gender = gender
                    };

                    await _unitOfWork.Repository<Patient>().AddAsync(patient, cancellationToken);
                }
                foreach (var service in request.MedicalServices)
                {
                    var isNeedDoctor = service.AppointmentType == "General" || service.AppointmentType == "Consultation" || service.AppointmentType == "Surgery";
                    if (isNeedDoctor)
                    {
                        var schedule = await _unitOfWork.Repository<DoctorSchedule>()
                            .GetAll(s => s.DoctorId == request.DoctorId && s.WeekDay == service.AppointmentDate.DayOfWeek.ToString()).FirstOrDefaultAsync(cancellationToken);

                        if (schedule == null)
                            return ErrorResponseModel<AppointmentToReturnResponse>.Failure(GenericErrors.ScheduleNotFound);

                        if (schedule.CurrentAppointments >= schedule.Capacity)
                            return ErrorResponseModel<AppointmentToReturnResponse>.Failure(GenericErrors.ScheduleFull);

                        schedule.CurrentAppointments++;
                        _unitOfWork.Repository<DoctorSchedule>().Update(schedule);
                    }


                    if (!Enum.TryParse<AppointmentType>(service.AppointmentType, true, out var appointmentType))
                        return ErrorResponseModel<AppointmentToReturnResponse>.Failure(GenericErrors.InvalidType);





                    await _unitOfWork.CompleteAsync(cancellationToken);

                    var appointmentNumber = await _unitOfWork.Repository<Appointment>()
                                   .CountAsync(a => a.Type == Enum.Parse<AppointmentType>(service.AppointmentType) && a.AppointmentDate == service.AppointmentDate, cancellationToken) + 1;

                    var appointment = new Appointment
                    {
                        PatientId = patient.Id,
                        DoctorId = request.DoctorId,
                        AppointmentDate = service.AppointmentDate,
                        Type = Enum.Parse<AppointmentType>(service.AppointmentType),
                        PaymentMethod = request.PaymentMethod,
                        AppointmentNumber = appointmentNumber,
                        EmergencyLevel = request.EmergencyLevel,
                        CompanionName = request.CompanionName,
                        CompanionNationalId = request.CompanionNationalId,
                        CompanionPhone = request.CompanionPhone
                    };

                    await _unitOfWork.Repository<Appointment>().AddAsync(appointment, cancellationToken);
                    await _unitOfWork.CompleteAsync(cancellationToken);


                    if (request.MedicalServices.Count > 0)
                    {
                        var serviceDetails = new List<MedicalServiceDetail>();
                        foreach (var item in service.MedicalServiceIds)
                        {
                            var serviceDetail = new MedicalServiceDetail();
                            serviceDetail.AppointmentId = appointment.Id;
                            serviceDetail.MedicalServiceId = item;
                            serviceDetail.AppointmentDate = service.AppointmentDate;

                            serviceDetails.Add(serviceDetail);
                        }
                        await _unitOfWork.Repository<MedicalServiceDetail>().AddRangeAsync(serviceDetails, cancellationToken);
                    }
                    await _unitOfWork.CompleteAsync(cancellationToken);

                    var createdAppointment = await _unitOfWork.Repository<Appointment>()
                        .GetAll(a => a.Id == appointment.Id)
                        .Include(a => a.MedicalService)
                        .FirstOrDefaultAsync(cancellationToken);



                    var response = new AppointmentToReturnResponse
                    {
                        AppointmentNumber = createdAppointment?.AppointmentNumber ?? 0,
                        MedicalServiceName = createdAppointment?.MedicalService?.Name,
                    };
                }
                await transaction.CommitAsync(cancellationToken);


                return ErrorResponseModel<AppointmentToReturnResponse>.Success(GenericErrors.AddSuccess);

            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                return ErrorResponseModel<AppointmentToReturnResponse>.Failure(GenericErrors.TransFailed);
            }
        }

        public PagedResponseModel<List<AppointmentsGroupResponse>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default)
        {
            try
            {
                var Params = new SqlParameter[4];
                var Type = pagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Type")?.ItemValue;
                Params[0] = new SqlParameter("@SearchText", pagingFilter.SearchText ?? (object)DBNull.Value);
                Params[1] = new SqlParameter("@Type", Type ?? (object)DBNull.Value);
                Params[2] = new SqlParameter("@CurrentPage", pagingFilter.CurrentPage);
                Params[3] = new SqlParameter("@PageSize", pagingFilter.PageSize);
                var flatData = _sQLHelper.SQLQuery<AppointmentsGroupResponse>("dbo.SP_GetAllAppointments", Params);
                int totalCount = 0;
                int.TryParse(flatData?.FirstOrDefault()?.TotalCount.ToString(), out totalCount);

                var results = flatData
               .GroupBy(x => x.Id)
               .Select(g =>
               {
                   var first = g.First();
                   return new AppointmentsGroupResponse
                   {
                       Id = first.Id,
                       PatientName = first.PatientName,
                       DoctorName = first.DoctorName,
                       AppointmentDate = first.AppointmentDate,
                       PaymentMethod = first.PaymentMethod,
                       Status = first.Status.TryTranslateEnum<AppointmentStatus>(),
                       Type = first.Type.TryTranslateEnum<AppointmentType>(),
                       DoctorId = first.DoctorId,
                       PatientId = first.PatientId,
                       AppointmentNumber = first.AppointmentNumber,
                       CreatedOn = first.CreatedOn,
                       UpdatedOn = first.UpdatedOn,
                       CreatedBy = first.CreatedBy,
                       UpdatedBy = first.UpdatedBy,
                       PatientPhone = first.PatientPhone,
                       ClinicId = first.ClinicId,
                       ClinicName = first.ClinicName,
                       MedicalServiceName =  string.Join(";;;", g.Select(x => (string)x.MedicalServiceName).Where(x => !string.IsNullOrEmpty(x)).ToList()),
                   };
               }).ToList();

                return PagedResponseModel<List<AppointmentsGroupResponse>>.Success(GenericErrors.GetSuccess, totalCount, results);
            }
            catch (Exception)
            {
                return PagedResponseModel<List<AppointmentsGroupResponse>>.Failure(GenericErrors.TransFailed);
            }

        }

        public async Task<ErrorResponseModel<AppointmentResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var appointment = await _unitOfWork.Repository<Appointment>()
                .GetAll(i => i.Id == id)
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .Include(x => x.Patient)
                .Include(x => x.Doctor)
                .Include(x => x.Patient.InsuranceCompany)
                .Include(x => x.Patient.InsuranceCategory)
                .Include(x => x.Clinic)
                .Include(x => x.MedicalServiceDetails)
                .FirstOrDefaultAsync(cancellationToken);

            var medicalServiceTypes = await _unitOfWork.Repository<MedicalService>().GetAll().ToListAsync();

            if (appointment == null)
                return ErrorResponseModel<AppointmentResponse>.Failure(GenericErrors.NotFound);

            var medicalServices = new List<MedicalServicesModel>();

            if (appointment.MedicalServiceDetails?.Count > 0)
            {
                foreach (var item in appointment.MedicalServiceDetails)
                {
                    var serviceType = medicalServiceTypes.FirstOrDefault(i => i.Id == item.MedicalServiceId);
                    if (serviceType != null)
                    {
                        var obj = new MedicalServicesModel
                        {
                            MedicalServiceId = serviceType?.Id,
                            MedicalServiceName = serviceType?.Name,
                            MedicalServiceType = serviceType?.Type,
                            MedicalServicePrice = serviceType?.Price ?? 0,
                            MedicalServiceDate = item.AppointmentDate,
                            DoctorName = appointment?.Doctor?.FullName
                        };

                        medicalServices.Add(obj);
                    }
                }

            }

            var response = new AppointmentResponse
            {
                Id = appointment.Id,
                PatientName = appointment.Patient.FullName,
                DoctorName = appointment?.Doctor?.FullName,
                AppointmentDate = appointment?.AppointmentDate,
                PaymentMethod = appointment?.PaymentMethod,
                Status = appointment.Status.GetArabicValue(),
                Type = appointment.Type.GetArabicValue(),
                DoctorId = appointment.DoctorId,
                PatientId = appointment.PatientId,
                CreatedOn = appointment.CreatedOn,
                UpdatedOn = appointment.UpdatedOn,
                CreatedBy = $"{appointment.CreatedBy.FirstName} {appointment.CreatedBy.LastName}",
                UpdatedBy = $"{appointment?.UpdatedBy?.FirstName} {appointment?.UpdatedBy?.LastName}" ?? string.Empty,
                PatientPhone = appointment?.Patient?.Phone,
                CompanionName = appointment?.CompanionName,
                CompanionNationalId = appointment?.CompanionNationalId,
                CompanionPhone = appointment?.CompanionPhone,
                EmergencyLevel = appointment?.EmergencyLevel,
                ClinicId = appointment?.ClinicId,
                ClinicName = appointment?.Clinic?.Name,
                MedicalServices = medicalServices,
                Gender = appointment.Patient.Gender.GetArabicValue(),
                AppointmentNumber = appointment.AppointmentNumber
            };

            return ErrorResponseModel<AppointmentResponse>.Success(GenericErrors.GetSuccess, response);
        }

        public async Task<PagedResponseModel<DataTable>> GetCountsAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default)
        {
            try
            {
                var Params = new SqlParameter[2];
                var Type = pagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Type")?.ItemId;
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

        public async Task UpdateAppointmentsToCompletedAsync()
        {
            var appointments = await _unitOfWork.Repository<Appointment>().GetAll()
            .Where(b => b.Status != AppointmentStatus.Rejected && b.Status != AppointmentStatus.Completed)
            .ToListAsync();

            foreach (var appointment in appointments)
            {
                appointment.Status = AppointmentStatus.Completed;
                _unitOfWork.Repository<Appointment>().Update(appointment);
            }

            await _unitOfWork.CompleteAsync();
        }

        public async Task<ErrorResponseModel<string>> UpdateAsync(int id, UpdateAppointmentRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var appointment = await _unitOfWork.Repository<Appointment>()
                    .GetAll(i => i.Id == id)
                    .Include(x => x.Patient)
                    .Include(x => x.MedicalServiceDetails)
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);

                if (appointment == null)
                    return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

                if (!Enum.TryParse<AppointmentType>(request.AppointmentType, true, out var appointmentType))
                    return ErrorResponseModel<string>.Failure(GenericErrors.InvalidType);

                if (!Enum.TryParse<Gender>(request.Gender, true, out var gender))
                    return ErrorResponseModel<string>.Failure(GenericErrors.InvalidType);

                if (appointment.AppointmentDate != request.AppointmentDate || appointment.DoctorId != request.DoctorId)
                {
                    var weekDay = request.AppointmentDate.DayOfWeek.ToString();

                    var schedule = await _unitOfWork.Repository<DoctorSchedule>()
                        .GetAll(s => s.DoctorId == request.DoctorId && s.WeekDay == weekDay)
                        .FirstOrDefaultAsync(cancellationToken);

                    if (schedule == null)
                        return ErrorResponseModel<string>.Failure(GenericErrors.ScheduleNotFound);

                    if (schedule.CurrentAppointments >= schedule.Capacity)
                        return ErrorResponseModel<string>.Failure(GenericErrors.ScheduleFull);


                    ////var appointmentTime = ;

                    //if (request.AppointmentDate < schedule.StartTime || appointmentTime > schedule.EndTime)
                    //    return ErrorResponseModel<string>.Failure(GenericErrors.InvalidType);

                    var oldSchedule = await _unitOfWork.Repository<DoctorSchedule>()
                        .GetAll(s => s.DoctorId == appointment.DoctorId && s.WeekDay == appointment.AppointmentDate.Value.DayOfWeek.ToString())
                        .FirstOrDefaultAsync(cancellationToken);

                    if (oldSchedule != null)
                    {
                        oldSchedule.CurrentAppointments--;
                        _unitOfWork.Repository<DoctorSchedule>().Update(oldSchedule);
                    }

                    schedule.CurrentAppointments++;
                    _unitOfWork.Repository<DoctorSchedule>().Update(schedule);
                }

                appointment.DoctorId = request.DoctorId;
                appointment.AppointmentDate = request.AppointmentDate;
                appointment.PaymentMethod = request.PaymentMethod;
                appointment.Type = appointmentType;
                appointment.EmergencyLevel = request.EmergencyLevel;
                appointment.CompanionName = request.CompanionName;
                appointment.CompanionNationalId = request.CompanionNationalId;
                appointment.CompanionPhone = request.CompanionPhone;
                appointment.Patient.FullName = request.PatientName;
                appointment.Patient.Phone = request.PatientPhone;
                appointment.Patient.InsuranceCompanyId = request.InsuranceCompanyId;
                appointment.Patient.InsuranceCategoryId = request.InsuranceCategoryId;
                appointment.Patient.InsuranceNumber = request.InsuranceNumber;
                appointment.Patient.Gender = gender;

                _unitOfWork.Repository<Appointment>().Update(appointment);
                if (appointment.MedicalServiceDetails?.Count > 0)
                    _unitOfWork.Repository<MedicalServiceDetail>().DeleteRange(appointment.MedicalServiceDetails);
                if (request.MedicalServiceIds?.Count > 0)
                {
                    var serviceDetails = new List<MedicalServiceDetail>();
                    foreach (var item in request.MedicalServiceIds)
                    {
                        var serviceDetail = new MedicalServiceDetail
                        {
                            AppointmentId = appointment.Id,
                            MedicalServiceId = item
                        };

                        serviceDetails.Add(serviceDetail);
                    }
                    await _unitOfWork.Repository<MedicalServiceDetail>().AddRangeAsync(serviceDetails, cancellationToken);
                }

                await _unitOfWork.CompleteAsync(cancellationToken);

                return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess);
            }
            catch (Exception)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }


        public async Task<ErrorResponseModel<string>> UpdateStatusAsync(int id, UpdatePatientStatusInEmergencyRequest request, CancellationToken cancellationToken = default)
        {
            var appointment = await _unitOfWork.Repository<Appointment>().GetAll(i => i.Id == id).Include(x => x.Patient).Include(x => x.UpdatedBy).FirstOrDefaultAsync(cancellationToken: cancellationToken);

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

        public async Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var appointment = await _unitOfWork.Repository<Appointment>()
                    .GetAll(i => i.Id == id)
                    .FirstOrDefaultAsync(cancellationToken);

                if (appointment == null)
                    return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

                appointment.Status = AppointmentStatus.Rejected;

                _unitOfWork.Repository<Appointment>().Update(appointment);

                var schedule = await _unitOfWork.Repository<DoctorSchedule>()
                            .GetAll(s => s.DoctorId == appointment.DoctorId && s.WeekDay == appointment.AppointmentDate.Value.DayOfWeek.ToString())
                            .FirstOrDefaultAsync(cancellationToken);

                if (schedule != null && schedule.CurrentAppointments > 0)
                {
                    schedule.CurrentAppointments--;
                    _unitOfWork.Repository<DoctorSchedule>().Update(schedule);
                }

                await _unitOfWork.CompleteAsync(cancellationToken);

                return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess);
            }
            catch (Exception)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<PagedResponseModel<DataTable>> GetStaffAppointmentsAsync(int staffId, PagingFilterModel pagingFilter, CancellationToken cancellationToken = default)
        {
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new("@StaffId", staffId),
                    new("@SearchText", pagingFilter.SearchText ?? (object)DBNull.Value),
                    new("@CurrentPage", pagingFilter.CurrentPage),
                    new("@PageSize", pagingFilter.PageSize)
                };

                // Add filter parameters
                if (pagingFilter.FilterList != null)
                {
                    var statusFilter = pagingFilter.FilterList.FirstOrDefault(f => f.CategoryName == "Status");
                    if (statusFilter != null)
                    {
                        parameters.Add(new SqlParameter("@Status", statusFilter.ItemValue ?? (object)DBNull.Value));
                    }

                    var dateFilter = pagingFilter.FilterList.FirstOrDefault(f => f.CategoryName == "Date");
                    if (dateFilter != null)
                    {
                        if (dateFilter.FromDate.HasValue)
                            parameters.Add(new SqlParameter("@FromDate", dateFilter.FromDate.Value));
                        if (dateFilter.ToDate.HasValue)
                            parameters.Add(new SqlParameter("@ToDate", dateFilter.ToDate.Value));
                    }
                }

                var dt = await _sQLHelper.ExecuteDataTableAsync("[dbo].[SP_GetStaffAppointments]", parameters.ToArray());

                int totalCount = 0;
                if (dt.Rows.Count > 0)
                {
                    totalCount = dt.Rows[0].Field<int>("TotalCount");
                }

                return PagedResponseModel<DataTable>.Success(GenericErrors.GetSuccess, totalCount, dt);
            }
            catch (Exception)
            {
                return PagedResponseModel<DataTable>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ErrorResponseModel<ClosedShiftResponse>> CloseShiftAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var closedShiftResponse = new ClosedShiftResponse();

                var closedAppointments = await _unitOfWork.Repository<Appointment>()
                    .GetAll(a => a.AppointmentDate <= DateOnly.FromDateTime(DateTime.UtcNow) && !a.IsClosed && a.Status != AppointmentStatus.Rejected)
                    .Include(a => a.MedicalServiceDetails)
                        .ThenInclude(msd => msd.MedicalService)
                    //.AsNoTracking() // Prevent tracking issues
                    .ToListAsync(cancellationToken);

                var medicalServiceCounts = closedAppointments
                    .SelectMany(a => a.MedicalServiceDetails)
                    .GroupBy(msd => new
                    {
                        msd.MedicalServiceId,
                        msd.MedicalService.Name,
                        Price = msd.MedicalService.Price,
                    })
                    .Select(g => new MedicalServiceCountResponse
                    {
                        MedicalServiceId = g.Key.MedicalServiceId,
                        MedicalServiceName = g.Key.Name,
                        Count = g.Count(),
                        Price = g.Key.Price,
                        TotalPrice = g.Count() * (g.Key.Price ?? 0)
                    })
                    .ToList();

                closedShiftResponse.MedicalServices = medicalServiceCounts;
                closedShiftResponse.TotalAmount = medicalServiceCounts.Sum(m => m.TotalPrice);
                closedShiftResponse.ClosedAt = DateTime.UtcNow;

                var user = _httpContextAccessor.HttpContext?.User;
                closedShiftResponse.ClosedBy = user?.Identity?.Name ?? "غير معرف";

                // Update appointments (now tracked separately)
                var appointmentsToUpdate = await _unitOfWork.Repository<Appointment>()
                    .GetAll(a =>
                        a.AppointmentDate <= DateOnly.FromDateTime(DateTime.UtcNow)
                        && !a.IsClosed
                        && a.Status != AppointmentStatus.Rejected)
                    .ToListAsync(cancellationToken);

                foreach (var appointment in appointmentsToUpdate)
                {
                    appointment.IsClosed = true;
                    _unitOfWork.Repository<Appointment>().Update(appointment);
                }

                await _unitOfWork.CompleteAsync(cancellationToken);

                return ErrorResponseModel<ClosedShiftResponse>.Success(GenericErrors.GetSuccess, closedShiftResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error closing shift: {ex.Message}");
                return ErrorResponseModel<ClosedShiftResponse>.Failure(GenericErrors.TransFailed);
            }
        }
    }
}
