using Hospital_MS.Core.Abstractions;
using Hospital_MS.Core.Contracts.Patients;
using Hospital_MS.Core.Errors;
using Hospital_MS.Core.Models;
using Hospital_MS.Core.Repositories;
using Hospital_MS.Core.Services;
using Hospital_MS.Core.Specifications.MedicalHistories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Services
{
    public class PatientHistoryService(IUnitOfWork unitOfWork) : IPatientHistoryService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> CreateAsync(PatientMedicalHistoryRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var patientIsExist = await _unitOfWork.Repository<Patient>().AnyAsync(x => x.Id == request.PatientId, cancellationToken);

                if (!patientIsExist)
                    return Result.Failure(GenericErrors<Patient>.NotFound);

                var medicalHistory = new PatientMedicalHistory
                {
                    PatientId = request.PatientId,
                    Description = request.Description,
                };

                await _unitOfWork.Repository<PatientMedicalHistory>().AddAsync(medicalHistory, cancellationToken);

                await _unitOfWork.CompleteAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception)
            {
                return Result.Failure(GenericErrors<Patient>.FailedToAdd);
            }

        }

        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var medicalHistory = await _unitOfWork.Repository<PatientMedicalHistory>().GetByIdAsync(id, cancellationToken);

                if (medicalHistory is not { })
                    return Result.Failure(GenericErrors<PatientMedicalHistory>.NotFound);

                _unitOfWork.Repository<PatientMedicalHistory>().Delete(medicalHistory);

                await _unitOfWork.CompleteAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception)
            {
                return Result.Failure(GenericErrors<PatientMedicalHistory>.FailedToDelete);
            }

        }

        public async Task<Result<IReadOnlyList<PatientMedicalHistoryResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var spec = new PatientMedicalHistorySpecification();

            var medicalHistories = await _unitOfWork.Repository<PatientMedicalHistory>().GetAllWithSpecAsync(spec, cancellationToken);

            var response = medicalHistories.Select(m => new PatientMedicalHistoryResponse
            {
                Id = m.Id,
                Description = m.Description,
                PatientId = m.PatientId,
                PatientName = m.Patient.FullName,
                CreatedOn = m.CreatedOn,
                CreatedBy = $"{m.CreatedBy?.FirstName} {m.CreatedBy?.LastName}",
                UpdatedOn = m.UpdatedOn,
                UpdatedBy = m.UpdatedBy != null ?
                    $"{m.UpdatedBy.FirstName} {m.UpdatedBy.LastName}" :
                    string.Empty

            }).ToList().AsReadOnly();

            return Result.Success<IReadOnlyList<PatientMedicalHistoryResponse>>(response);
        }

        public async Task<Result<PatientMedicalHistoryResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var spec = new PatientMedicalHistorySpecification(id);

            var medicalHistory = await _unitOfWork.Repository<PatientMedicalHistory>().GetByIdWithSpecAsync(spec, cancellationToken);

            if (medicalHistory is not { })
                return Result.Failure<PatientMedicalHistoryResponse>(GenericErrors<PatientMedicalHistory>.NotFound);

            var response = new PatientMedicalHistoryResponse
            {
                Id = medicalHistory.Id,
                Description = medicalHistory.Description,
                PatientId = medicalHistory.PatientId,
                PatientName = medicalHistory.Patient.FullName,
                CreatedOn = medicalHistory.CreatedOn,
                CreatedBy = $"{medicalHistory.CreatedBy?.FirstName} {medicalHistory.CreatedBy?.LastName}",
                UpdatedOn = medicalHistory.UpdatedOn,
                UpdatedBy = medicalHistory.UpdatedBy != null ?
                    $"{medicalHistory.UpdatedBy.FirstName} {medicalHistory.UpdatedBy.LastName}" :
                    string.Empty
            };

            return Result.Success(response);
        }

        public async Task<Result<IReadOnlyList<PatientMedicalHistoryResponse>>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
        {
            var spec = new AllPatientHistoriesSpecification(patientId);

            var medicalHistories = await _unitOfWork.Repository<PatientMedicalHistory>().GetAllWithSpecAsync(spec, cancellationToken);

            var response = medicalHistories.Select(m => new PatientMedicalHistoryResponse
            {
                Id = m.Id,
                Description = m.Description,
                PatientId = m.PatientId,
                PatientName = m.Patient.FullName,
                CreatedOn = m.CreatedOn,
                CreatedBy = $"{m.CreatedBy?.FirstName} {m.CreatedBy?.LastName}",
                UpdatedOn = m.UpdatedOn,
                UpdatedBy = m.UpdatedBy != null ?
                    $"{m.UpdatedBy.FirstName} {m.UpdatedBy.LastName}" :
                    string.Empty
            }).ToList().AsReadOnly();

            return Result.Success<IReadOnlyList<PatientMedicalHistoryResponse>>(response);
        }

        public async Task<Result> UpdateAsync(int id, PatientMedicalHistoryRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var medicalHistory = await _unitOfWork.Repository<PatientMedicalHistory>().GetByIdAsync(id, cancellationToken);

                if (medicalHistory is not { })
                    return Result.Failure(GenericErrors<PatientMedicalHistory>.NotFound);

                medicalHistory.Description = request.Description;

                _unitOfWork.Repository<PatientMedicalHistory>().Update(medicalHistory);

                await _unitOfWork.CompleteAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception)
            {
                return Result.Failure(GenericErrors<PatientMedicalHistory>.FailedToUpdate);
            }
        }
    }
}
