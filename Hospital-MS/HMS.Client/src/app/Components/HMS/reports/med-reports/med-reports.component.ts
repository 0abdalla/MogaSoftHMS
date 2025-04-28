import { Component } from '@angular/core';
declare var html2pdf: any;

@Component({
  selector: 'app-med-reports',
  templateUrl: './med-reports.component.html',
  styleUrl: './med-reports.component.css'
})
export class MedReportsComponent {
  activeTab = 'appointments';

  appointments = [
    { id: 1, patient: 'محمد علي', doctor: 'د. أحمد', date: '2025-04-01', status: 'مكتملة' },
    { id: 2, patient: 'سارة حسن', doctor: 'د. منى', date: '2025-04-02', status: 'ملغاة' }
  ];

  patients = [
    { id: 1, name: 'محمد علي', age: 30, gender: 'ذكر', visits: 5 },
    { id: 2, name: 'سارة حسن', age: 25, gender: 'أنثى', visits: 2 }
  ];

  doctors = [
    { id: 1, name: 'د. أحمد', specialty: 'باطنة', cases: 10 },
    { id: 2, name: 'د. منى', specialty: 'جلدية', cases: 7 }
  ];

  exportAppointmentsPDF() {
    const element = document.getElementById('pdfAppointmentContent');
  
    const opt = {
      margin:       0.5,
      filename:     'appointment-details.pdf',
      image:        { type: 'jpeg', quality: 1 },
      html2canvas:  { scale: 2 },
      jsPDF:        { unit: 'in', format: 'a4', orientation: 'portrait' }
    };
  
    html2pdf().set(opt).from(element).save();
  }

  exportPatientsPDF() {
    const element = document.getElementById('pdfPatientContent');
  
    const opt = {
      margin:       0.5,
      filename:     'patient-details.pdf',
      image:        { type: 'jpeg', quality: 1 },
      html2canvas:  { scale: 2 },
      jsPDF:        { unit: 'in', format: 'a4', orientation: 'portrait' }
    };
  
    html2pdf().set(opt).from(element).save();
  }

  exportDoctorsPDF() {
    const element = document.getElementById('pdfDoctorContent');
  
    const opt = {
      margin:       0.5,
      filename:     'doctor-details.pdf',
      image:        { type: 'jpeg', quality: 1 },
      html2canvas:  { scale: 2 },
      jsPDF:        { unit: 'in', format: 'a4', orientation: 'portrait' }
    };
  
    html2pdf().set(opt).from(element).save();
  }
}
