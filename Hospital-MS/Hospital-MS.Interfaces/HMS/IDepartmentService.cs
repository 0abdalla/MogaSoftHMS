using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Departments;

namespace Hospital_MS.Interfaces.HMS
{
    public interface IDepartmentService
    {
        Task<ErrorResponseModel<string>> CreateAsync(CreateDepartmentRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<List<DepartmentResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<DepartmentResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> UpdateAsync(int id, CreateDepartmentRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
