export interface InsuranceCompany {
    id: number;
    name: string;
    code: string;
    contactNumber: string;
    email: string;
    status: 'Active' | 'Inactive';
    registrationDate?: Date;
    processedClaimsCount?: number;
    contractStartDate: Date;
    contractEndDate: Date;
    insuranceCategories: { name: string; rate: number }[];
    isActive:any;
}  