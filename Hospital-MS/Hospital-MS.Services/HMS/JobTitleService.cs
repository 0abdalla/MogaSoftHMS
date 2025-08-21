using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Common;
using Hospital_MS.Core.Contracts.JobTitle;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Extensions;
using Hospital_MS.Core.Models;
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
    public class JobTitleService(IUnitOfWork unitOfWork, ISQLHelper sQLHelper) : IJobTitleService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ISQLHelper _sQLHelper = sQLHelper;

        public async Task<ErrorResponseModel<string>> CreateAsync(JobTitleRequest request, CancellationToken cancellationToken = default)
        {
            var isExists = await _unitOfWork.Repository<JobTitle>()
                .AnyAsync(x => x.Name == request.Name, cancellationToken);

            if (isExists)
                return ErrorResponseModel<string>.Failure(GenericErrors.AlreadyExists);

            if (!Enum.TryParse<StatusTypes>(request.Status, true, out var status))
                return ErrorResponseModel<string>.Failure(GenericErrors.InvalidStatus);

            var depIsExists = await _unitOfWork.Repository<JobDepartment>()
                .AnyAsync(x => x.Id == request.JobDepartmentId, cancellationToken);

            if (!depIsExists)
                return ErrorResponseModel<string>.Failure(new Error("هذا القسم غير موجود", Status.NotFound));

            var title = new JobTitle
            {
                Name = request.Name,
                Description = request.Description,
                JobDepartmentId = request.JobDepartmentId,
                Status = status

            };

            try
            {
                await _unitOfWork.Repository<JobTitle>().AddAsync(title, cancellationToken);
                await _unitOfWork.CompleteAsync(cancellationToken);
                return ErrorResponseModel<string>.Success(GenericErrors.GetSuccess);
            }
            catch (Exception)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ErrorResponseModel<JobTitleResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var jobTitle = await _unitOfWork.Repository<JobTitle>()
                .GetAll(x => x.Id == id)
                .Include(x => x.JobDepartment)
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .FirstOrDefaultAsync(cancellationToken);

            if (jobTitle is not { })
                return ErrorResponseModel<JobTitleResponse>.Failure(GenericErrors.NotFound);

            var response = new JobTitleResponse
            {
                Id = jobTitle.Id,
                Name = jobTitle.Name,
                JobDepartmentId = jobTitle.JobDepartmentId,
                JobDepartmentName = jobTitle?.JobDepartment?.Name,
                Description = jobTitle.Description,
                Status = jobTitle.Status.GetArabicValue(),
                Audit = new AuditResponse
                {
                    CreatedBy = $"{jobTitle.CreatedBy.FirstName} {jobTitle.CreatedBy.LastName}",
                    CreatedOn = jobTitle.CreatedOn,
                    UpdatedBy = $"{jobTitle?.UpdatedBy?.FirstName} {jobTitle?.UpdatedBy?.LastName}",
                    UpdatedOn = jobTitle?.UpdatedOn,
                }
            };

            return ErrorResponseModel<JobTitleResponse>.Success(GenericErrors.GetSuccess, response);
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
                var dt = await _sQLHelper.ExecuteDataTableAsync("dbo.SP_GetAllJobTitles", Params);
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

        public async Task<ErrorResponseModel<string>> UpdateAsync(int id, JobTitleRequest request, CancellationToken cancellationToken = default)
        {
            var jobTitle = await _unitOfWork.Repository<JobTitle>()
                .GetAll(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (jobTitle is null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            if (!Enum.TryParse<StatusTypes>(request.Status, true, out var newStatus))
                return ErrorResponseModel<string>.Failure(GenericErrors.InvalidStatus);

            var depIsExists = await _unitOfWork.Repository<Department>()
                .AnyAsync(x => x.Id == request.JobDepartmentId, cancellationToken);

            if (!depIsExists)
                return ErrorResponseModel<string>.Failure(new Error("هذا القسم غير موجود", Status.NotFound));

            jobTitle.Name = request.Name;
            jobTitle.JobDepartmentId = request.JobDepartmentId;
            jobTitle.Description = request.Description;
            jobTitle.Status = newStatus;


            try
            {
                _unitOfWork.Repository<JobTitle>().Update(jobTitle);
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
            var jobTitle = await _unitOfWork.Repository<JobTitle>()
                .GetAll(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
            if (jobTitle is null)
                return ErrorResponseModel<bool>.Failure(GenericErrors.NotFound);
            try
            {
                _unitOfWork.Repository<JobTitle>().Delete(jobTitle);
                await _unitOfWork.CompleteAsync(cancellationToken);
                return ErrorResponseModel<bool>.Success(GenericErrors.GetSuccess);
            }
            catch (Exception)
            {
                return ErrorResponseModel<bool>.Failure(GenericErrors.TransFailed);
            }
        }
    }
}
