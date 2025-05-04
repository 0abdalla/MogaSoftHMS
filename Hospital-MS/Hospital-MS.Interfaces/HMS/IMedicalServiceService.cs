using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.MedicalServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Interfaces.HMS
{
    public interface IMedicalServiceService
    {
        Task<ErrorResponseModel<string>> CreateMedicalService(MedicalServiceRequest request, CancellationToken cancellationToken = default);
        Task<PagedResponseModel<DataTable>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default);
    }
}
