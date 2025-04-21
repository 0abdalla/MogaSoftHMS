using Hospital_MS.Core.Abstractions;
using Hospital_MS.Core.Contracts.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Services
{
    public interface IPatientHistoryService
    {
        Task<Result> CreateAsync(PatientMedicalHistoryRequest request, CancellationToken cancellationToken = default);
        Task<Result> UpdateAsync(int id, PatientMedicalHistoryRequest request, CancellationToken cancellationToken = default);
        Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<Result<PatientMedicalHistoryResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Result<IReadOnlyList<PatientMedicalHistoryResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Result<IReadOnlyList<PatientMedicalHistoryResponse>>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
    }
}
