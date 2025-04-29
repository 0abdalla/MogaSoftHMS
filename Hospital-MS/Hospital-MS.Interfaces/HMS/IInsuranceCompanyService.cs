using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Insurances;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Interfaces.HMS
{
    public interface IInsuranceCompanyService
    {
        Task<ErrorResponseModel<int>> CreateAsync(InsuranceRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<InsuranceResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> UpdateAsync(int id, InsuranceRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<IReadOnlyList<InsuranceResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
        // Task<PagedResponseModel<DataTable>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
