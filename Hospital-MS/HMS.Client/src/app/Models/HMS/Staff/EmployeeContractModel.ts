import { CreatorModel } from "./CreatorModel";
import { EmployeeModel } from "./EmployeeModel";

export interface EmployeeContractModel extends CreatorModel {
    contractId: number | null;
    startDate: string;
    endDate: string;
    contractPeriodYears: number;
    vacationPeriodDays: number | null;
    vacationDate: string | null;
    isGossi: boolean;
    basicSalary: number;
    extraSalary: number;
    transportation: number;
    housingAllowance: number;
    mobileAllowance: number;
    workNature: number;
    mealAllowance: number;
    other: number | null;
    totalSalary: number | null;
    employeeId: number | null;
    branchId: number | null;
    employee: EmployeeModel | null;
}