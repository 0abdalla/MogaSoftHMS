import { Component, ElementRef, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { SharedService } from '../../../../Services/shared.service';
import jsPDF from 'jspdf';
import html2canvas from 'html2canvas';

@Component({
  selector: 'app-print-invoice',
  templateUrl: './print-invoice.component.html',
  styleUrl: './print-invoice.component.css'
})
export class PrintInvoiceComponent {
  userName: any;
  currentDate = new Date();
  @ViewChild('printSection', { static: false }) printSectionRef: ElementRef;

  // invoiceData: any;
  @Output() viewReady = new EventEmitter<void>();

  ngAfterViewInit() {
  this.viewReady.emit();
}
    @Input() invoiceData: any;

  constructor(private sharedService: SharedService) {
    this.userName = sessionStorage.getItem('firstName') + ' ' + sessionStorage.getItem('lastName');
  }

  generatePdf() {
    setTimeout(() => {
      this.sharedService.generatePdf(this.printSectionRef.nativeElement);
    }, 500);
  }
downloadPDF() {
  if (!this.printSectionRef) {
    console.error("printSectionRef not ready yet!");
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
