import { Component, ElementRef, Input, ViewChild } from '@angular/core';
import jsPDF from 'jspdf';
import html2canvas from 'html2canvas';

@Component({
  selector: 'app-print-invoice',
  templateUrl: './print-invoice.component.html',
  styleUrls: ['./print-invoice.component.css']
})
export class PrintInvoiceComponent {
  @ViewChild('printSection', { static: false }) printSectionRef!: ElementRef;
  @Input() invoiceData: any;
  userName = sessionStorage.getItem('firstName') + ' ' + sessionStorage.getItem('lastName');
  currentDate = new Date();

  downloadPDF() {
    if (!this.printSectionRef) {
      console.error('printSectionRef مش موجود!');
      return;
    }

    const DATA = this.printSectionRef.nativeElement;
    html2canvas(DATA, { scale: 2 }).then(canvas => {
      const fileWidth = 210;
      const fileHeight = (canvas.height * fileWidth) / canvas.width;
      const FILEURI = canvas.toDataURL('image/png');
      const PDF = new jsPDF('p', 'mm', 'a4');

      PDF.addImage(FILEURI, 'PNG', 0, 0, fileWidth, fileHeight);
      PDF.save('invoice.pdf');
    });
  }
}
