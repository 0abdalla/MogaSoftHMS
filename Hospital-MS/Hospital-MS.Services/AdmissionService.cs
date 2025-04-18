using Hospital_MS.Core.Abstractions;
using Hospital_MS.Core.Contracts.Admissions;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Errors;
using Hospital_MS.Core.Helpers;
using Hospital_MS.Core.Models;
using Hospital_MS.Core.Repositories;
using Hospital_MS.Core.Services;
using Hospital_MS.Core.Specifications.Admissions;

namespace Hospital_MS.Services
{
    public class AdmissionService(IUnitOfWork unitOfWork) : IAdmissionService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> CreateAsync(CreateAdmissionRequest request, CancellationToken cancellationToken = default)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                if (!Enum.TryParse<PatientStatus>(request.PatientStatus, true, out var patientStatus))
                    return Result.Failure(new Error("InvalidStatus", "Invalid patient status provided.", 400));

                var patient = new Patient
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
                };

                await _unitOfWork.Repository<Patient>().AddAsync(patient, cancellationToken);
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
                await _unitOfWork.CompleteAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result.Failure(GenericErrors<Admission>.FailedToAdd);
            }

        }

        public async Task<Result<AdmissionResponse>> GetByIdAsync(int patientId, CancellationToken cancellationToken = default)
        {
            var spec = new AdmissionSpecification(patientId);

            var admission = await _unitOfWork.Repository<Admission>().GetByIdWithSpecAsync(spec, cancellationToken);

            if (admission is not { })
                return Result.Failure<AdmissionResponse>(GenericErrors<Admission>.NotFound);

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
                UpdatedBy = admission.UpdatedBy != null ?
                    $"{admission.UpdatedBy.FirstName} {admission.UpdatedBy.LastName}" :
                    string.Empty
            };

            return Result.Success(response);
        }
    }
}
