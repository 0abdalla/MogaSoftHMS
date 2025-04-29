using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Clinics;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using Hospital_MS.Core.Services;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Services.HMS
{
    public class ClinicService(IUnitOfWork unitOfWork) : IClinicService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorResponseModel<string>> CreateAsync(CreateClinicRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!Enum.TryParse<ClinicType>(request.Type, true, out var parsedType))
                    return ErrorResponseModel<string>.Failure(GenericErrors.InvalidType);

                var clinic = new Clinic
                {
                    Name = request.Name,
                    Type = parsedType
                };

                await _unitOfWork.Repository<Clinic>().AddAsync(clinic, cancellationToken);

                await _unitOfWork.CompleteAsync(cancellationToken);

                return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess);
            }
            catch (Exception)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ErrorResponseModel<List<ClinicResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
        {

            var clinics = await _unitOfWork.Repository<Clinic>().GetAll().ToListAsync();

            var clinicResponses = clinics.Select(c => new ClinicResponse
            {
                Id = c.Id,
                Name = c.Name,
                Type = c.Type.ToString(),

            }).ToList();


            return ErrorResponseModel<List<ClinicResponse>>.Success(GenericErrors.GetSuccess, clinicResponses);
        }
    }
}
