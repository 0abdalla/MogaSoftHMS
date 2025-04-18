using Hospital_MS.Core.Abstractions;
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
        Task<Result> CreateAsync(CreateClinicRequest request, CancellationToken cancellationToken = default);
        Task<Result<IReadOnlyList<ClinicResponse>>> GetAllAsync(CancellationToken cancellationToken = default);

    }
}
