import { CreatorModel } from "./CreatorModel";

export interface EmployeeAdvanceModel extends CreatorModel {
    employeeAdvanceId: number | null;
    employeeId: number | null;
    employeeName: string;
    advanceTypeNameEN: string;
    advanceTypeNameAR: string;
    advanceTypeId: number;
    advanceAmount: number;
    paymentAmount: number;
    paymentFromDate: string;
    paymentToDate: string | null;
    workflowStatusId: number | null;
    workflowStatusNameEN: string;
    workflowStatusNameAR: string;
    isApproved: boolean | null;
    notes: string;
    totalPaid: number | null;
    totalRemaining: number | null;
    totalCount: number | null;
}

export enum HRWorkflowStatus {
    Pending = 11, //معلق
    Rejected = 12,//ملغى
    Approved = 13,//مقبول
    Completed = 14,//منتهي
}