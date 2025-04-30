using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Insurances;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.Common;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Services.HMS
{
    public class InsuranceCompanyService(IUnitOfWork unitOfWork, ISQLHelper sQLHelper) : IInsuranceCompanyService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ISQLHelper _sQLHelper = sQLHelper;

        public async Task<ErrorResponseModel<int>> CreateAsync(InsuranceRequest request, CancellationToken cancellationToken = default)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var isExisted = await _unitOfWork.Repository<InsuranceCompany>().AnyAsync(x => x.Name == request.Name, cancellationToken);

                if (isExisted)
                    return ErrorResponseModel<int>.Failure(GenericErrors.AlreadyExists);

                var insuranceCompany = new InsuranceCompany
                {
                    Name = request.Name,
                    IsActive = true,
                    Email = request.Email,
                    Phone = request.Phone,
                    Code = request.Code,
                    ContractStartDate = request.ContractStartDate,
                    ContractEndDate = request.ContractEndDate,
                };

                await _unitOfWork.Repository<InsuranceCompany>().AddAsync(insuranceCompany, cancellationToken);
                await _unitOfWork.CompleteAsync(cancellationToken);

                if (request?.InsuranceCategories?.Count > 0)
                {
                    foreach (var insuranceCategory in request.InsuranceCategories)
                    {
                        var insuranceCategoryEntity = new InsuranceCategory
                        {
                            Name = insuranceCategory.Name,
                            InsuranceCompanyId = insuranceCompany.Id,
                            IsActive = true,
                            Rate = insuranceCategory.Rate
                        };

                        await _unitOfWork.Repository<InsuranceCategory>().AddAsync(insuranceCategoryEntity, cancellationToken);
                    }

                    await _unitOfWork.CompleteAsync(cancellationToken);
                }

                await transaction.CommitAsync(cancellationToken);

                return ErrorResponseModel<int>.Success(GenericErrors.AddSuccess, insuranceCompany.Id);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                return ErrorResponseModel<int>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var insurance = await _unitOfWork.Repository<InsuranceCompany>().GetByIdAsync(id, cancellationToken);

            if (insurance is null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            insurance.IsActive = false;

            _unitOfWork.Repository<InsuranceCompany>().Update(insurance);

            int saveResult = await _unitOfWork.CompleteAsync(cancellationToken);

            if (saveResult > 0)
                return ErrorResponseModel<string>.Success(GenericErrors.DeleteSuccess);

            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }

        public async Task<ErrorResponseModel<IReadOnlyList<InsuranceResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var insurances = _unitOfWork.Repository<InsuranceCompany>().GetAll().Include(x => x.Categories);

            if (insurances is null)
                return ErrorResponseModel<IReadOnlyList<InsuranceResponse>>.Failure(GenericErrors.NotFound);

            var response = insurances.Select(insurance => new InsuranceResponse
            {
                Id = insurance.Id,
                Name = insurance.Name,
                InsuranceCategories = insurance.Categories.Select(x => new InsuranceCategoryResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Rate = x.Rate

                }).ToList(),

                Code = insurance.Code,
                ContractStartDate = insurance.ContractStartDate,
                ContractEndDate = insurance.ContractEndDate,
                Email = insurance.Email,
                Phone = insurance.Phone,
                IsActive = insurance.IsActive

            }).ToList().AsReadOnly();

            return ErrorResponseModel<IReadOnlyList<InsuranceResponse>>.Success(GenericErrors.GetSuccess, response);
        }

        //public async Task<PagedResponseModel<DataTable>> GetAllAsync(CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        string query = "SP_GetAllCompanyInsurances";

        //        var result = await _sQLHelper.ExecuteDataTableAsync(query);

        //        return PagedResponseModel<DataTable>.Success(GenericErrors.GetSuccess, result.Rows.Count, result);
        //    }
        //    catch (Exception)
        //    {
        //        return PagedResponseModel<DataTable>.Failure(GenericErrors.TransFailed);
        //    }
        //}

        public async Task<ErrorResponseModel<InsuranceResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var insurance = await _unitOfWork.Repository<InsuranceCompany>().GetAll()
                .Include(x => x.Categories)
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (insurance is null)
                return ErrorResponseModel<InsuranceResponse>.Failure(GenericErrors.NotFound);

            var response = new InsuranceResponse
            {
                Id = insurance.Id,
                Name = insurance.Name,
                InsuranceCategories = [.. insurance.Categories.Select(x => new InsuranceCategoryResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Rate = x.Rate
                })],

                Code = insurance.Code,
                ContractStartDate = insurance.ContractStartDate,
                ContractEndDate = insurance.ContractEndDate,
                Email = insurance.Email,
                Phone = insurance.Phone,
                IsActive = insurance.IsActive,

                CreatedOn = insurance.CreatedOn,
                CreatedBy = $"{insurance.CreatedBy?.FirstName} {insurance.CreatedBy?.LastName}",
                UpdatedOn = insurance.UpdatedOn,
                UpdatedBy = insurance.UpdatedBy != null ? $"{insurance.UpdatedBy.FirstName} {insurance.UpdatedBy.LastName}" : string.Empty
            };

            return ErrorResponseModel<InsuranceResponse>.Success(GenericErrors.GetSuccess, response);
        }

        public async Task<ErrorResponseModel<string>> UpdateAsync(int id, InsuranceRequest request, CancellationToken cancellationToken = default)
        {
            var insurance = await _unitOfWork.Repository<InsuranceCompany>()
                .GetAll(x => x.Id == id)
                .Include(x => x.Categories)
                .Include(x=>x.CreatedBy)
                .Include(x=>x.UpdatedBy)
                .FirstOrDefaultAsync(cancellationToken);

            if (insurance is null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            var isExisted = await _unitOfWork.Repository<InsuranceCompany>().AnyAsync(x => x.Name == request.Name && x.Id != id, cancellationToken);

            if (isExisted)
                return ErrorResponseModel<string>.Failure(GenericErrors.AlreadyExists);

            insurance.Name = request.Name;
            insurance.Email = request.Email;
            insurance.Phone = request.Phone;
            insurance.Code = request.Code;
            insurance.ContractStartDate = request.ContractStartDate;
            insurance.ContractEndDate = request.ContractEndDate;

            if (request?.InsuranceCategories?.Count > 0)
            {
                // Remove all existing categories
                var existingCategories = insurance.Categories.ToList();

                _unitOfWork.Repository<InsuranceCategory>().DeleteRange(existingCategories);

                // Add new categories
                foreach (var categoryRequest in request.InsuranceCategories)
                {
                    var newCategory = new InsuranceCategory
                    {
                        Name = categoryRequest.Name,
                        Rate = categoryRequest.Rate,
                        InsuranceCompanyId = insurance.Id,
                        IsActive = true
                    };
                    await _unitOfWork.Repository<InsuranceCategory>().AddAsync(newCategory, cancellationToken);
                }
            }

            _unitOfWork.Repository<InsuranceCompany>().Update(insurance);

            int saveResult = await _unitOfWork.CompleteAsync(cancellationToken);

            if (saveResult > 0)
                return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess);

            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }
}
