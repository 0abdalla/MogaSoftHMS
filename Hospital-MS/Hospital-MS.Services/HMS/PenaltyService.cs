using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.AccountTree;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using Hospital_MS.Core.Models.HR;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Services.HMS
{
    public class PenaltyService: IPenaltyService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PenaltyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<EmployeePenaltyDto> GetAllEmployeePenaltiesData(PagingFilterModel SearchModel)
        {
            var query = from penalty in _unitOfWork.Repository<Penalty>().GetAll()
                        join emp in _unitOfWork.Repository<Staff>().GetAll() on penalty.StaffId equals emp.Id
                        join penaltyType in _unitOfWork.Repository<PenaltyType>().GetAll() on penalty.PenaltyTypeId equals penaltyType.PenaltyTypeId
                        select new EmployeePenaltyDto
                        {
                            EmployeeId = penalty.StaffId,
                            EmployeeName = emp.FullName,
                            PenaltyId = penalty.PenaltyId,
                            PenaltyTypeId = penalty.PenaltyTypeId,
                            ExecutionDate = penalty.ExecutionDate,
                            PenaltyDate = penalty.PenaltyDate,
                            TotalDeduction = penalty.TotalDeduction,
                            DeductionByDays = penalty.DeductionByDays,
                            DeductionAmount = penalty.DeductionAmount,
                            Reason = penalty.Reason,
                            WorkflowStatusId = penalty.WorkflowStatusId,
                            CreatedBy = penalty.CreatedBy,
                            CreatedDate = penalty.CreatedDate,
                            ModifiedBy = penalty.ModifiedBy,
                            ModifiedDate = penalty.ModifiedDate,
                            PenaltyType = penaltyType.NameEN,
                        };
            int totalCount = query.Count();
            if (SearchModel.CurrentPage > 0 && SearchModel.PageSize > 0)
            {
                int skip = (SearchModel.CurrentPage - 1) * SearchModel.PageSize;
                query = query.Skip(skip).Take(SearchModel.PageSize);
            }

            var results = query.ToList();
            results.ForEach(x => x.TotalCount = totalCount);
            return results;
        }

        public List<EmployeePenaltyDto> GetPenaltiesByEmployeeId(int EmployeeId, PagingFilterModel SearchModel)
        {
            var query = from penalty in _unitOfWork.Repository<Penalty>().GetAll()
                        join emp in _unitOfWork.Repository<Staff>().GetAll() on penalty.StaffId equals emp.Id
                        join penaltyType in _unitOfWork.Repository<PenaltyType>().GetAll() on penalty.PenaltyTypeId equals penaltyType.PenaltyTypeId
                        where penalty.StaffId == EmployeeId
                        select new EmployeePenaltyDto
                        {
                            EmployeeId = penalty.StaffId,
                            EmployeeName = emp.FullName,
                            PenaltyId = penalty.PenaltyId,
                            PenaltyTypeId = penalty.PenaltyTypeId,
                            ExecutionDate = penalty.ExecutionDate,
                            PenaltyDate = penalty.PenaltyDate,
                            TotalDeduction = penalty.TotalDeduction,
                            DeductionByDays = penalty.DeductionByDays,
                            DeductionAmount = penalty.DeductionAmount,
                            Reason = penalty.Reason,
                            WorkflowStatusId = penalty.WorkflowStatusId,
                            CreatedBy = penalty.CreatedBy,
                            CreatedDate = penalty.CreatedDate,
                            ModifiedBy = penalty.ModifiedBy,
                            ModifiedDate = penalty.ModifiedDate,
                            PenaltyType = penaltyType.NameEN,
                        };
            int totalCount = query.Count();
            if (SearchModel.CurrentPage > 0 && SearchModel.PageSize > 0)
            {
                int skip = (SearchModel.CurrentPage - 1) * SearchModel.PageSize;
                query = query.Skip(skip).Take(SearchModel.PageSize);
            }

            var results = query.ToList();
            results.ForEach(x => x.TotalCount = totalCount);
            return results;
        }

        public async Task<ErrorResponseModel<string>> AddNewEmployeePenalty(int EmployeeId, EmployeePenaltyDto model, CancellationToken cancellationToken = default)
        {

            try
            {
                var penalty = new Penalty();

                penalty.StaffId = EmployeeId;
                penalty.PenaltyTypeId = model.PenaltyTypeId.Value;
                penalty.ExecutionDate = model.ExecutionDate.Value;
                penalty.PenaltyDate = DateTime.Now;
                penalty.TotalDeduction = model.TotalDeduction.Value;
                penalty.DeductionByDays = model.DeductionByDays.Value;
                penalty.DeductionAmount = model.DeductionAmount ?? 0;
                penalty.Reason = model.Reason;
                penalty.WorkflowStatusId = (int)HRWorkflowStatus.Approved;
                penalty.CreatedBy = model.CreatedBy;
                penalty.CreatedDate = DateTime.Now;

                await _unitOfWork.Repository<Penalty>().AddAsync(penalty, cancellationToken);
                await _unitOfWork.CompleteAsync();


                return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess);
            }
            catch (Exception ex)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }

        }

        public async Task<ErrorResponseModel<string>> EditEmployeePenalty(int EmployeeId, EmployeePenaltyDto model, CancellationToken cancellationToken = default)
        {

            try
            {
                var penalty = await _unitOfWork.Repository<Penalty>().GetByIdAsync(model.PenaltyId.Value);
                if (penalty != null)
                {
                    penalty.PenaltyTypeId = model.PenaltyTypeId.Value;
                    penalty.ExecutionDate = model.ExecutionDate.Value;
                    penalty.TotalDeduction = model.TotalDeduction.Value;
                    penalty.DeductionByDays = model.DeductionByDays.Value;
                    penalty.DeductionAmount = model.DeductionAmount;
                    penalty.Reason = model.Reason;
                    penalty.WorkflowStatusId = model.WorkflowStatusId;
                    penalty.ModifiedBy = model.ModifiedBy;
                    penalty.ModifiedDate = DateTime.Now;

                    await _unitOfWork.CompleteAsync();


                    return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess);
                }
                else
                    return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);
            }
            catch (Exception ex)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }

        }


        public async Task<ErrorResponseModel<string>> DeleteEmployeePenalty(int PenaltyId)
        {

            try
            {
                var penalty = await _unitOfWork.Repository<Penalty>().GetByIdAsync(PenaltyId);
                if (penalty != null)
                {
                    _unitOfWork.Repository<Penalty>().Delete(penalty);
                    _unitOfWork.CompleteAsync();
                    return ErrorResponseModel<string>.Success(GenericErrors.DeleteSuccess);
                }
                else
                    return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);
            }
            catch (Exception ex)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }

        }

        public async Task<ContractDetail> GetEmployeeContractDetails(int EmployeeId)
        {
            var contract = await _unitOfWork.Repository<ContractDetail>().GetByIdAsync(EmployeeId);
            return contract ?? new ContractDetail();
        }

        public async Task<List<SelectorDataModel>> GetActiveEmployeesSelector()
        {
            var result = await _unitOfWork.Repository<Staff>().GetAll(i => i.Status == StaffStatus.Active).Include(i => i.Branches).ToListAsync();
            var data = result.Select(i => new SelectorDataModel
            {
                Id = i.Id,
                Name = i.FullName,
                BranchId = i.BranchId,
                BranchName = i.Branches.Name,
                VacationDays = i.VacationDays,
            }).ToList();

            return data;
        }

        public async Task<List<SelectorDataModel>> GetPenaltyTypesSelector()
        {
            var result = await _unitOfWork.Repository<PenaltyType>().GetAllAsync();
            var data = result.Select(type => new SelectorDataModel
            {
                Id = type.PenaltyTypeId,
                Name = type.NameEN
            }).ToList();

            return data;
        }
    }
}
