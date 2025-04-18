using Hospital_MS.Core.Abstractions;
using Hospital_MS.Core.Contracts.Clinics;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Errors;
using Hospital_MS.Core.Models;
using Hospital_MS.Core.Repositories;
using Hospital_MS.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Services
{
    public class ClinicService(IUnitOfWork unitOfWork) : IClinicService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> CreateAsync(CreateClinicRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                if(!Enum.TryParse<ClinicType>(request.Type,true,out var parsedType))
                    return Result.Failure(new Error("InvalidType", "Invalid Clinic Type provided.", 400));


                var clinic = new Clinic
                {
                    Name = request.Name,
                    Type = parsedType
                };

                await _unitOfWork.Repository<Clinic>().AddAsync(clinic, cancellationToken);

                await _unitOfWork.CompleteAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception)
            {
                return Result.Failure(GenericErrors<Clinic>.FailedToAdd);
            }
        }

        public async Task<Result<IReadOnlyList<ClinicResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
        {

            var clinics = await _unitOfWork.Repository<Clinic>().GetAllAsync(cancellationToken);

            var clinicResponses = clinics.Select(c => new ClinicResponse
            {
                Id = c.Id,
                Name = c.Name,
                Type = c.Type.ToString(),

            }).ToList().AsReadOnly();


            return Result.Success<IReadOnlyList<ClinicResponse>>(clinicResponses);
        }
    }
}
