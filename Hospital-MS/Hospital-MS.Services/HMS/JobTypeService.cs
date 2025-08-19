using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Common;
using Hospital_MS.Core.Contracts.JobType;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Extensions;
using Hospital_MS.Core.Models.HR;
using Hospital_MS.Interfaces.Common;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Hospital_MS.Services.HMS
{
    public class JobTypeService(IUnitOfWork unitOfWork, ISQLHelper sQLHelper) : IJobTypeService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ISQLHelper _sQLHelper = sQLHelper;

        public async Task<ErrorResponseModel<string>> CreateAsync(JobTypeRequest request, CancellationToken cancellationToken = default)
        {
            var isExists = await _unitOfWork.Repository<JobType>()
               .AnyAsync(x => x.Name == request.Name, cancellationToken);

            if (isExists)
                return ErrorResponseModel<string>.Failure(GenericErrors.AlreadyExists);

            if (!Enum.TryParse<StatusTypes>(request.Status, true, out var status))
                return ErrorResponseModel<string>.Failure(GenericErrors.InvalidStatus);


            var type = new JobType
            {
                Name = request.Name,
                Description = request.Description,
                Status = status
            };

            try
            {
                await _unitOfWork.Repository<JobType>().AddAsync(type, cancellationToken);
                await _unitOfWork.CompleteAsync(cancellationToken);
                return ErrorResponseModel<string>.Success(GenericErrors.GetSuccess);
            }
            catch (Exception)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ErrorResponseModel<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var jobType = await _unitOfWork.Repository<JobType>()
                .GetAll(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
            if (jobType is null)
                return ErrorResponseModel<bool>.Failure(GenericErrors.NotFound);
            try
            {
                _unitOfWork.Repository<JobType>().Delete(jobType);
                await _unitOfWork.CompleteAsync(cancellationToken);
                return ErrorResponseModel<bool>.Success(GenericErrors.GetSuccess);
            }
            catch (Exception)
            {
                return ErrorResponseModel<bool>.Failure(GenericErrors.TransFailed);
            }

        }

        public async Task<PagedResponseModel<DataTable>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default)
        {
            try
            {
                var Params = new SqlParameter[6];
                var Status = pagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Status")?.ItemValue;
                var FromDate = pagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Date")?.FromDate;
                var ToDate = pagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Date")?.ToDate;
                var SearchText = pagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "SearchText")?.ItemValue;
                Params[0] = new SqlParameter("@SearchText", SearchText ?? (object)DBNull.Value);
                Params[1] = new SqlParameter("@Status", Status ?? (object)DBNull.Value);
                Params[2] = new SqlParameter("@FromDate", FromDate ?? (object)DBNull.Value);
                Params[3] = new SqlParameter("@ToDate", ToDate ?? (object)DBNull.Value);
                Params[4] = new SqlParameter("@CurrentPage", pagingFilter.CurrentPage);
                Params[5] = new SqlParameter("@PageSize", pagingFilter.PageSize);
                var dt = await _sQLHelper.ExecuteDataTableAsync("dbo.SP_GetAllJobTypes", Params);
                int totalCount = 0;
                if (dt.Rows.Count > 0)
                {
                    int.TryParse(dt.Rows[0]["TotalCount"]?.ToString(), out totalCount);
                }

                foreach (DataRow row in dt.Rows)
                {
                    row.TryTranslateEnum<StatusTypes>("Status");
                }

                return PagedResponseModel<DataTable>.Success(GenericErrors.GetSuccess, totalCount, dt);
            }
            catch (Exception)
            {
                return PagedResponseModel<DataTable>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ErrorResponseModel<JobTypeResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var jobType = await _unitOfWork.Repository<JobType>()
                .GetAll(x => x.Id == id)
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .FirstOrDefaultAsync(cancellationToken);

            if (jobType is not { })
                return ErrorResponseModel<JobTypeResponse>.Failure(GenericErrors.NotFound);

            var response = new JobTypeResponse
            {
                Id = jobType.Id,
                Name = jobType.Name,
                Description = jobType.Description,
                Status = jobType.Status.GetArabicValue(),
                Audit = new AuditResponse
                {
                    CreatedBy = $"{jobType.CreatedBy.FirstName} {jobType.CreatedBy.LastName}",
                    CreatedOn = jobType.CreatedOn,
                    UpdatedBy = $"{jobType?.UpdatedBy?.FirstName} {jobType?.UpdatedBy?.LastName}",
                    UpdatedOn = jobType?.UpdatedOn,
                }
            };

            return ErrorResponseModel<JobTypeResponse>.Success(GenericErrors.GetSuccess, response);
        }

        public async Task<ErrorResponseModel<string>> UpdateAsync(int id, JobTypeRequest request, CancellationToken cancellationToken = default)
        {
            var jobType = await _unitOfWork.Repository<JobType>()
                .GetAll(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (jobType is null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            if (!Enum.TryParse<StatusTypes>(request.Status, true, out var newStatus))
                return ErrorResponseModel<string>.Failure(GenericErrors.InvalidStatus);

            jobType.Name = request.Name;
            jobType.Description = request.Description;
            jobType.Status = newStatus;


            try
            {
                _unitOfWork.Repository<JobType>().Update(jobType);
                await _unitOfWork.CompleteAsync(cancellationToken);
                return ErrorResponseModel<string>.Success(GenericErrors.GetSuccess);
            }
            catch (Exception)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }

        }
    }
}
