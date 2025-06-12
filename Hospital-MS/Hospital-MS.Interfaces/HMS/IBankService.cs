using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Banks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Interfaces.HMS;
public interface IBankService
{
    Task<ErrorResponseModel<string>> CreateAsync(BankRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> UpdateAsync(int id, BankRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResponseModel<DataTable>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<BankResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<ErrorResponseModel<BankStatementResponse>> GetBankStatementAsync(int bankId, DateOnly fromDate, DateOnly toDate, CancellationToken cancellationToken);
}
