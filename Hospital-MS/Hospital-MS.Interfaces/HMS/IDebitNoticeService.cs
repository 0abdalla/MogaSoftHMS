using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.DebitNotices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Interfaces.HMS;
public interface IDebitNoticeService
{
    Task<ErrorResponseModel<string>> CreateAsync(DebitNoticeRequest request, CancellationToken cancellationToken = default);
    Task<PagedResponseModel<DataTable>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<DebitNoticeResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> UpdateAsync(int id, DebitNoticeRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
