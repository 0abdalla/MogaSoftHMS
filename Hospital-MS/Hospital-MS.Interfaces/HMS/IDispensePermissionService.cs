using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.DispensePermission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Interfaces.HMS;
public interface IDispensePermissionService
{
    Task<ErrorResponseModel<string>> CreateAsync(DispensePermissionRequest request, CancellationToken cancellationToken = default);
    Task<PagedResponseModel<DataTable>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<DispensePermissionResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}