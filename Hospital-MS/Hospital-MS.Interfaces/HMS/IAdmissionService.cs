using Hospital_MS.Core.Abstractions;
using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Admissions;
using Hospital_MS.Core.Contracts.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Services
{
    public interface IAdmissionService
    {
        Task<ErrorResponseModel<string>> CreateAsync(CreateAdmissionRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<AdmissionResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        //Task<Result<IReadOnlyList<PatientResponse>>> GetAllAsync(GetPatientsRequest request, CancellationToken cancellationToken = default);

        //Task<int> GetAdmissionsCountAsync(GetPatientsRequest request, CancellationToken cancellationToken = default);

        //Task<Result<AdmissionCountsResponse>> GetCountsAsync(CancellationToken cancellationToken = default);
    }
}
