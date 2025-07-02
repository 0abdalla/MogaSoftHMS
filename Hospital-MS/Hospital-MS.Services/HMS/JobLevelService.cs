using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Common;
using Hospital_MS.Core.Contracts.JobLevel;
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
    public class JobLevelService(IUnitOfWork unitOfWork, ISQLHelper sQLHelper) : IJobLevelService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ISQLHelper _sQLHelper = sQLHelper;

        public async Task<ErrorResponseModel<string>> CreateAsync(JobLevelRequest request, CancellationToken cancellationToken = default)
        {
            var isExists = await _unitOfWork.Repository<JobLevel>()
               .AnyAsync(x => x.Name == request.Name, cancellationToken);

            if (isExists)
                return ErrorResponseModel<string>.Failure(GenericErrors.AlreadyExists);

            if (!Enum.TryParse<StatusTypes>(request.Status, true, out var status))
                return ErrorResponseModel<string>.Failure(GenericErrors.InvalidStatus);


            var level = new JobLevel
            {
                Name = request.Name,
                Description = request.Description,
                Status = status
            };

            try
            {
                await _unitOfWork.Repository<JobLevel>().AddAsync(level, cancellationToken);
                await _unitOfWork.CompleteAsync(cancellationToken);
                return ErrorResponseModel<string>.Success(GenericErrors.GetSuccess);
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
                var dt = await _sQLHelper.ExecuteDataTableAsync("dbo.SP_GetAllJobLevels", Params);
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

        public async Task<ErrorResponseModel<JobLevelResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var jobLevel = await _unitOfWork.Repository<JobLevel>()
               .GetAll(x => x.Id == id)
               .Include(x => x.CreatedBy)
               .Include(x => x.UpdatedBy)
               .FirstOrDefaultAsync(cancellationToken);

            if (jobLevel is not { })
                return ErrorResponseModel<JobLevelResponse>.Failure(GenericErrors.NotFound);

            var response = new JobLevelResponse
            {
                Id = jobLevel.Id,
                Name = jobLevel.Name,
                Description = jobLevel.Description,
                Status = jobLevel.Status.GetArabicValue(),
                Audit = new AuditResponse
                {
                    CreatedBy = $"{jobLevel.CreatedBy.FirstName} {jobLevel.CreatedBy.LastName}",
                    CreatedOn = jobLevel.CreatedOn,
                    UpdatedBy = $"{jobLevel?.UpdatedBy?.FirstName} {jobLevel?.UpdatedBy?.LastName}",
                    UpdatedOn = jobLevel?.UpdatedOn,
                }
            };

            return ErrorResponseModel<JobLevelResponse>.Success(GenericErrors.GetSuccess, response);
        }

        public async Task<ErrorResponseModel<string>> UpdateAsync(int id, JobLevelRequest request, CancellationToken cancellationToken = default)
        {
            var jobLevel = await _unitOfWork.Repository<JobLevel>()
               .GetAll(x => x.Id == id)
               .FirstOrDefaultAsync(cancellationToken);

            if (jobLevel is null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            if (!Enum.TryParse<StatusTypes>(request.Status, true, out var newStatus))
                return ErrorResponseModel<string>.Failure(GenericErrors.InvalidStatus);

            jobLevel.Name = request.Name;
            jobLevel.Description = request.Description;
            jobLevel.Status = newStatus;

            try
            {
                _unitOfWork.Repository<JobLevel>().Update(jobLevel);
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
