using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Clinics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Interfaces.HMS
{
    public interface IClinicService
    {
        Task<ErrorResponseModel<string>> CreateAsync(CreateClinicRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<List<ClinicResponse>>> GetAllAsync(CancellationToken cancellationToken = default);

    }
}
