using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Departments;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Hospital_MS.Services.HMS
{
    public class DepartmentService(IUnitOfWork unitOfWork) : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorResponseModel<string>> CreateAsync(CreateDepartmentRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var department = new Department
                {
                    Name = request.Name,
                    Description = request.Description,
                };

                await _unitOfWork.Repository<Department>().AddAsync(department, cancellationToken);

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
                var department = await _unitOfWork.Repository<Department>().GetByIdAsync(id, cancellationToken);
                if (department == null)
                {
                    return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);
                }

                _unitOfWork.Repository<Department>().Delete(department);
                await _unitOfWork.CompleteAsync(cancellationToken);
                return ErrorResponseModel<string>.Success(GenericErrors.DeleteSuccess);
            }
            catch (Exception)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ErrorResponseModel<List<DepartmentResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var departments = await _unitOfWork.Repository<Department>()
                .GetAll()
                .ToListAsync(cancellationToken: cancellationToken);

            var response = departments.Select(d => new DepartmentResponse
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description

            }).ToList();

            return ErrorResponseModel<List<DepartmentResponse>>.Success(GenericErrors.GetSuccess, response);
        }

        public async Task<ErrorResponseModel<DepartmentResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var department = await _unitOfWork.Repository<Department>().GetByIdAsync(id, cancellationToken);

            if (department == null)
            {
                return ErrorResponseModel<DepartmentResponse>.Failure(GenericErrors.NotFound);
            }

            var response = new DepartmentResponse
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description
            };
            return ErrorResponseModel<DepartmentResponse>.Success(GenericErrors.GetSuccess, response);

        }

        public async Task<ErrorResponseModel<string>> UpdateAsync(int id, CreateDepartmentRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var department = await _unitOfWork.Repository<Department>().GetByIdAsync(id, cancellationToken);
                if (department == null)
                {
                    return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);
                }

                department.Name = request.Name;
                department.Description = request.Description;

                _unitOfWork.Repository<Department>().Update(department);
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
