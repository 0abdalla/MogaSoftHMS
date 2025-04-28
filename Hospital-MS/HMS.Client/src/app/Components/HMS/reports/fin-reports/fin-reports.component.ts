import { Component } from '@angular/core';
declare var html2pdf: any;

@Component({
  selector: 'app-fin-reports',
  templateUrl: './fin-reports.component.html',
  styleUrl: './fin-reports.component.css'
})
export class FinReportsComponent {
  activeTab = 'billing';

  billings = [
    { id: 1, patient: 'محمد علي', amount: 500, date: '2025-04-05', status: 'مدفوع' },
    { id: 2, patient: 'سارة حسن', amount: 300, date: '2025-04-06', status: 'غير مدفوع' }
  ];

  attendance = [
    { id: 1, employee: 'أحمد محمود', date: '2025-04-01', in: '09:00', out: '17:00' },
    { id: 2, employee: 'منى السيد', date: '2025-04-01', in: '09:15', out: '16:45' }
  ];

  exportBillingPDF() {
    const element = document.getElementById('pdfBillContent');
  
    const opt = {
      margin:       0.5,
      filename:     'bill-details.pdf',
      image:        { type: 'jpeg', quality: 1 },
      html2canvas:  { scale: 2 },
      jsPDF:        { unit: 'in', format: 'a4', orientation: 'portrait' }
    };
  
    html2pdf().set(opt).from(element).save();
  } 
  exportAttendancePDF() {
    const element = document.getElementById('pdfAttendanceContent');
  
    const opt = {
      margin:       0.5,
      filename:     'attendance-details.pdf',
      image:        { type: 'jpeg', quality: 1 },
      html2canvas:  { scale: 2 },
      jsPDF:        { unit: 'in', format: 'a4', orientation: 'portrait' }
    };
  
    html2pdf().set(opt).from(element).save();
  } 

}
