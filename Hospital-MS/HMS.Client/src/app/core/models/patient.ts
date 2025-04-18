export interface PaginatedPatients {
    pageSize: number;
    pageIndex: number;
    count: number;
    data: Patients[];
}

export interface Patients {
    id: number;
    patientName: string;
    age: number;
    service: string;
    clinicId: number;
    clinicName: string;
    doctorId: number;
    doctorName: string;
    date: string;
    time: string;
    status: string;
    type: string;
}