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
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Hospital_MS.Services.HMS
{
    public class AppointmentService(IUnitOfWork unitOfWork, ISQLHelper sQLHelper, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager) :
        IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ISQLHelper _sQLHelper = sQLHelper;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager = userManager;


        public async Task<PagedResponseModel<List<AppointmentsGroupResponse>>> GetAllAsync(
            PagingFilterModel filter, CancellationToken cancellationToken = default)
        {
            try
            {
                var repo = _unitOfWork.Repository<Appointment>();

                var query = repo
                    .GetAll()
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .Include(a => a.Clinic)
                    .Include(a => a.MedicalServiceDetails)
                        .ThenInclude(ms => ms.MedicalService)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(filter.SearchText))
                {
                    query = query.Where(a =>
                        a.Patient.FullName.Contains(filter.SearchText) ||
                        a.Patient.Phone.Contains(filter.SearchText) ||
                        (a.Doctor != null && a.Doctor.FullName.Contains(filter.SearchText)));
                }

                var typeFilter = filter.FilterList?
                    .FirstOrDefault(x => x.CategoryName == "Type")?.ItemValue;

                if (!string.IsNullOrEmpty(typeFilter) &&
                    Enum.TryParse(typeof(AppointmentType), typeFilter, true, out var parsedType))
                {
                    query = query.Where(a => a.Type == (AppointmentType)parsedType);
                }

                var totalCount = await query.CountAsync(cancellationToken);

                var appointments = await query
                    .OrderByDescending(a => a.CreatedOn)
                    .Skip((filter.CurrentPage - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .ToListAsync(cancellationToken);

                var result = appointments.Select(a => new AppointmentsGroupResponse
                {
                    Id = a.Id,
                    PatientId = a.PatientId,
                    PatientName = a.Patient.FullName,
                    PatientPhone = a.Patient.Phone,

                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor?.FullName,

                    AppointmentDate = a.AppointmentDate,
                    AppointmentNumber = a.AppointmentNumber,


                    PaymentMethod = a.PaymentMethod?.ToString(),
                    Status = a.Status.ToString(),
                    Type = a.Type.ToString(),

                    CreatedOn = a.CreatedOn,
                    UpdatedOn = a.UpdatedOn,
                    CreatedBy = a.CreatedById,
                    UpdatedBy = a.UpdatedById,

                    MedicalServiceName = string.Join(";;;",
                        a.MedicalServiceDetails
                            .Select(ms => ms.MedicalService.Name)
                            .Where(n => !string.IsNullOrEmpty(n)))
                })
                .ToList();

                return PagedResponseModel<List<AppointmentsGroupResponse>>
                    .Success(GenericErrors.GetSuccess, totalCount, result);
            }
            catch
            {
                return PagedResponseModel<List<AppointmentsGroupResponse>>
                    .Failure(GenericErrors.TransFailed);
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
                //PaymentMethod = appointment?.PaymentMethod,
                Status = appointment.Status.GetArabicValue(),
                Type = appointment.Type.GetArabicValue(),
                DoctorId = appointment.DoctorId,
                PatientId = appointment.PatientId,
                CreatedOn = appointment.CreatedOn,
                UpdatedOn = appointment.UpdatedOn,
                CreatedBy = $"{appointment.CreatedBy.FirstName} {appointment.CreatedBy.LastName}",
                UpdatedBy = $"{appointment?.UpdatedBy?.FirstName} {appointment?.UpdatedBy?.LastName}" ?? string.Empty,
                PatientPhone = appointment?.Patient?.Phone,
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
                appointment.Type = appointmentType;
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
                    .GetAll(a => a.AppointmentDate <= DateTime.UtcNow && !a.IsClosed && a.Status != AppointmentStatus.Rejected)
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

                var appUser = await _userManager.GetUserAsync(user);
                closedShiftResponse.ClosedBy = appUser.UserName ?? "غير معرف";

                // Update appointments (now tracked separately)
                var appointmentsToUpdate = await _unitOfWork.Repository<Appointment>()
                    .GetAll(a =>
                        a.AppointmentDate <= DateTime.UtcNow
                        && !a.IsClosed
                        && a.Status != AppointmentStatus.Rejected)
                    .ToListAsync(cancellationToken);

                foreach (var appointment in appointmentsToUpdate)
                {
                    appointment.IsClosed = true;
                    _unitOfWork.Repository<Appointment>().Update(appointment);
                }

                var minDate = closedAppointments
                        .Where(a => a.AppointmentDate.HasValue)
                         .Min(a => a.AppointmentDate);

                // save the closed shift 

                var closedShift = new Shift
                {
                    TotalAmount = closedShiftResponse.TotalAmount,
                    ClosedAt = closedShiftResponse.ClosedAt,
                    ClosedBy = closedShiftResponse.ClosedBy,
                    //OpenedAt = minDate.HasValue
                    //            ? minDate.Value(TimeOnly.MinValue)
                    //            : DateTime.UtcNow
                };

                await _unitOfWork.Repository<Shift>().AddAsync(closedShift, cancellationToken);
                await _unitOfWork.CompleteAsync(cancellationToken);

                foreach (var ms in medicalServiceCounts)
                {
                    var shiftService = new ShiftMedicalService
                    {
                        ShiftId = closedShift.Id,
                        MedicalServiceId = ms.MedicalServiceId,
                        MedicalServiceName = ms.MedicalServiceName,
                        Count = ms.Count,
                        Price = ms.Price,
                        TotalPrice = ms.TotalPrice
                    };

                    await _unitOfWork.Repository<ShiftMedicalService>().AddAsync(shiftService, cancellationToken);
                }

                var newShift = new Shift
                {
                    OpenedAt = DateTime.UtcNow,
                    TotalAmount = 0,
                    ClosedBy = string.Empty,
                    ClosedAt = null
                };

                await _unitOfWork.Repository<Shift>().AddAsync(newShift, cancellationToken);

                await _unitOfWork.CompleteAsync(cancellationToken);

                return ErrorResponseModel<ClosedShiftResponse>.Success(GenericErrors.GetSuccess, closedShiftResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error closing shift: {ex.Message}");
                return ErrorResponseModel<ClosedShiftResponse>.Failure(GenericErrors.TransFailed);
            }
        }


        public async Task<ErrorResponseModel<List<AppointmentToReturnResponse>>> CreateAsyncV2(CreateAppointmentRequest request,CancellationToken cancellationToken)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var gender = request.Gender;
                var appointmentType = request.AppointmentType;

                var patientRepo = _unitOfWork.Repository<Patient>();

                var patient = await patientRepo
                    .GetAll(p => p.Phone == request.PatientPhone)
                    .FirstOrDefaultAsync(cancellationToken);

                if (patient == null)
                {
                    patient = new Patient
                    {
                        FullName = ArabicNormalizer.NormalizeArabic(request.PatientName),
                        Phone = request.PatientPhone,
                        Gender = gender,
                        InsuranceCompanyId = request.InsuranceCompanyId,
                        InsuranceCategoryId = request.InsuranceCategoryId,
                        InsuranceNumber = request.InsuranceNumber,
                        LastVisitDate = DateTime.UtcNow,
                        Status = PatientStatus.Outpatient
                    };

                    await patientRepo.AddAsync(patient, cancellationToken);
                }
                else
                {
                    patient.FullName = ArabicNormalizer.NormalizeArabic(request.PatientName);
                    patient.Gender = gender;
                    patient.InsuranceCompanyId = request.InsuranceCompanyId;
                    patient.InsuranceCategoryId = request.InsuranceCategoryId;
                    patient.InsuranceNumber = request.InsuranceNumber;
                    patient.LastVisitDate = DateTime.UtcNow;

                    patientRepo.Update(patient);
                }

                await _unitOfWork.CompleteAsync(cancellationToken);

                var appointmentRepo = _unitOfWork.Repository<Appointment>();
                var msRepo = _unitOfWork.Repository<MedicalService>();
                var detailRepo = _unitOfWork.Repository<MedicalServiceDetail>();

                var responses = new List<AppointmentToReturnResponse>();

                if (request.MedicalServiceIds == null || !request.MedicalServiceIds.Any())
                    return ErrorResponseModel<List<AppointmentToReturnResponse>>.Failure(GenericErrors.InvalidType);

                var needDoctorScheduleCheck =
                    appointmentType == AppointmentType.General ||
                    appointmentType == AppointmentType.Consultation ||
                    appointmentType == AppointmentType.Surgery;

                if (needDoctorScheduleCheck && request.DoctorId.HasValue)
                {
                    var scheduleRepo = _unitOfWork.Repository<DoctorSchedule>();

                    var schedule = await scheduleRepo
                        .GetAll(s =>
                            s.DoctorId == request.DoctorId &&
                            s.WeekDay == request.AppointmentDate.DayOfWeek.ToString())
                        .FirstOrDefaultAsync(cancellationToken);

                    if (schedule == null)
                        return ErrorResponseModel<List<AppointmentToReturnResponse>>.Failure(GenericErrors.ScheduleNotFound);

                    if (schedule.CurrentAppointments >= schedule.Capacity)
                        return ErrorResponseModel<List<AppointmentToReturnResponse>>.Failure(GenericErrors.ScheduleFull);

                    schedule.CurrentAppointments++;
                    scheduleRepo.Update(schedule);
                    await _unitOfWork.CompleteAsync(cancellationToken);
                }

                foreach (var serviceId in request.MedicalServiceIds)
                {
                    var targetDate = request.AppointmentDate.Date;

                    var countQuery = appointmentRepo.GetAll(a =>
                        a.MedicalServiceId == serviceId &&
                        a.AppointmentDate.HasValue &&
                        a.AppointmentDate.Value.Date == targetDate
                    );

                    var existingCount = await countQuery.CountAsync(cancellationToken);
                    var appointmentNumber = existingCount + 1;

                    var appointment = new Appointment
                    {
                        PatientId = patient.Id,
                        DoctorId = request.DoctorId,
                        MedicalServiceId = serviceId,
                        AppointmentDate = request.AppointmentDate,
                        DurationInMinutes = 15,
                        Status = AppointmentStatus.Pending,
                        Type = appointmentType,
                        PaymentMethod = request.PaymentMethod,
                        BillingStatus = BillingStatus.Unpaid,
                        AppointmentNumber = appointmentNumber
                    };

                    await appointmentRepo.AddAsync(appointment, cancellationToken);
                    await _unitOfWork.CompleteAsync(cancellationToken);

                    var detail = new MedicalServiceDetail
                    {
                        AppointmentId = appointment.Id,
                        MedicalServiceId = serviceId,
                        AppointmentDate = appointment.AppointmentDate ?? DateTime.UtcNow
                    };

                    await detailRepo.AddAsync(detail, cancellationToken);
                    await _unitOfWork.CompleteAsync(cancellationToken);

                    var service = await msRepo.GetByIdAsync(serviceId, cancellationToken);

                    responses.Add(new AppointmentToReturnResponse
                    {
                        AppointmentId = appointment.Id,
                        AppointmentNumber = appointment.AppointmentNumber,
                        AppointmentDate = appointment.AppointmentDate,
                        PatientName = patient.FullName,
                        PatientPhone = patient.Phone,
                        DoctorName = appointment.Doctor?.FullName,
                        MedicalServices = new List<MedicalServiceResponse>
                {
                    new MedicalServiceResponse
                    {
                        Name = service?.Name,
                        Price = service?.Price ?? 0
                    }
                },
                        TotalPrice = service?.Price ?? 0
                    });
                }

                await transaction.CommitAsync(cancellationToken);

                return ErrorResponseModel<List<AppointmentToReturnResponse>>
                    .Success(GenericErrors.AddSuccess, responses);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                return ErrorResponseModel<List<AppointmentToReturnResponse>>
                    .Failure(GenericErrors.TransFailed);
            }
        }


        public async Task<ErrorResponseModel<List<ShiftResponse>>> GetAllShiftsAsync(CancellationToken cancellationToken = default)
        {

            try
            {
                var shifts = await _unitOfWork.Repository<Shift>().GetAll()
                    .Include(s => s.MedicalServices)
                    .OrderByDescending(s => s.Id)
                    .ToListAsync(cancellationToken);

                var shiftResponses = shifts.Select(s => new ShiftResponse
                {
                    Id = s.Id,
                    OpenedAt = s.OpenedAt,
                    ClosedAt = s.ClosedAt,
                    ClosedBy = s.ClosedBy,
                    TotalAmount = s.TotalAmount,
                    MedicalServices = s.MedicalServices.Select(ms => new ShiftMedicalServiceResponse
                    {
                        MedicalServiceId = ms.MedicalServiceId,
                        MedicalServiceName = ms.MedicalServiceName,
                        Count = ms.Count,
                        Price = ms.Price,
                        TotalPrice = ms.TotalPrice

                    }).ToList()

                }).ToList();
                return ErrorResponseModel<List<ShiftResponse>>.Success(GenericErrors.GetSuccess, shiftResponses);
            }
            catch (Exception)
            {
                return ErrorResponseModel<List<ShiftResponse>>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ErrorResponseModel<ShiftResponse>> GetShiftByIdAsync(int id, CancellationToken cancellationToken = default)
        {

            try
            {
                var shift = await _unitOfWork.Repository<Shift>()
                    .GetAll(s => s.Id == id)
                    .Include(s => s.MedicalServices)
                    .FirstOrDefaultAsync(cancellationToken);

                if (shift == null)
                    return ErrorResponseModel<ShiftResponse>.Failure(GenericErrors.NotFound);

                var response = new ShiftResponse
                {
                    Id = shift.Id,
                    OpenedAt = shift.OpenedAt,
                    ClosedAt = shift.ClosedAt,
                    ClosedBy = shift.ClosedBy,
                    TotalAmount = shift.TotalAmount,
                    MedicalServices = shift.MedicalServices.Select(ms => new ShiftMedicalServiceResponse
                    {
                        MedicalServiceId = ms.MedicalServiceId,
                        MedicalServiceName = ms.MedicalServiceName,
                        Count = ms.Count,
                        Price = ms.Price,
                        TotalPrice = ms.TotalPrice
                    }).ToList()
                };
                return ErrorResponseModel<ShiftResponse>.Success(GenericErrors.GetSuccess, response);
            }
            catch (Exception)
            {
                return ErrorResponseModel<ShiftResponse>.Failure(GenericErrors.TransFailed);

            }
        }

        //public AppointmentType SetAppointmentType(AppointmentType type) =>
        //   type switch
        //   {
        //       AppointmentType.MRI
        //       or AppointmentType.Panorama
        //       or AppointmentType.CTScan
        //       or AppointmentType.Ultrasound
        //       or AppointmentType.XRay
        //       or AppointmentType.Echo
        //       or AppointmentType.Mammogram
        //           => AppointmentType.Radiology,
        //       _ => type
        //   };

        //public async Task<ErrorResponseModel<AppointmentTypeCountsResponse>> GetCountsAsyncV2(CancellationToken cancellationToken = default)
        //{
        //    try
        //    {
        //        var appointments = _unitOfWork.Repository<Appointment>().GetAll();

        //        var response = new AppointmentTypeCountsResponse
        //        {
        //            EmergencyAppointments = await appointments.CountAsync(a => a.Type == AppointmentType.Emergency, cancellationToken),
        //            GeneralAppointments = await appointments.CountAsync(a => a.Type == AppointmentType.General, cancellationToken),
        //            ConsultationAppointments = await appointments.CountAsync(a => a.Type == AppointmentType.Consultation, cancellationToken),
        //            SurgeryAppointments = await appointments.CountAsync(a => a.Type == AppointmentType.Surgery, cancellationToken),
        //            ScreeningAppointments = await appointments.CountAsync(a => a.Type == AppointmentType.Screening, cancellationToken),
        //            RadiologyAppointments = await appointments.CountAsync(a =>
        //                a.Type == AppointmentType.Radiology ||
        //                a.Type == AppointmentType.Panorama ||
        //                a.Type == AppointmentType.MRI ||
        //                a.Type == AppointmentType.CTScan ||
        //                a.Type == AppointmentType.Ultrasound ||
        //                a.Type == AppointmentType.XRay ||
        //                a.Type == AppointmentType.Echo ||
        //                a.Type == AppointmentType.Mammogram, cancellationToken),
        //            TotalAppointments = await appointments.CountAsync(cancellationToken)
        //        };

        //        return ErrorResponseModel<AppointmentTypeCountsResponse>.Success(GenericErrors.GetSuccess, response);
        //    }
        //    catch (Exception)
        //    {
        //        return ErrorResponseModel<AppointmentTypeCountsResponse>.Failure(GenericErrors.TransFailed);
        //    }
        //}
    }
}
