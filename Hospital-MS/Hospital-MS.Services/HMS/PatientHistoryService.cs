using Hospital_MS.Core.Abstractions;
using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Patients;
using Hospital_MS.Core.Errors;
using Hospital_MS.Core.Models;
using Hospital_MS.Core.Services;
using Hospital_MS.Interfaces.Common;
using Hospital_MS.Interfaces.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Services.HMS
{
    public class PatientHistoryService(IUnitOfWork unitOfWork, ISQLHelper sQLHelper) : IPatientHistoryService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ISQLHelper _sQLHelper = sQLHelper;

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
                var medicalHistory = await _unitOfWork.Repository<PatientMedicalHistory>().GetAll(i => i.Id == id).FirstOrDefaultAsync();

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

        public async Task<PagedResponseModel<DataTable>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default)
        {
            try
            {
                var Params = new SqlParameter[3];
                Params[0] = new SqlParameter("@SearchText", pagingFilter.SearchText ?? (object)DBNull.Value);
                Params[2] = new SqlParameter("@CurrentPage", pagingFilter.CurrentPage);
                Params[3] = new SqlParameter("@PageSize", pagingFilter.PageSize);
                var dt = await _sQLHelper.ExecuteDataTableAsync("dbo.SP_GetAllPatientMedicalHistory", Params);
                int totalCount = 0;
                if (dt.Rows.Count > 0)
                    int.TryParse(dt.Rows[0]["TotalCount"]?.ToString(), out totalCount);

                return PagedResponseModel<DataTable>.Success(GenericErrors.GetSuccess, totalCount, new List<DataTable?> { dt });
            }
            catch (Exception)
            {
                return PagedResponseModel<DataTable>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ErrorResponseModel<PatientMedicalHistoryResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var spec = new PatientMedicalHistory();

            var medicalHistory = await _unitOfWork.Repository<PatientMedicalHistory>().GetAll(i => i.Id == id).Include(x => x.Patient).Include(x => x.CreatedBy).Include(x => x.UpdatedBy).FirstOrDefaultAsync();

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
            var m = await _unitOfWork.Repository<PatientMedicalHistory>().GetAll(i => i.PatientId == patientId).Include(x => x.Patient).Include(x => x.CreatedBy).Include(x => x.UpdatedBy).FirstOrDefaultAsync();

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
                var medicalHistory = await _unitOfWork.Repository<PatientMedicalHistory>().GetAll(i => i.Id == id).FirstOrDefaultAsync();

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
