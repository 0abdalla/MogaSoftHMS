using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.MedicalServices;
using Hospital_MS.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Interfaces.HMS
{
    public interface IMedicalServiceService
    {
        Task<ErrorResponseModel<string>> CreateMedicalService(MedicalServiceRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> CreateRadiologyBodyType(RadiologyBodyTypeRequest request, CancellationToken cancellationToken = default);
        Task<PagedResponseModel<List<MedicalServiceResponse>>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<List<RadiologyBodyType>>> GetRadiologyBodyTypes();
        Task<ErrorResponseModel<string>> UpdateAsync(int id, MedicalServiceRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<MedicalServiceResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
