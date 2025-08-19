using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.JobType;
using System.Data;

namespace Hospital_MS.Interfaces.HMS
{
    public interface IJobTypeService
    {
        Task<ErrorResponseModel<string>> CreateAsync(JobTypeRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> UpdateAsync(int id, JobTypeRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<JobTypeResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
        Task<PagedResponseModel<DataTable>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
