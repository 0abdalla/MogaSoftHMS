using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Admissions;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using Hospital_MS.Core.Services;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Hospital_MS.Services.HMS
{
    public class AdmissionService(IUnitOfWork unitOfWork) : IAdmissionService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorResponseModel<string>> CreateAsync(CreateAdmissionRequest request,CancellationToken cancellationToken = default)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var patientStatus = request.PatientStatus;
                var patientGender = request.PatientGender;

                if (!await _unitOfWork.Repository<Doctor>()
                    .AnyAsync(d => d.Id == request.DoctorId, cancellationToken))
                    return ErrorResponseModel<string>.Failure(
                        new Error("لا يوجد طبيب بهذا الرقم", Status.NotFound));

                var room = await _unitOfWork.Repository<Room>()
                    .GetByIdAsync(request.RoomId, cancellationToken);

                if (room == null)
                    return ErrorResponseModel<string>.Failure(
                        new Error("لا يوجد غرفة بهذا الرقم", Status.NotFound));

                var bed = await _unitOfWork.Repository<Bed>()
                    .GetByIdAsync(request.BedId, cancellationToken);

                if (bed == null)
                    return ErrorResponseModel<string>.Failure(
                        new Error("لا يوجد سرير بهذا الرقم", Status.NotFound));

                if (bed.RoomId != request.RoomId)
                    return ErrorResponseModel<string>.Failure(
                        new Error("السرير لا ينتمي إلى هذه الغرفة", Status.Conflict));

                if (bed.Status == BedStatus.NotAvailable)
                    return ErrorResponseModel<string>.Failure(
                        new Error("السرير محجوز بالفعل", Status.Conflict));
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
                        Gender = patientGender,
                        Status = PatientStatus.Inpatient,
                        DateOfBirth = request.PatientBirthDate,
                        NationalId = request.PatientNationalId,
                        Address = request.PatientAddress,
                        EmergencyContact01 = request.EmergencyContact01,
                        EmergencyPhone01 = request.EmergencyPhone01,
                        EmergencyContact02 = request.EmergencyContact02,
                        EmergencyPhone02 = request.EmergencyPhone02,
                        InsuranceCompanyId = request.InsuranceCompanyId,
                        InsuranceCategoryId = request.InsuranceCategoryId,
                        InsuranceNumber = request.InsuranceNumber,
                        LastVisitDate = DateTime.UtcNow
                    };

                    await patientRepo.AddAsync(patient, cancellationToken);
                }
                else
                {
                    patient.FullName = ArabicNormalizer.NormalizeArabic(request.PatientName);
                    patient.Gender = patientGender;
                    patient.Status = PatientStatus.Inpatient;
                    patient.DateOfBirth = request.PatientBirthDate;
                    patient.NationalId = request.PatientNationalId;
                    patient.Address = request.PatientAddress;
                    patient.EmergencyContact01 = request.EmergencyContact01;
                    patient.EmergencyPhone01 = request.EmergencyPhone01;
                    patient.EmergencyContact02 = request.EmergencyContact02;
                    patient.EmergencyPhone02 = request.EmergencyPhone02;
                    patient.InsuranceCompanyId = request.InsuranceCompanyId;
                    patient.InsuranceCategoryId = request.InsuranceCategoryId;
                    patient.InsuranceNumber = request.InsuranceNumber;
                    patient.LastVisitDate = DateTime.UtcNow;

                    patientRepo.Update(patient);
                }

                await _unitOfWork.CompleteAsync(cancellationToken);

                if (!Enum.IsDefined(typeof(AdmissionType), request.AdmissionType))
                    return ErrorResponseModel<string>.Failure(GenericErrors.InvalidType);

                //if (request.AdmissionType == AdmissionType.Surgery && request.MedicalServiceId == null)
                //    return ErrorResponseModel<string>.Failure(
                //        new Error("يجب اختيار خدمة طبية عند اختيار نوع دخول (جراحة", Status.Failed));

                var admission = new Admission
                {
                    PatientId = patient.Id,
                    DepartmentId = request.DepartmentId,
                    DoctorId = request.DoctorId,
                    RoomId = request.RoomId,
                    BedId = request.BedId,

                    AdmissionType = request.AdmissionType,
                    PaymentMethod = request.PaymentMethod,

                    HealthStatus = request.HealthStatus,
                    InitialDiagnosis = request.InitialDiagnosis,
                    Notes = request.Notes,

                    HasCompanion = request.HasCompanion,
                    CompanionName = request.CompanionName,
                    CompanionPhone = request.CompanionPhone,
                    CompanionNationalId = request.CompanionNationalId
                };

                await _unitOfWork.Repository<Admission>().AddAsync(admission, cancellationToken);

                bed.Status = BedStatus.NotAvailable;
                _unitOfWork.Repository<Bed>().Update(bed);

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

        public async Task<PagedResponseModel<List<AdmissionResponse>>> GetAllAsync(
    PagingFilterModel pagingFilter,
    CancellationToken cancellationToken = default)
        {
            try
            {
                var repo = _unitOfWork.Repository<Admission>();

                var query = repo.GetAll()
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .Include(a => a.Department)
                    .Include(a => a.Room)
                    .Include(a => a.Bed)
                    .Include(a => a.Charges)
                    .AsQueryable();

                // Search
                if (!string.IsNullOrWhiteSpace(pagingFilter.SearchText))
                {
                    query = query.Where(a =>
                        a.Patient.FullName.Contains(pagingFilter.SearchText) ||
                        a.Patient.Phone.Contains(pagingFilter.SearchText));
                }

                // Paging
                var totalCount = await query.CountAsync(cancellationToken);

                var admissions = await query
                    .OrderByDescending(a => a.AdmissionDate)
                    .Skip((pagingFilter.CurrentPage - 1) * pagingFilter.PageSize)
                    .Take(pagingFilter.PageSize)
                    .ToListAsync(cancellationToken);

                var mapped = admissions.Select(MapToResponse).ToList();

                return PagedResponseModel<List<AdmissionResponse>>
                    .Success(GenericErrors.GetSuccess, totalCount, mapped);
            }
            catch
            {
                return PagedResponseModel<List<AdmissionResponse>>
                    .Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ErrorResponseModel<AdmissionResponse>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var admission = await _unitOfWork.Repository<Admission>()
                .GetAll(a => a.Id == id)
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.Department)
                .Include(a => a.Room)
                .Include(a => a.Bed)
                .Include(a => a.Charges)
                .FirstOrDefaultAsync(cancellationToken);

            if (admission == null)
                return ErrorResponseModel<AdmissionResponse>
                    .Failure(new Error("لا يوجد سجل دخول بهذا الرقم", Status.NotFound));

            return ErrorResponseModel<AdmissionResponse>
                .Success(GenericErrors.GetSuccess, MapToResponse(admission));
        }

        public async Task<ErrorResponseModel<string>> UpdateAsync(
    int id,
    UpdateAdmissionRequest request,
    CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Admission>();
            var admission = await repo.GetByIdAsync(id, cancellationToken);

            if (admission == null)
                return ErrorResponseModel<string>.Failure(new Error("غير موجود", Status.NotFound));

            admission.HealthStatus = request.HealthStatus;
            admission.InitialDiagnosis = request.InitialDiagnosis;
            admission.Notes = request.Notes;

            admission.HasCompanion = request.HasCompanion;
            admission.CompanionName = request.CompanionName;
            admission.CompanionNationalId = request.CompanionNationalId;
            admission.CompanionPhone = request.CompanionPhone;

            admission.Status = request.Status;
            admission.PaymentMethod = request.PaymentMethod;

            if (request.Status == AdmissionStatus.Discharged)
            {
                admission.DischargeSummary = request.DischargeSummary;
                admission.DischargeDate = request.DischargeDate ?? DateTime.UtcNow;

                // تحرير السرير
                var bed = await _unitOfWork.Repository<Bed>()
                    .GetByIdAsync(admission.BedId, cancellationToken);

                if (bed != null)
                {
                    bed.Status = BedStatus.Available;
                    _unitOfWork.Repository<Bed>().Update(bed);
                }
            }

            repo.Update(admission);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess);
        }

        //public async Task<ErrorResponseModel<AdmissionResponse>> GetByIdAsync(int patientId, CancellationToken cancellationToken = default)
        //{
        //    var admission = await _unitOfWork.Repository<Admission>()
        //        .GetAll(i => i.PatientId == patientId)
        //        .Include(x => x.CreatedBy)
        //        .Include(x => x.UpdatedBy)
        //        .Include(x => x.Patient)
        //        .Include(x => x.Bed)
        //        .Include(x => x.Room)
        //        .Include(x => x.MedicalService)
        //        .Include(x => x.Doctor)
        //        .Include(x => x.Department)
        //        .FirstOrDefaultAsync(cancellationToken);

        //    if (admission is not { })
        //        return ErrorResponseModel<AdmissionResponse>.Failure(GenericErrors.NotFound);

        //    var response = new AdmissionResponse
        //    {
        //        PatientName = admission.Patient.FullName,
        //        PatientId = admission.PatientId,
        //        DateOfBirth = admission.Patient.DateOfBirth,
        //        Phone = admission.Patient.Phone,
        //        Address = admission.Patient.Address,
        //        PatientStatus = admission.Patient.Status.ToString(),
        //        AdmissionDate = admission.AdmissionDate,
        //        RoomNumber = admission.Room.Number,
        //        BedNumber = admission.Bed.Number,
        //        DepartmentName = admission.Department.Name,
        //        DoctorName = admission.Doctor.FullName,
        //        InsuranceCompanyName = admission.Patient.InsuranceCompany?.Name,
        //        InsuranceCategoryName = admission.Patient.InsuranceCategory?.Name,
        //        InsuranceNumber = admission.Patient.InsuranceNumber,
        //        EmergencyContact01 = admission.Patient.EmergencyContact01,
        //        EmergencyPhone01 = admission.Patient.EmergencyPhone01,
        //        EmergencyContact02 = admission.Patient.EmergencyPhone02,
        //        EmergencyPhone02 = admission.Patient.EmergencyPhone02,
        //        HealthStatus = admission.HealthStatus,
        //        InitialDiagnosis = admission.InitialDiagnosis,
        //        HasCompanion = admission.HasCompanion,
        //        CompanionName = admission.CompanionName,
        //        CompanionPhone = admission.CompanionPhone,
        //        CompanionNationalId = admission.CompanionNationalId,
        //        Notes = admission.Notes,
        //        //MedicalServiceName = admission?.MedicalService?.Name,
        //        //surgeryType = admission.surgeryType,

        //        CreatedOn = admission.CreatedOn,
        //        CreatedBy = $"{admission.CreatedBy?.FirstName} {admission.CreatedBy?.LastName}",
        //        UpdatedOn = admission.UpdatedOn,
        //        UpdatedBy = admission.UpdatedBy != null ? $"{admission.UpdatedBy.FirstName} {admission.UpdatedBy.LastName}" : string.Empty
        //    };

        //    return ErrorResponseModel<AdmissionResponse>.Success(GenericErrors.GetSuccess, response);
        //}
        private AdmissionResponse MapToResponse(Admission a)
        {
            return new AdmissionResponse
            {
                Id = a.Id,
                AdmissionDate = a.AdmissionDate,
                DischargeDate = a.DischargeDate,

                PatientStatus = a.Status.ToString(),
                AdmissionType = a.AdmissionType.ToString(),

                InitialDiagnosis = a.InitialDiagnosis,
                HealthStatus = a.HealthStatus,
                Notes = a.Notes,

                HasCompanion = a.HasCompanion,
                CompanionName = a.CompanionName,
                CompanionPhone = a.CompanionPhone,
                CompanionNationalId = a.CompanionNationalId,

                PatientId = a.PatientId,
                PatientName = a.Patient.FullName,
                PatientPhone = a.Patient.Phone,

                DoctorId = a.DoctorId,
                DoctorName = a.Doctor?.FullName,

                DepartmentId = a.DepartmentId,
                DepartmentName = a.Department.Name,

                RoomId = a.RoomId,
                RoomName = a.Room.Number,

                BedId = a.BedId,
                BedNumber = a.Bed.Number,

                //PaymentMethod = a.PaymentMethod,
                //DischargeSummary = a.DischargeSummary,

                //Charges = a.Charges.Select(c => new AdmissionChargeDto
                //{
                //    Description = c.Description,
                //    Price = c.Price,
                //    ChargeDate = c.ChargeDate
                //}).ToList()
            };
        }

        public async Task<ErrorResponseModel<IReadOnlyList<PatientAdmissionsResponse>>> GetPatientAdmissionsByIdAsync(int id, CancellationToken cancellationToken = default)
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

            var response = admissions.Select(x => new PatientAdmissionsResponse
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
                NationalId = x.Patient.NationalId,
                Gender = x.Patient.Gender.ToString(),

            }).ToList().AsReadOnly();

            return ErrorResponseModel<IReadOnlyList<PatientAdmissionsResponse>>.Success(GenericErrors.GetSuccess, response);
        }
    }
}
