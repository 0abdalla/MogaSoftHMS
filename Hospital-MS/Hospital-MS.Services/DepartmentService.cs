using Hospital_MS.Core.Abstractions;
using Hospital_MS.Core.Contracts.Departments;
using Hospital_MS.Core.Errors;
using Hospital_MS.Core.Models;
using Hospital_MS.Core.Repositories;
using Hospital_MS.Core.Services;

namespace Hospital_MS.Services
{
    public class DepartmentService(IUnitOfWork unitOfWork) : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> CreateAsync(CreateDepartmentRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var department = new Department
                {
                    Name = request.Name,
                };

                await _unitOfWork.Repository<Department>().AddAsync(department, cancellationToken);

                await _unitOfWork.CompleteAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception)
            {
                return Result.Failure(GenericErrors<Department>.FailedToAdd);

            }
        }

        public async Task<Result<IReadOnlyList<DepartmentResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var departments = await _unitOfWork.Repository<Department>().GetAllAsync(cancellationToken);

            var response = departments.Select(d => new DepartmentResponse
            {
                Id = d.Id,
                Name = d.Name,

            }).ToList().AsReadOnly();

            return Result.Success<IReadOnlyList<DepartmentResponse>>(response);
        }
    }
}
