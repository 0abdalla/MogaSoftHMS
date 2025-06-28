import { CreatorModel } from "./CreatorModel";
import { EmployeeModel } from "./EmployeeModel";

export interface EmployeeVerificationModel extends CreatorModel {
    employeeVerificationId: number | null;
    employeeId: number | null;
    bankId: number | null;
    bankAccountNumber: string;
    iqamaNumber: string;
    iqamaIssuePlaceId: number | null;
    iqamaIssueDate: string | null;
    iqamaExpireDate: string | null;
    drivingLicenseNumber: string;
    drivingLicenseIssueDate: string | null;
    drivingLicenseExpireDate: string | null;
    vehicleId: number | null;
    vehicleNumber: string;
    vehicleCode: number | null;
    employee: EmployeeModel;
}