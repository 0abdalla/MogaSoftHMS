using Hospital_MS.Core.Abstractions;
using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Patients;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Services
{
    public interface IPatientHistoryService
    {
        Task<ErrorResponseModel<string>> CreateAsync(PatientMedicalHistoryRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> UpdateAsync(int id, PatientMedicalHistoryRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<PatientMedicalHistoryResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<PagedResponseModel<DataTable>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<PatientMedicalHistoryResponse>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
    }
}
