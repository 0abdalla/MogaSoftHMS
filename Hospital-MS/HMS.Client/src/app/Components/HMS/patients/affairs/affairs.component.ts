import { Component } from '@angular/core';

@Component({
  selector: 'app-affairs',
  templateUrl: './affairs.component.html',
  styleUrl: './affairs.component.css'
})
export class AffairsComponent {
  patientStatuses = [
    { name: 'إقامة', count: 20, color: '#1E90FF' },
    { name: 'عمليات', count: 10, color: '#8B0000' },
    { name: 'حالة حرجة', count: 5, color: '#FF0000' },
    { name: 'تم علاجه', count: 30, color: '#008000' },
    { name: 'أرشيف', count: 25, color: '#808080' },
    { name: 'عيادات خارجية', count: 15, color: '#FFA500' },
  ];

  patients = [
    { id: 1, name: 'محمد أحمد', age: 30, status: 'إقامة', lastInteraction: '1/4/2025', notes: ['محتاج مكالمة من الدكتور'], phone: '01012345678' },
    { id: 2, name: 'سلمى علي', age: 25, status: 'عيادات خارجية', lastInteraction: '3/4/2025', notes: ['تم حجز موعد متابعة'], phone: '01023456789' },
    { id: 3, name: 'أحمد عبد الله', age: 40, status: 'حالة حرجة', lastInteraction: '2/4/2025', notes: ['تم نقله إلى العناية المركزة'], phone: '01034567890' },
    { id: 4, name: 'ليلى محمود', age: 35, status: 'عمليات', lastInteraction: '30/3/2025', notes: ['تحتاج تقييم بعد العملية'], phone: '01045678901' },
    { id: 5, name: 'خالد يوسف', age: 50, status: 'تم علاجه', lastInteraction: '25/3/2025', notes: ['تم إخلاء المريض بنجاح'], phone: '01056789012' }
  ];  

  
  getStatusColor(status: string): string {
    const statusObj = this.patientStatuses.find((s) => s.name === status);
    return statusObj ? statusObj.color : '#000';
  }

  filters = { name: '', status: '' };

  filteredPatients = [...this.patients];

  applyFilters(event: Event) {
    event.preventDefault();
    const { name, status } = this.filters;

    this.filteredPatients = this.patients.filter((patient) => {
      const matchesName = name ? patient.name.includes(name) : true;
      const matchesStatus = status ? patient.status === status : true;
      return matchesName && matchesStatus;
    });
  }
  resetFilters() {
    this.filters = { name: '', status: '' };
    this.filteredPatients = [...this.patients];
  }
  selectedPatient = this.patients[0]; // مؤقتًا أول مريض
  newNote = '';

addNote() {
  if (!this.newNote.trim()) return;

  if (!this.selectedPatient.notes) {
    this.selectedPatient.notes = [];
  }

  this.selectedPatient.notes.unshift(this.newNote.trim());
  this.newNote = '';
}
}
