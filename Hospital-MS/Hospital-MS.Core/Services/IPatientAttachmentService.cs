using Hospital_MS.Core.Abstractions;
using Hospital_MS.Core.Contracts.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Services
{
    public interface IPatientAttachmentService
    {
        Task<Result> CreateAsync(int patientId, PatientAttachmentRequest request, CancellationToken cancellationToken = default);
        Task<Result<IReadOnlyList<PatientAttachmentResponse>>> GetAllAsync(int patientId, CancellationToken cancellationToken = default);
        Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
