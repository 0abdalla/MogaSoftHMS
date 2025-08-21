using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.JobDepartment;
using System.Data;

namespace Hospital_MS.Interfaces.HMS
{
    public interface IJobDepartmentService
    {
        Task<ErrorResponseModel<string>> CreateAsync(JobDepartmentRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> UpdateAsync(int id, JobDepartmentRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<JobDepartmentResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
        Task<PagedResponseModel<DataTable>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
