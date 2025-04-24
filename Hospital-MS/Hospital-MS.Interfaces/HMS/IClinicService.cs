using Hospital_MS.Core.Abstractions;
using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Clinics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Services
{
    public interface IClinicService
    {
        Task<ErrorResponseModel<string>> CreateAsync(CreateClinicRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<ClinicResponse>> GetAllAsync(CancellationToken cancellationToken = default);

    }
}
