import { Injectable } from '@angular/core';
import { FilterModel } from '../Models/Generics/PagingFilterModel';
import html2pdf from 'html2pdf.js';
import { DatePipe } from '@angular/common';
declare var bootstrap: any;

@Injectable({
  providedIn: 'root'
})
export class SharedService {

  constructor(private datePipe: DatePipe) { }

  CreateFilterList(name: string, value: any): FilterModel[] {
    let filterList: FilterModel[] = [];
    let filterModel: FilterModel = {
      categoryName: name,
      itemValue: value
    };
    filterList.push(filterModel);
    return filterList;
  }

  closeModal(ModalId: string) {
    const modalElement = document.getElementById(ModalId);
    if (modalElement) {
      const modalInstance = bootstrap.Modal.getInstance(modalElement)
        || new bootstrap.Modal(modalElement);
      modalInstance.hide();
    }
  }

  getArabicDayAndTimeRange(dateString: string): string {
    const date = new Date(dateString);
    const arabicDays = ['الأحد', 'الاثنين', 'الثلاثاء', 'الأربعاء', 'الخميس', 'الجمعة', 'السبت'];
    const dayIndex = date.getDay();
    const arabicDay = arabicDays[dayIndex];
    const startHours = date.getHours();
    const startMinutes = date.getMinutes();
    const endDate = new Date(date);
    endDate.setMinutes(endDate.getMinutes() + 15);
    const endHours = endDate.getHours();
    const endMinutes = endDate.getMinutes();
    const pad = (n: number) => n.toString().padStart(2, '0');
    const startTime = `${pad(startHours)}:${pad(startMinutes)}`;
    const endTime = `${pad(endHours)}:${pad(endMinutes)}`;

    return `${arabicDay} من ${startTime} إلى ${endTime}`;
  }

  generatePdf(data: HTMLElement) {
    const date = new Date();
    const formattedDate = this.datePipe.transform(date, 'yyyy-MM-dd HH:mm:ss');
    const options = {
      margin: 0.5,
      filename: 'Appointment' + '_' + formattedDate + '.pdf',
      image: { type: 'jpeg', quality: 0.98 },
      html2canvas: { scale: 2 },
      jsPDF: { unit: 'mm', format: 'a4', orientation: 'portrait' }
    };
    // -------------Download PDF File------------------
    html2pdf().from(data).set(options).save();
    // -------------Open PDF File In New Tab------------------
    // html2pdf().from(data).set(options).toPdf().get('pdf').then((pdf) => {
    //   const pdfUrl = pdf.output('bloburl');
    //   window.open(pdfUrl, '_blank');
    // });
  }
}
