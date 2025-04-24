using Hospital_MS.Core.Common;
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
        Task<ErrorResponseModel<PatientResponse>> GetAllAsync(GetPatientsRequest request, CancellationToken cancellationToken = default);
        Task<int> GetPatientsCountAsync(GetPatientsRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<PatientCountsResponse>> GetCountsAsync(CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> UpdateStatusAsync(int id, UpdatePatientStatusRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<PatientResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
