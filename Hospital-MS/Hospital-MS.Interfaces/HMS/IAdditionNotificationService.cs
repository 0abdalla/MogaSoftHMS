using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.AdditionNotifications;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Interfaces.HMS;
public interface IAdditionNotificationService
{
    Task<ErrorResponseModel<string>> CreateAsync(AdditionNotificationRequest request, CancellationToken cancellationToken = default);
    Task<PagedResponseModel<DataTable>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<AdditionNotificationResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> UpdateAsync(int id, AdditionNotificationRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
