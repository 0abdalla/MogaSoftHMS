import { Component, ElementRef, ViewChild } from '@angular/core';
import { SharedService } from '../../../../Services/shared.service';

@Component({
  selector: 'app-print-invoice',
  templateUrl: './print-invoice.component.html',
  styleUrl: './print-invoice.component.css'
})
export class PrintInvoiceComponent {
  userName: any;
  currentDate = new Date();
  @ViewChild('printSection', { static: false }) printSectionRef: ElementRef;
  invoiceData: any;

  constructor(private sharedService: SharedService) { 
    this.userName = sessionStorage.getItem('firstName') + ' ' + sessionStorage.getItem('lastName');    
  }

  generatePdf() {
    setTimeout(() => {
      this.sharedService.generatePdf(this.printSectionRef.nativeElement);
    }, 500);
  }
}
