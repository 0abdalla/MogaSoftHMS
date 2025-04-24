using Hospital_MS.Core.Abstractions;
using Hospital_MS.Core.Contracts.Doctors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Services
{
    public interface IDoctorService
    {
        Task<Result> CreateAsync(DoctorRequest request, CancellationToken cancellationToken = default);
        Task<Result<DoctorResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Result<IReadOnlyList<AllDoctorsResponse>>> GetAllAsync(GetDoctorsRequest request,CancellationToken cancellationToken = default);
        Task<int> GetAllCountAsync(GetDoctorsRequest request, CancellationToken cancellationToken = default);
        Task<Result> UpdateAsync(int id, DoctorRequest request, CancellationToken cancellationToken = default);
        Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<Result<DoctorsCountResponse>> GetCountsAsync(CancellationToken cancellationToken = default);
    }
}
