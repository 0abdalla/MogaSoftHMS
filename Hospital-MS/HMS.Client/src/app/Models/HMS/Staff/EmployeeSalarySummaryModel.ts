export interface EmployeeSalarySummaryModel {
    totalCount: number | null;
    employeeId: number | null;
    employeeCode: string;
    bankAccountNumber: string;
    bank: string;
    jobTitle: string;
    employeeNameEN: string;
    employeeNameAR: string;
    branchId: number | null;
    branchNameEN: string;
    branchNameAR: string;
    jobNameEN: string;
    jobNameAR: string;
    deductions: number | null;
    advances: number | null;
    penalties: number | null;
    overtime: number | null;
    grossSalary: number | null;
    totalSalary: number | null;
    basicSalary: number | null;
    extraSalary: number | null;
    transportation: number | null;
    housingAllowance: number | null;
    mobileAllowance: number | null;
    workNature: number | null;
    mealAllowance: number | null;
    other: number | null;
    dailySalary: number | null;
    totalDeductions: number | null;
    calculatedSalary: number | null;
    netSalary: number | null;
    presentDays: number | null;
    offDays: number | null;
    sickDays: number | null;
    absentDays: number | null;
    totalWorkingDays: number | null;
    isPaid: boolean | null;
    isGossi: boolean | null;
    totalGrossSalary: number | null;
    hRManagerApprove: boolean | null;
    financeManagerApprove: boolean | null;
    auditingApprove: boolean | null;
    executiveManagerApprove: boolean | null;
    generalManagerApprove: boolean | null;
    visaPayment: number | null;
    cashPayment: number | null;
}