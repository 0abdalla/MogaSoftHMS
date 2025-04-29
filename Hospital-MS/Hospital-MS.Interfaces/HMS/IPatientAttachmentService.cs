using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Interfaces.HMS
{
    public interface IPatientAttachmentService
    {
        Task<ErrorResponseModel<string>> CreateAsync(int patientId, PatientAttachmentRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<List<PatientAttachmentResponse>>> GetAllAsync(int patientId, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
