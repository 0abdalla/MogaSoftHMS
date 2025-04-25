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
    public interface IPatientService
    {
        Task<PagedResponseModel<DataTable>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default);
        Task<PagedResponseModel<DataTable>> GetCountsAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> UpdateStatusAsync(int id, UpdatePatientStatusRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<PatientResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
