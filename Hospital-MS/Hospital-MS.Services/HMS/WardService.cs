using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Wards;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Hospital_MS.Services.HMS
{
    public class WardService(IUnitOfWork unitOfWork) : IWardService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorResponseModel<string>> CreateAsync(CreateWardRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var ward = new Ward
                {
                    Name = request.Name,
                };

                await _unitOfWork.Repository<Ward>().AddAsync(ward, cancellationToken);

                await _unitOfWork.CompleteAsync(cancellationToken);

                return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess);
            }
            catch (Exception)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var ward = await _unitOfWork.Repository<Ward>().GetByIdAsync(id, cancellationToken);
                if (ward == null)
                {
                    return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);
                }
                _unitOfWork.Repository<Ward>().Delete(ward);
                await _unitOfWork.CompleteAsync(cancellationToken);
                return ErrorResponseModel<string>.Success(GenericErrors.DeleteSuccess);
            }
            catch (Exception)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }

        }

        public async Task<ErrorResponseModel<List<WardResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var wards = await _unitOfWork.Repository<Ward>()
                .GetAll()
                .OrderByDescending(i => i.Id)
                .ToListAsync(cancellationToken);

            var response = wards.Select(ward => new WardResponse
            {
                Id = ward.Id,
                Name = ward.Name,
            }).ToList();

            return ErrorResponseModel<List<WardResponse>>.Success(GenericErrors.GetSuccess, response);
        }

        public async Task<ErrorResponseModel<WardResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var ward = await _unitOfWork.Repository<Ward>().GetByIdAsync(id, cancellationToken);
            if (ward == null)
            {
                return ErrorResponseModel<WardResponse>.Failure(GenericErrors.NotFound);
            }
            var response = new WardResponse
            {
                Id = ward.Id,
                Name = ward.Name,
            };
            return ErrorResponseModel<WardResponse>.Success(GenericErrors.GetSuccess, response);

        }

        public async Task<ErrorResponseModel<string>> UpdateAsync(int id, CreateWardRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var ward = await _unitOfWork.Repository<Ward>().GetByIdAsync(id, cancellationToken);
                if (ward == null)
                {
                    return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);
                }
                ward.Name = request.Name;
                _unitOfWork.Repository<Ward>().Update(ward);
                await _unitOfWork.CompleteAsync(cancellationToken);
                return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess);
            }
            catch (Exception)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }

        }
    }
}
