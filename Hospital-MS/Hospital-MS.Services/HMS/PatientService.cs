using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Patients;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.Common;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Hospital_MS.Services.HMS
{
    public class PatientService(IUnitOfWork unitOfWork, ISQLHelper sQLHelper) : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ISQLHelper _sQLHelper = sQLHelper;

        public async Task<PagedResponseModel<DataTable>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default)
        {
            try
            {
                var Params = new SqlParameter[6];
                var Status = pagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Status")?.ItemValue;
                var FromDate = pagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Date")?.FromDate;
                var ToDate = pagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Date")?.ToDate;
                Params[0] = new SqlParameter("@SearchText", pagingFilter.SearchText ?? (object)DBNull.Value);
                Params[1] = new SqlParameter("@Status", Status ?? (object)DBNull.Value);
                Params[2] = new SqlParameter("@FromDate", FromDate ?? (object)DBNull.Value);
                Params[3] = new SqlParameter("@ToDate", ToDate ?? (object)DBNull.Value);
                Params[4] = new SqlParameter("@CurrentPage", pagingFilter.CurrentPage);
                Params[5] = new SqlParameter("@PageSize", pagingFilter.PageSize);
                var dt = await _sQLHelper.ExecuteDataTableAsync("dbo.SP_GetAllPatients", Params);
                int totalCount = 0;
                if (dt.Rows.Count > 0)
                {
                    int.TryParse(dt.Rows[0]["TotalCount"]?.ToString(), out totalCount);
                }

                return PagedResponseModel<DataTable>.Success(GenericErrors.GetSuccess, totalCount, dt);
            }
            catch (Exception)
            {
                return PagedResponseModel<DataTable>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ErrorResponseModel<PatientResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var patient = await _unitOfWork.Repository<Patient>().GetAll(i => i.Id == id).Include(i => i.InsuranceCompany).Include(i => i.InsuranceCategory)
                .Include(x => x.CreatedBy).Include(x => x.UpdatedOn).FirstOrDefaultAsync(cancellationToken);

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

        public async Task<PagedResponseModel<DataTable>> GetCountsAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default)
        {
            try
            {
                var Params = new SqlParameter[4];
                var Status = pagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Status")?.ItemValue;
                var FromDate = pagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Date")?.FromDate;
                var ToDate = pagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Date")?.ToDate;
                Params[0] = new SqlParameter("@SearchText", pagingFilter.SearchText ?? (object)DBNull.Value);
                Params[1] = new SqlParameter("@Status", Status ?? (object)DBNull.Value);
                Params[2] = new SqlParameter("@FromDate", FromDate ?? (object)DBNull.Value);
                Params[3] = new SqlParameter("@ToDate", ToDate ?? (object)DBNull.Value);
                var dt = await _sQLHelper.ExecuteDataTableAsync("dbo.SP_GetPatientStatusCountStatistics", Params);

                return PagedResponseModel<DataTable>.Success(GenericErrors.GetSuccess, 6, dt);
            }
            catch (Exception)
            {
                return PagedResponseModel<DataTable>.Failure(GenericErrors.TransFailed);
            } 
        }

        public async Task<ErrorResponseModel<string>> UpdateStatusAsync(int id, UpdatePatientStatusRequest request, CancellationToken cancellationToken = default)
        {
            var patient = await _unitOfWork.Repository<Patient>().GetAll(i => i.Id == id).FirstOrDefaultAsync();

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
