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

        public async Task<ErrorResponseModel<string>> CreateAsync(CreateAdmissionRequest request, CancellationToken cancellationToken = default)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                if (!Enum.TryParse<PatientStatus>(request.PatientStatus, true, out var patientStatus))
                    return ErrorResponseModel<string>.Failure(GenericErrors.InvalidStatus);

                if (!Enum.TryParse<Gender>(request.PatientGender, true, out var patientGender))
                    return ErrorResponseModel<string>.Failure(GenericErrors.InvalidStatus);

                var doctorExists = await _unitOfWork.Repository<Doctor>().AnyAsync(d => d.Id == request.DoctorId, cancellationToken);
                if (!doctorExists)
                    return ErrorResponseModel<string>.Failure(new Error("لايوجد طبيب بهذا الرقم", Status.NotFound));

                var roomExists = await _unitOfWork.Repository<Room>().AnyAsync(r => r.Id == request.RoomId, cancellationToken);
                if (!roomExists)
                    return ErrorResponseModel<string>.Failure(new Error("لايوجد غرفة بهذا الرقم", Status.NotFound));

                var bedExists = await _unitOfWork.Repository<Bed>().GetByIdAsync(request.BedId);
                if (bedExists == null)
                    return ErrorResponseModel<string>.Failure(new Error("لايوجد سرير بهذا الرقم", Status.NotFound));

                var bedAssigned = await _unitOfWork.Repository<Admission>().AnyAsync(a => a.BedId == request.BedId, cancellationToken);
                if (bedAssigned)
                    return ErrorResponseModel<string>.Failure(new Error("السرير محجوز بالفعل", Status.Conflict));

                // Check if the patient already exists by phone number
                var existingPatient = await _unitOfWork.Repository<Patient>()
                    .GetAll(p => p.Phone == request.PatientPhone)
                    .FirstOrDefaultAsync(cancellationToken);

                Patient patient;

                if (existingPatient != null)
                {
                    existingPatient.FullName = ArabicNormalizer.NormalizeArabic(request.PatientName);
                    existingPatient.InsuranceCompanyId = request.InsuranceCompanyId;
                    existingPatient.InsuranceCategoryId = request.InsuranceCategoryId;
                    existingPatient.DateOfBirth = request.PatientBirthDate;
                    existingPatient.Address = request.PatientAddress;
                    existingPatient.EmergencyContact01 = request.EmergencyContact01;
                    existingPatient.EmergencyPhone01 = request.EmergencyPhone01;
                    existingPatient.EmergencyContact02 = request.EmergencyContact02;
                    existingPatient.EmergencyPhone02 = request.EmergencyPhone02;
                    existingPatient.Status = patientStatus;
                    existingPatient.NationalId = request.PatientNationalId;
                    existingPatient.InsuranceNumber = request.InsuranceNumber;
                    existingPatient.Gender = patientGender;

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
                        DateOfBirth = request.PatientBirthDate,
                        Address = request.PatientAddress,
                        EmergencyContact01 = request.EmergencyContact01,
                        EmergencyPhone01 = request.EmergencyPhone01,
                        EmergencyContact02 = request.EmergencyContact02,
                        EmergencyPhone02 = request.EmergencyPhone02,
                        Status = patientStatus,
                        NationalId = request.PatientNationalId,
                        InsuranceNumber = request.InsuranceNumber,
                        Gender = patientGender
                    };

                    await _unitOfWork.Repository<Patient>().AddAsync(patient, cancellationToken);
                }

                await _unitOfWork.CompleteAsync(cancellationToken);

                var admission = new Admission
                {
                    PatientId = patient.Id,
                    BedId = request.BedId,
                    RoomId = request.RoomId,
                    CompanionName = request.CompanionName,
                    CompanionNationalId = request.CompanionNationalId,
                    CompanionPhone = request.CompanionPhone,
                    DepartmentId = request.DepartmentId,
                    DoctorId = request.DoctorId,
                    HealthStatus = request.HealthStatus,
                    HasCompanion = request.HasCompanion,
                    InitialDiagnosis = request.InitialDiagnosis,
                    Notes = request.Notes,
                };

                await _unitOfWork.Repository<Admission>().AddAsync(admission, cancellationToken);

                bedExists.Status = BedStatus.NotAvailable;
                _unitOfWork.Repository<Bed>().Update(bedExists);

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

        public async Task<ErrorResponseModel<AdmissionResponse>> GetByIdAsync(int patientId, CancellationToken cancellationToken = default)
        {
            var admission = await _unitOfWork.Repository<Admission>()
                .GetAll(i => i.PatientId == patientId)
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .Include(x => x.Patient)
                .Include(x => x.Bed)
                .Include(x => x.Room)
                .Include(x => x.Doctor)
                .Include(x => x.Department)
                .FirstOrDefaultAsync(cancellationToken);

            if (admission is not { })
                return ErrorResponseModel<AdmissionResponse>.Failure(GenericErrors.NotFound);

            var response = new AdmissionResponse
            {
                PatientName = admission.Patient.FullName,
                PatientId = admission.PatientId,
                DateOfBirth = admission.Patient.DateOfBirth,
                Phone = admission.Patient.Phone,
                Address = admission.Patient.Address,
                PatientStatus = admission.Patient.Status.ToString(),
                AdmissionDate = admission.AdmissionDate,
                RoomNumber = admission.Room.Number,
                BedNumber = admission.Bed.Number,
                DepartmentName = admission.Department.Name,
                DoctorName = admission.Doctor.FullName,
                InsuranceCompanyName = admission.Patient.InsuranceCompany?.Name,
                InsuranceCategoryName = admission.Patient.InsuranceCategory?.Name,
                InsuranceNumber = admission.Patient.InsuranceNumber,
                EmergencyContact01 = admission.Patient.EmergencyContact01,
                EmergencyPhone01 = admission.Patient.EmergencyPhone01,
                EmergencyContact02 = admission.Patient.EmergencyPhone02,
                EmergencyPhone02 = admission.Patient.EmergencyPhone02,
                HealthStatus = admission.HealthStatus,
                InitialDiagnosis = admission.InitialDiagnosis,
                HasCompanion = admission.HasCompanion,
                CompanionName = admission.CompanionName,
                CompanionPhone = admission.CompanionPhone,
                CompanionNationalId = admission.CompanionNationalId,
                Notes = admission.Notes,
                CreatedOn = admission.CreatedOn,
                CreatedBy = $"{admission.CreatedBy?.FirstName} {admission.CreatedBy?.LastName}",
                UpdatedOn = admission.UpdatedOn,
                UpdatedBy = admission.UpdatedBy != null ? $"{admission.UpdatedBy.FirstName} {admission.UpdatedBy.LastName}" : string.Empty
            };

            return ErrorResponseModel<AdmissionResponse>.Success(GenericErrors.GetSuccess, response);
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
