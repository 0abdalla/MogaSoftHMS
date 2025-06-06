﻿using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Staff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Interfaces.HMS
{
    public interface IStaffService
    {
        Task<ErrorResponseModel<string>> CreateAsync(CreateStaffRequest request, CancellationToken cancellationToken = default);
        //Task<ErrorResponseModel<StaffResponse>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<StaffResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        //Task<ErrorResponseModel<StaffResponse>> GetFilteredStaffAsync(GetStaffRequest request, CancellationToken cancellationToken = default);
        //Task<int> GetFilteredStaffCountAsync(GetStaffRequest request, CancellationToken cancellationToken = default);
        //Task<ErrorResponseModel<StaffCountsResponse>> GetStaffCountsAsync(CancellationToken cancellationToken = default);

        Task<PagedResponseModel<DataTable>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default);
        Task<PagedResponseModel<DataTable>> GetCountsAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default);
    }
}
