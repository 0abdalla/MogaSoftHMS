export class Home{
    results!:any
    bedOccupancyRate : number;
    activeDoctors : number;
    currentPatients : number;
    appointmentsCount : number;
    currentMonthAppointments: { [key: string]: number } = {};
    previousMonthAppointments: { [key: string]: number } = {};
    // 
    completedAppointmentsRate : number;
}