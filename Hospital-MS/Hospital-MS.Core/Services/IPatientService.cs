using Hospital_MS.Core.Abstractions;
using Hospital_MS.Core.Contracts.Admissions;
using Hospital_MS.Core.Contracts.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Services
{
    public interface IPatientService
    {
        Task<Result<IReadOnlyList<PatientResponse>>> GetAllAsync(GetPatientsRequest request, CancellationToken cancellationToken = default);
        Task<int> GetPatientsCountAsync(GetPatientsRequest request, CancellationToken cancellationToken = default);
        Task<Result<PatientCountsResponse>> GetCountsAsync(CancellationToken cancellationToken = default);
        Task<Result> UpdateStatusAsync(int id, UpdatePatientStatusRequest request, CancellationToken cancellationToken = default);

        Task<Result<PatientResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
