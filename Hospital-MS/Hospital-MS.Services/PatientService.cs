using Hospital_MS.Core.Abstractions;
using Hospital_MS.Core.Contracts.Admissions;
using Hospital_MS.Core.Contracts.Patients;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Errors;
using Hospital_MS.Core.Models;
using Hospital_MS.Core.Repositories;
using Hospital_MS.Core.Services;
using Hospital_MS.Core.Specifications.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Services
{
    public class PatientService(IUnitOfWork unitOfWork) : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<IReadOnlyList<PatientResponse>>> GetAllAsync(GetPatientsRequest request, CancellationToken cancellationToken = default)
        {
            var spec = new PatientSpecification(request);

            var patients = await _unitOfWork.Repository<Patient>().GetAllWithSpecAsync(spec, cancellationToken);

            var response = patients.Select(patient => new PatientResponse
            {
                PatientId = patient.Id,
                PatientName = patient.FullName,
                Address = patient.Address,
                DateOfBirth = patient.DateOfBirth,
                PatientStatus = patient.Status.ToString(),
                Phone = patient.Phone,
                CreatedOn = patient.CreatedOn,
                CreatedBy = $"{patient.CreatedBy?.FirstName} {patient.CreatedBy?.LastName}",
                UpdatedOn = patient.UpdatedOn,
                UpdatedBy = patient.UpdatedBy != null ?
                    $"{patient.UpdatedBy.FirstName} {patient.UpdatedBy.LastName}" :
                    string.Empty

            }).ToList().AsReadOnly();

            return Result.Success<IReadOnlyList<PatientResponse>>(response);
        }

        public async Task<Result<PatientCountsResponse>> GetCountsAsync(CancellationToken cancellationToken = default)
        {
            var patients = await _unitOfWork.Repository<Patient>().GetAllAsync(cancellationToken);

            var response = new PatientCountsResponse
            {
                StayingCount = patients.Count(a => a.Status == PatientStatus.Staying),
                SurgeryCount = patients.Count(a => a.Status == PatientStatus.Surgery),
                CriticalConditionCount = patients.Count(a => a.Status == PatientStatus.CriticalCondition),
                TreatedCount = patients.Count(a => a.Status == PatientStatus.Treated),
                ArchivedCount = patients.Count(a => a.Status == PatientStatus.Archived),
                OutpatientCount = patients.Count(a => a.Status == PatientStatus.Outpatient)
            };

            return Result.Success(response);
        }

        public async Task<int> GetPatientsCountAsync(GetPatientsRequest request, CancellationToken cancellationToken = default)
        {
            var spec = new PatientCountsSpecification(request);

            return await _unitOfWork.Repository<Patient>().GetCountAsync(spec, cancellationToken);
        }

        public async Task<Result> UpdateStatusAsync(int id, UpdatePatientStatusRequest request, CancellationToken cancellationToken = default)
        {
            var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(id, cancellationToken);

            if (patient is not { })
                return Result.Failure(GenericErrors<Patient>.NotFound);

            if (!Enum.TryParse<PatientStatus>(request.NewStatus, true, out var patientStatus))

                return Result.Failure(new Error("InvalidStatus", "Invalid patient status provided.", 400));

            patient.Status = patientStatus;
            patient.Notes = request.Notes;

            _unitOfWork.Repository<Patient>().Update(patient);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success();
        }
    }
}
