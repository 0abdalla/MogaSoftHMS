using Hospital_MS.Core.Abstractions;
using Hospital_MS.Core.Common;
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
        Task<ErrorResponseModel<string>> CreateAsync(DoctorRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<DoctorResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<List<AllDoctorsResponse>>> GetAllAsync(GetDoctorsRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> UpdateAsync(int id, DoctorRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<DoctorsCountResponse>> GetCountsAsync(CancellationToken cancellationToken = default);
    }
}
