﻿using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Doctors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Interfaces.HMS
{
    public interface IDoctorService
    {
        Task<ErrorResponseModel<string>> CreateAsync(DoctorRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<DoctorResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
       // Task<PagedResponseModel<DataTable>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> UpdateAsync(int id, DoctorRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<PagedResponseModel<DataTable>> GetCountsAsync(CancellationToken cancellationToken = default);

        Task<PagedResponseModel<List<AllDoctorsResponse>>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default);
        Task ResetCurrentAppointmentsAsync();
    }
}
