using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Beds;

namespace Hospital_MS.Interfaces.HMS
{
    public interface IBedService
    {
        Task<ErrorResponseModel<string>> CreateAsync(CreateBedRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<List<BedResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<BedResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> UpdateAsync(int id, CreateBedRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
