using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.JobDepartment;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models.HR;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Services.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Interfaces.Common;
using Microsoft.EntityFrameworkCore;
using Hospital_MS.Core.Contracts.Common;
using Hospital_MS.Core.Contracts.JobTitle;
using Hospital_MS.Core.Extensions;
using Microsoft.Data.SqlClient;

namespace Hospital_MS.Services.HMS
{
    public class JobDepartmentService(IUnitOfWork unitOfWork, ISQLHelper sQLHelper) : IJobDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ISQLHelper _sQLHelper = sQLHelper;

        public async Task<ErrorResponseModel<string>> CreateAsync(JobDepartmentRequest request, CancellationToken cancellationToken = default)
        {
            var isExists = await _unitOfWork.Repository<JobDepartment>()
                .AnyAsync(x => x.Name == request.Name, cancellationToken);

            if (isExists)
                return ErrorResponseModel<string>.Failure(GenericErrors.AlreadyExists);

            if (!Enum.TryParse<StatusTypes>(request.Status, true, out var status))
                return ErrorResponseModel<string>.Failure(GenericErrors.InvalidStatus);

            var dep = new JobDepartment
            {
                Name = request.Name,
                Description = request.Description,
                Status = status
            };

            try
            {
                await _unitOfWork.Repository<JobDepartment>().AddAsync(dep, cancellationToken);
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
                Params[0] = new SqlParameter("@SearchText", pagingFilter.SearchText ?? (object)DBNull.Value);
                Params[1] = new SqlParameter("@Status", Status ?? (object)DBNull.Value);
                Params[2] = new SqlParameter("@FromDate", FromDate ?? (object)DBNull.Value);
                Params[3] = new SqlParameter("@ToDate", ToDate ?? (object)DBNull.Value);
                Params[4] = new SqlParameter("@CurrentPage", pagingFilter.CurrentPage);
                Params[5] = new SqlParameter("@PageSize", pagingFilter.PageSize);
                var dt = await _sQLHelper.ExecuteDataTableAsync("dbo.SP_GetAllJobDepartments", Params);
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

        public async Task<ErrorResponseModel<JobDepartmentResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var jobTitle = await _unitOfWork.Repository<JobDepartment>()
               .GetAll(x => x.Id == id)
               .Include(x => x.CreatedBy)
               .Include(x => x.UpdatedBy)
               .FirstOrDefaultAsync(cancellationToken);

            if (jobTitle is not { })
                return ErrorResponseModel<JobDepartmentResponse>.Failure(GenericErrors.NotFound);

            var response = new JobDepartmentResponse
            {
                Id = jobTitle.Id,
                Name = jobTitle.Name,
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

            return ErrorResponseModel<JobDepartmentResponse>.Success(GenericErrors.GetSuccess, response);
        }

        public async Task<ErrorResponseModel<string>> UpdateAsync(int id, JobDepartmentRequest request, CancellationToken cancellationToken = default)
        {
            var jobDepartment = await _unitOfWork.Repository<JobDepartment>()
                .GetAll(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (jobDepartment is null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            if (!Enum.TryParse<StatusTypes>(request.Status, true, out var newStatus))
                return ErrorResponseModel<string>.Failure(GenericErrors.InvalidStatus);

            jobDepartment.Name = request.Name;
            jobDepartment.Description = request.Description;
            jobDepartment.Status = newStatus;

            try
            {
                _unitOfWork.Repository<JobDepartment>().Update(jobDepartment);
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
