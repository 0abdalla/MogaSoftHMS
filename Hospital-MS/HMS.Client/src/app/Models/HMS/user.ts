export interface User {
    userId: string;      
    token: string;
    firstName: string;
    lastName: string;
    email: string;
    address: string;
    isActive: boolean;
    expiresIn: number;
    loginDate: string | null;
}  