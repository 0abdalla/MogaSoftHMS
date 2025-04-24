using Hospital_MS.Core.Abstractions;
using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Patients;
using Hospital_MS.Core.Errors;
using Hospital_MS.Core.Models;
using Hospital_MS.Core.Services;
using Hospital_MS.Core.Specifications.MedicalHistories;
using Hospital_MS.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Services.HMS
{
    public class PatientHistoryService(IUnitOfWork unitOfWork) : IPatientHistoryService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorResponseModel<string>> CreateAsync(PatientMedicalHistoryRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var patientIsExist = await _unitOfWork.Repository<Patient>().AnyAsync(x => x.Id == request.PatientId, cancellationToken);

                if (!patientIsExist)
                    return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

                var medicalHistory = new PatientMedicalHistory
                {
                    PatientId = request.PatientId,
                    Description = request.Description,
                };

                await _unitOfWork.Repository<PatientMedicalHistory>().AddAsync(medicalHistory, cancellationToken);

                await _unitOfWork.CompleteAsync(cancellationToken);

                return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess);
            }
            catch (Exception)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }

        }

        public async Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var medicalHistory = await _unitOfWork.Repository<PatientMedicalHistory>().GetByIdAsync(id, cancellationToken);

                if (medicalHistory is not { })
                    return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

                _unitOfWork.Repository<PatientMedicalHistory>().Delete(medicalHistory);

                await _unitOfWork.CompleteAsync(cancellationToken);

                return ErrorResponseModel<string>.Success(GenericErrors.DeleteSuccess);
            }
            catch (Exception)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }

        }

        public async Task<ErrorResponseModel<PatientMedicalHistoryResponse>> GetAllAsync(CancellationToken cancellationToken = default)
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

            return ErrorResponseModel<PatientMedicalHistoryResponse>.Success(GenericErrors.GetSuccess);
        }

        public async Task<ErrorResponseModel<PatientMedicalHistoryResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var spec = new PatientMedicalHistorySpecification(id);

            var medicalHistory = await _unitOfWork.Repository<PatientMedicalHistory>().GetByIdWithIncludesAsync(i => i.Id == id, cancellationToken, x => x.Patient, x => x.CreatedBy, x => x.UpdatedBy);

            if (medicalHistory is not { })
                return ErrorResponseModel<PatientMedicalHistoryResponse>.Failure(GenericErrors.NotFound);

            var response = new PatientMedicalHistoryResponse
            {
                Id = medicalHistory.Id,
                Description = medicalHistory.Description,
                PatientId = medicalHistory.PatientId,
                PatientName = medicalHistory.Patient.FullName,
                CreatedOn = medicalHistory.CreatedOn,
                CreatedBy = $"{medicalHistory.CreatedBy?.FirstName} {medicalHistory.CreatedBy?.LastName}",
                UpdatedOn = medicalHistory.UpdatedOn,
                UpdatedBy = medicalHistory.UpdatedBy != null ? $"{medicalHistory.UpdatedBy.FirstName} {medicalHistory.UpdatedBy.LastName}" : string.Empty
            };

            return ErrorResponseModel<PatientMedicalHistoryResponse>.Success(GenericErrors.GetSuccess, response);
        }

        public async Task<ErrorResponseModel<PatientMedicalHistoryResponse>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
        {
            var m = await _unitOfWork.Repository<PatientMedicalHistory>().GetByIdWithIncludesAsync(i => i.PatientId == patientId, cancellationToken, x => x.Patient, x => x.CreatedBy, x => x.UpdatedBy);

            var response = new PatientMedicalHistoryResponse
            {
                Id = m.Id,
                Description = m.Description,
                PatientId = m.PatientId,
                PatientName = m.Patient.FullName,
                CreatedOn = m.CreatedOn,
                CreatedBy = $"{m.CreatedBy?.FirstName} {m.CreatedBy?.LastName}",
                UpdatedOn = m.UpdatedOn,
                UpdatedBy = m.UpdatedBy != null ? $"{m.UpdatedBy.FirstName} {m.UpdatedBy.LastName}" : string.Empty
            };

            return ErrorResponseModel<PatientMedicalHistoryResponse>.Success(GenericErrors.GetSuccess, response);
        }

        public async Task<ErrorResponseModel<string>> UpdateAsync(int id, PatientMedicalHistoryRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var medicalHistory = await _unitOfWork.Repository<PatientMedicalHistory>().GetByIdAsync(id, cancellationToken);

                if (medicalHistory is not { })
                    return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

                medicalHistory.Description = request.Description;

                _unitOfWork.Repository<PatientMedicalHistory>().Update(medicalHistory);

                await _unitOfWork.CompleteAsync(cancellationToken);

                return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess);
            }
            catch (Exception)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }
    }
}
