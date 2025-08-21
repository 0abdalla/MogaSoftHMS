using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Wards;

namespace Hospital_MS.Interfaces.HMS
{
    public interface IWardService
    {
        Task<ErrorResponseModel<string>> CreateAsync(CreateWardRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<List<WardResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<WardResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> UpdateAsync(int id, CreateWardRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);

    }
}
