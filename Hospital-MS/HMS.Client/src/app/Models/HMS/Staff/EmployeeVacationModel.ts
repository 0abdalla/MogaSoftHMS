export interface EmployeeVacationModel {
    employeeId: number;
    employeeName: string;
    vacationId: number;
    vacationTypeId: number;
    vacationType: string;
    notes: string;
    fromDate: string | null;
    toDate: string | null;
    lastDayWork: string | null;
    period: number | null;
    workflowStatusId: number | null;
    workflowStatusNameEN: string;
    workflowStatusNameAR: string;
    isApproved: boolean;
}