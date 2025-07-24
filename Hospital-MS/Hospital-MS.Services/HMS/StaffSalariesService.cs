using Hospital_MS.Core.Contracts.Staff;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;


namespace Hospital_MS.Services.HMS
{
    public class StaffSalariesService : IStaffSalariesService
    {
        private readonly IUnitOfWork _unitOfWork;
        public StaffSalariesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<List<StaffSalaryResponse>> CalculateStaffSalaries(List<StaffSalaryRequest> Model)
        {
            var StaffSalaries = new List<StaffSalaryResponse>();
            foreach (var staff in Model)
            {
                var StaffSalaryObj = new StaffSalaryResponse();
                StaffSalaryObj.Vacation = 0.00;
                var Year = staff.Date.Year;
                var Month = staff.Date.Month;
                var Entity = await _unitOfWork.Repository<Staff>().GetAll(i => i.Id == staff.StaffId)
                    .Include(i => i.EmployeeAdvances.Where(i => i.WorkflowStatusId == 13))
                    .Include(i => i.Vacations.Where(i => i.FromDate.Year == Year))
                    .Include(i => i.Penalties.Where(i => i.ExecutionDate.Month == Month && i.ExecutionDate.Year == Year))
                    .FirstOrDefaultAsync();

                if (Entity != null)
                {
                    var TaxAmount = Entity.BasicSalary * ((double)Entity.Tax / 100);
                    var InsuranceAmount = Entity.BasicSalary * ((double)Entity.Insurance / 100);
                    var NetSalary = Entity.BasicSalary - TaxAmount - InsuranceAmount;
                    var AdvancesValue = Entity.EmployeeAdvances.Where(i => i.PaymentToDate >= staff.Date).Sum(i => i.PaymentAmount);
                    var PenaltyValue = Entity.Penalties.Sum(i => i.TotalDeduction);
                    var allowedVacationDays = Entity.VacationDays;
                    var totalVacationUsedThisYear = Entity.Vacations.Sum(i => i.Period) ?? 0;
                    var vacationDaysThisMonth = Entity.Vacations.Where(i => i.FromDate.Month == Month).Sum(i => i.Period) ?? 0;
                    if (totalVacationUsedThisYear > allowedVacationDays)
                    {
                        var exceededDays = totalVacationUsedThisYear - allowedVacationDays;
                        var deductedDaysThisMonth = Math.Min(exceededDays.GetValueOrDefault(), vacationDaysThisMonth);
                        if (deductedDaysThisMonth > 0)
                        {
                            var NetVacationValue = (NetSalary / 30) * deductedDaysThisMonth;
                            StaffSalaryObj.Vacation = NetVacationValue;
                        }
                    }

                    StaffSalaryObj.Name = Entity.FullName;
                    StaffSalaryObj.Date = staff.Date;
                    StaffSalaryObj.BasicSalary = Entity.BasicSalary.Value;
                    StaffSalaryObj.SecondShift = 0.00;//  descuse this prop
                    StaffSalaryObj.Overtime = staff.Overtime;
                    StaffSalaryObj.Total = StaffSalaryObj.BasicSalary + StaffSalaryObj.SecondShift + StaffSalaryObj.Overtime;
                    StaffSalaryObj.Insurance = InsuranceAmount.Value;
                    StaffSalaryObj.DifferenceBasicDays = 0.00;  //  descuse this prop
                    StaffSalaryObj.Absence = 0.00;  //  descuse this prop
                    StaffSalaryObj.TotalDeductions = StaffSalaryObj.Insurance + StaffSalaryObj.DifferenceBasicDays + StaffSalaryObj.Absence;
                    StaffSalaryObj.Net = StaffSalaryObj.Total - StaffSalaryObj.TotalDeductions;
                    StaffSalaryObj.Taxes = TaxAmount.Value;
                    StaffSalaryObj.Penalties = PenaltyValue;
                    StaffSalaryObj.Loans = AdvancesValue;
                    StaffSalaryObj.TotalDays = staff.TotalDays;
                    StaffSalaryObj.Due = StaffSalaryObj.Net - (StaffSalaryObj.Taxes + StaffSalaryObj.Penalties.Value + StaffSalaryObj.Loans.Value + StaffSalaryObj.Vacation.Value);


                    StaffSalaries.Add(StaffSalaryObj);
                }
            }


            return StaffSalaries;
        }
    }
}
