using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.JobTitle;
using System.Data;

namespace Hospital_MS.Interfaces.HMS
{
    public interface IJobTitleService
    {
        Task<ErrorResponseModel<string>> CreateAsync(JobTitleRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> UpdateAsync(int id, JobTitleRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<JobTitleResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
        Task<PagedResponseModel<DataTable>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
