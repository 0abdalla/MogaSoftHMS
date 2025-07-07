using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.DailyRestrictions;
using Hospital_MS.Core.Contracts.FiscalYears;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Interfaces.HMS;
public interface IFiscalYearService
{
    Task<ErrorResponseModel<string>> CreateAsync(FiscalYearRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<FiscalYearResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResponseModel<List<FiscalYearResponse>>> GetAllAsync(PagingFilterModel filter, CancellationToken cancellationToken = default);
}
