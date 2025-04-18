import { Component } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { InsuranceCompany } from '../insurance-form/insurance-form.component';
declare var html2pdf: any;

@Component({
  selector: 'app-insurance-list',
  templateUrl: './insurance-list.component.html',
  styleUrl: './insurance-list.component.css'
})
export class InsuranceListComponent {
  companies: InsuranceCompany[] = [
    {
      id: 1,
      name: 'Y للتأمينات',
      code: 'Y123',
      contactNumber: '+966123456789',
      email: 'contact@y-insurance.com',
      address: 'الرياض، شارع العليا',
      status: 'Active',
      contractDetails: {
        description: 'عقد تأمين صحي شامل',
        startDate: new Date('2025-01-01'),
        endDate: new Date('2025-12-31'),
        categories: [
          { name: 'Premium', coveragePercentage: 90 },
          { name: 'Basic', coveragePercentage: 70 }
        ]
      },
    },
    {
      id: 2,
      name: 'Z للتأمين',
      code: 'Z456',
      contactNumber: '+966987654321',
      email: 'info@z-insurance.com',
      address: 'جدة، شارع الملك',
      status: 'Inactive',
      contractDetails: {
        description: 'عقد تأمين محدود',
        startDate: new Date('2024-06-01'),
        endDate: new Date('2024-12-31'),
        categories: [{ name: 'Standard', coveragePercentage: 80 }]
      },
    }
  ];

  filterForm: FormGroup;
  filteredCompanies: InsuranceCompany[] = [...this.companies];
  selectedCompany: InsuranceCompany | null = null;
  showModal = false;

  constructor(private fb: FormBuilder, private router: Router) {
    this.filterForm = this.fb.group({
      name: [''],
      status: ['']
    });
  }

  applyFilters(event: Event) {
    event.preventDefault();
    const filters = this.filterForm.value;
    this.filteredCompanies = this.companies.filter(company => {
      const matchesName = filters.name 
        ? company.name.toLowerCase().includes(filters.name.toLowerCase()) 
        : true;
      const matchesStatus = filters.status 
        ? company.status === filters.status 
        : true;
      return matchesName && matchesStatus;
    });
  }

  resetFilters() {
    this.filterForm.reset({ name: '', status: '' });
    this.filteredCompanies = [...this.companies];
  }

  showCompanyDetails(company: InsuranceCompany) {
    this.selectedCompany = company;
    this.showModal = true;
    console.log('Company Details:', company);
  }


  editCompany(id: number) {
    this.router.navigate(['/edit-insurance', id]);
  }
  print(){
    window.print();
  }

  exportToPDF() {
    const element = document.getElementById('pdfContent');
  
    const opt = {
      margin:       0.5,
      filename:     'insurance-details.pdf',
      image:        { type: 'jpeg', quality: 1 },
      html2canvas:  { scale: 2 },
      jsPDF:        { unit: 'in', format: 'a4', orientation: 'portrait' }
    };
  
    html2pdf().set(opt).from(element).save();
  }
}
