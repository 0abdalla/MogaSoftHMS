
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
        Task<PagedResponseModel<List<AdmissionResponse>>> GetAllAsync(PagingFilterModel pagingFilter,CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<AdmissionResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> UpdateAsync(int id,UpdateAdmissionRequest request,CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> CreateAsync(CreateAdmissionRequest request, CancellationToken cancellationToken = default);
        //Task<ErrorResponseModel<AdmissionResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<IReadOnlyList<PatientAdmissionsResponse>>> GetPatientAdmissionsByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
