import { Component, ElementRef, ViewChild } from '@angular/core';
import { SharedService } from '../../../../Services/shared.service';

@Component({
  selector: 'app-print-invoice',
  templateUrl: './print-invoice.component.html',
  styleUrl: './print-invoice.component.css'
})
export class PrintInvoiceComponent {
  @ViewChild('printSection', { static: false }) printSectionRef: ElementRef;
  invoiceData: any;

  constructor(private sharedService: SharedService) { }

  generatePdf() {
    setTimeout(() => {
      this.sharedService.generatePdf(this.printSectionRef.nativeElement);
    }, 500);
  }
}
