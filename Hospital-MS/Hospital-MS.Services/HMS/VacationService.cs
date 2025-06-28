using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.AccountTree;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using Hospital_MS.Core.Models.HR;
using Hospital_MS.Interfaces.Common;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Services.HMS
{
    public class VacationService: IVacationService
    {
        private readonly ISQLHelper _sQLHelper;
        private readonly IUnitOfWork _unitOfWork;
        public VacationService(ISQLHelper sQLHelper, IUnitOfWork unitOfWork)
        {
            _sQLHelper = sQLHelper;
            _unitOfWork = unitOfWork;
        }

        public List<EmployeeVacationDto> GetAllEmployeeVacationsData(PagingFilterModel SearchModel)
        {
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@CurrentPage", SearchModel.CurrentPage);
            param[1] = new SqlParameter("@PageSize", SearchModel.PageSize);
            var result = _sQLHelper.SQLQuery<EmployeeVacationDto>("[HR].[SP_GetEmployeeVacationsData]", param);
            return result;
        }

        public List<EmployeeVacationDto> GetVacationsByEmployeeId(int employeeId, PagingFilterModel SearchModel)
        {
            var query = from vacation in _unitOfWork.Repository<Vacation>().GetAll()
                        join emp in _unitOfWork.Repository<Staff>().GetAll() on vacation.StaffId equals emp.Id
                        where vacation.StaffId == employeeId
                        select new EmployeeVacationDto
                        {
                            EmployeeId = emp.Id,
                            EmployeeName = emp.FullName,
                            VacationId = vacation.VacationId,
                            VacationTypeId = vacation.VacationTypeId,
                            WorkflowStatusId = vacation.WorkflowStatusId,
                            FromDate = vacation.FromDate,
                            ToDate = vacation.ToDate,
                            LastDayWork = vacation.LastDayWork,
                            Period = vacation.Period,
                            Notes = vacation.Notes
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
        public async Task<ErrorResponseModel<string>> AddNewEmployeeVacation(int EmployeeId, EmployeeVacationDto model, CancellationToken cancellationToken = default)
        {
            try
            {
                var vacation = new Vacation();
                vacation.StaffId = EmployeeId;
                vacation.FromDate = model.FromDate;
                vacation.ToDate = model.ToDate;
                vacation.LastDayWork = model.LastDayWork;
                vacation.VacationTypeId = model.VacationTypeId;
                vacation.Period = (model.ToDate - model.FromDate).Days;
                vacation.Notes = model.Notes;
                vacation.CreatedDate = DateTime.Now;
                vacation.CreatedBy = model.CreatedBy;

                await _unitOfWork.Repository<Vacation>().AddAsync(vacation, cancellationToken);
                await _unitOfWork.CompleteAsync();


                return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess);
            }
            catch (Exception ex)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }

        }

        public async Task<ErrorResponseModel<string>> EditVacation(int EmployeeId, EmployeeVacationDto model)
        {
            try
            {
                var vacation = await _unitOfWork.Repository<Vacation>().GetByIdAsync(model.VacationId.Value);
                if (vacation != null)
                {
                    vacation.FromDate = model.FromDate;
                    vacation.ToDate = model.ToDate;
                    vacation.LastDayWork = model.LastDayWork;
                    vacation.Period = (model.ToDate - model.FromDate).Days;
                    vacation.Notes = model.Notes;
                    vacation.ModifiedDate = DateTime.Now;
                    vacation.ModifiedBy = model.ModifiedBy;

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

        public async Task<ErrorResponseModel<string>> DeleteVacation(int VacationId)
        {
            try
            {
                var Vacation = await _unitOfWork.Repository<Vacation>().GetByIdAsync(VacationId);
                if (Vacation != null)
                {
                    _unitOfWork.Repository<Vacation>().Delete(Vacation);
                    await _unitOfWork.CompleteAsync();
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


        public async Task<ErrorResponseModel<string>> ApproveEmployeeVacation(int VacationId, int EmployeeId, bool ApproveStatus)
        {
            try
            {
                var vacation = await _unitOfWork.Repository<Vacation>().GetAll(i => i.VacationId == VacationId && i.StaffId == EmployeeId).FirstOrDefaultAsync();

                if (vacation != null)
                {
                    vacation.WorkflowStatusId = ApproveStatus ? (int)HRWorkflowStatus.Approved : (int)HRWorkflowStatus.Rejected; ;
                    vacation.ModifiedBy = string.Empty;
                    vacation.ModifiedDate = DateTime.Now;

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

        public List<SelectorDataModel> GetVacationTypesSelector()
        {
            var results = _unitOfWork.Repository<VacationType>().GetAll().Select(i => new SelectorDataModel
            {
                Id = i.VacationTypeId,
                Name = i.NameAR
            }).ToList();

            return results;
        }
    }
}
