using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Staff;
using Hospital_MS.Core.Models;
using Hospital_MS.Core.Models.HR;
using Hospital_MS.Interfaces.Common;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;


namespace Hospital_MS.Services.HMS
{
    public class StaffSalariesService : IStaffSalariesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISQLHelper _sQLHelper;
        public StaffSalariesService(IUnitOfWork unitOfWork, ISQLHelper sQLHelper)
        {
            _unitOfWork = unitOfWork;
            _sQLHelper = sQLHelper;
        }


        public async Task<ErrorResponseModel<List<EmployeeSalary>>> CalculateStaffSalaries(DateTime Date, CancellationToken cancellationToken)
        {
            try
            {
                var isCalcedBefore = await _unitOfWork.Repository<EmployeeSalary>().CountAsync(i => i.Date.Year == Date.Year && i.Date.Month == Date.Month, cancellationToken) > 0;
                if (isCalcedBefore)
                    return ErrorResponseModel<List<EmployeeSalary>>.Failure(GenericErrors.CalcStaffSalaries);

                var AllEmployees = await _unitOfWork.Repository<Staff>().GetAll(i => i.Status == Core.Enums.StaffStatus.Active)
                        .Include(i => i.EmployeeAdvances.Where(i => i.WorkflowStatusId == 13))
                        .Include(i => i.AttendaceSalaries.Where(i => i.Date.Value.Month == Date.Month && i.Date.Value.Year == Date.Year))
                        .ToListAsync();

                var StaffSalaries = new List<EmployeeSalary>();
                foreach (var staff in AllEmployees)
                {
                    var StaffSalaryObj = new EmployeeSalary();
                    var AdvancesValue = staff.EmployeeAdvances.Where(i => i.PaymentToDate >= Date).Sum(i => i.PaymentAmount);
                    StaffSalaryObj.EmployeeCode = staff.Id;
                    StaffSalaryObj.StaffId = staff.Id;
                    StaffSalaryObj.VisaCode = staff.VisaCode;
                    StaffSalaryObj.Name = staff.FullName;
                    StaffSalaryObj.Date = Date;
                    StaffSalaryObj.BasicSalary = staff.BasicSalary.HasValue ? staff.BasicSalary.Value : 0;
                    var TaxAmount = StaffSalaryObj.BasicSalary * ((staff.Tax.HasValue ? (double)staff.Tax.Value : 0) / 100);
                    var InsuranceAmount = StaffSalaryObj.BasicSalary * ((staff.Insurance.HasValue ? (double)staff.Insurance.Value : 0) / 100);
                    StaffSalaryObj.DailyRate = Math.Round((StaffSalaryObj.BasicSalary / 30), 2);
                    if (staff.AttendaceSalaries != null && staff.AttendaceSalaries.Count > 0)
                        StaffSalaryObj.AttendanceDays = staff.AttendaceSalaries.FirstOrDefault().TotalDays.Value;
                    else
                        StaffSalaryObj.AttendanceDays = 0;
                    StaffSalaryObj.Total = Math.Round(StaffSalaryObj.AttendanceDays * StaffSalaryObj.DailyRate, 2);
                    StaffSalaryObj.Insurance = InsuranceAmount;
                    StaffSalaryObj.NetSalary = Math.Round(StaffSalaryObj.Total - StaffSalaryObj.Insurance, 2);
                    StaffSalaryObj.Taxes = TaxAmount;
                    StaffSalaryObj.Advances = AdvancesValue;
                    StaffSalaryObj.AmountDue = Math.Floor(StaffSalaryObj.NetSalary - StaffSalaryObj.Taxes - StaffSalaryObj.Advances);
                    StaffSalaries.Add(StaffSalaryObj);
                }

                return ErrorResponseModel<List<EmployeeSalary>>.Success(GenericErrors.AddSuccess, StaffSalaries);
            }
            catch (Exception ex)
            {
                return ErrorResponseModel<List<EmployeeSalary>>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ErrorResponseModel<bool>> AddStaffSalaries(List<EmployeeSalary> Salaries, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.Repository<EmployeeSalary>().AddRangeAsync(Salaries, cancellationToken);
                await _unitOfWork.CompleteAsync();
                return ErrorResponseModel<bool>.Success(GenericErrors.AddSuccess);
            }
            catch (Exception ex)
            {
                return ErrorResponseModel<bool>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<PagedResponseModel<DataTable>> GetAllStaffSalariesAsync(PagingFilterModel filter)
        {
            try
            {
                var searchText = filter.FilterList?.FirstOrDefault(i => i.CategoryName == "SearchText")?.ItemValue;
                var Date = filter.FilterList?.FirstOrDefault(i => i.CategoryName == "Date")?.ItemValue;
                if (Date != null)
                    Date = Date + "-01";

                var parameters = new[]
                {
                new SqlParameter("@SearchText", searchText ?? (object)DBNull.Value),
                new SqlParameter("@CurrentPage", filter.CurrentPage),
                new SqlParameter("@PageSize", filter.PageSize),
                new SqlParameter("@Date", Date),
                };

                var dt = await _sQLHelper.ExecuteDataTableAsync("[finance].[SP_GetAllStaffSalaries]", parameters);

                int totalCount = dt.Rows.Count > 0 && dt.Columns.Contains("TotalCount")
                    ? dt.Rows[0].Field<int?>("TotalCount") ?? 0
                    : dt.Rows.Count;

                return PagedResponseModel<DataTable>.Success(GenericErrors.GetSuccess, totalCount, dt);
            }
            catch (Exception)
            {
                return PagedResponseModel<DataTable>.Failure(GenericErrors.TransFailed);
            }
        }
    }
}
