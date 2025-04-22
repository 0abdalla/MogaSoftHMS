using Hospital_MS.Core.Abstractions;
using Hospital_MS.Core.Contracts.Staff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Services
{
    public interface IStaffService
    {
        Task<Result> CreateAsync(CreateStaffRequest request, CancellationToken cancellationToken = default);
        Task<Result<IReadOnlyList<StaffResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Result<StaffResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Result<IReadOnlyList<StaffResponse>>> GetFilteredStaffAsync(GetStaffRequest request, CancellationToken cancellationToken = default);
        Task<int> GetFilteredStaffCountAsync(GetStaffRequest request, CancellationToken cancellationToken = default);
        Task<Result<StaffCountsResponse>> GetStaffCountsAsync(CancellationToken cancellationToken = default);
    }
}
