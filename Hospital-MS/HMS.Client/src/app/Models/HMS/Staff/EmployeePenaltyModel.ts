import { CreatorModel } from "./CreatorModel";

export interface EmployeePenaltyModel extends CreatorModel {
    penaltyId: number;
    employeeId: number;
    employeeName: string;
    penaltyTypeId: number;
    penaltyType: string;
    penaltyDate: string;
    executionDate: string;
    deductionByDays: number;
    moneyAmount: number;
    totalDeduction: number;
    deductionAmount: number;
    reason: string;
    isActive: boolean;
    workflowStatusId: number | null;
    workflowStatusNameEN: string;
    workflowStatusNameAR: string;
    totalCount: number | null;
}