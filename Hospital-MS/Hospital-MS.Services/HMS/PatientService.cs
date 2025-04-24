using Hospital_MS.Core.Abstractions;
using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Admissions;
using Hospital_MS.Core.Contracts.Patients;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Errors;
using Hospital_MS.Core.Models;
using Hospital_MS.Core.Services;
using Hospital_MS.Core.Specifications.Patients;
using Hospital_MS.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Services.HMS
{
    public class PatientService(IUnitOfWork unitOfWork) : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorResponseModel<PatientResponse>> GetAllAsync(GetPatientsRequest request, CancellationToken cancellationToken = default)
        {
            var spec = new Patient();//PatientSpecification(request);

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

            return ErrorResponseModel<PatientResponse>.Success(GenericErrors.GetSuccess);
        }

        public async Task<ErrorResponseModel<PatientResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(id, cancellationToken);

            if (patient is not { })
                return ErrorResponseModel<PatientResponse>.Failure(GenericErrors.NotFound);

            var response = new PatientResponse
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
                UpdatedBy = patient.UpdatedBy != null ? $"{patient.UpdatedBy.FirstName} {patient.UpdatedBy.LastName}" : string.Empty
            };

            return ErrorResponseModel<PatientResponse>.Success(GenericErrors.GetSuccess, response);
        }

        public async Task<ErrorResponseModel<PatientCountsResponse>> GetCountsAsync(CancellationToken cancellationToken = default)
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

            return ErrorResponseModel<PatientCountsResponse>.Success(GenericErrors.GetSuccess, response);
        }

        public async Task<int> GetPatientsCountAsync(GetPatientsRequest request, CancellationToken cancellationToken = default)
        {
            var spec = new Patient();//PatientCountsSpecification(request);

            return await _unitOfWork.Repository<Patient>().GetCountAsync(spec, cancellationToken);
        }

        public async Task<ErrorResponseModel<string>> UpdateStatusAsync(int id, UpdatePatientStatusRequest request, CancellationToken cancellationToken = default)
        {
            var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(id, cancellationToken);

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
    }
}
