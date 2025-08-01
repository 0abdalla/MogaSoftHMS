import { Component, ElementRef, OnInit, TemplateRef, ViewChild } from '@angular/core';
import * as XLSX from 'xlsx';
import * as ExcelJS from 'exceljs';
import * as FileSaver from 'file-saver';
import { NgbOffcanvas } from '@ng-bootstrap/ng-bootstrap';
import { StaffService } from '../../../../Services/HMS/staff.service';
import { SharedService } from '../../../../Services/shared.service';
import { FilterModel, PagingFilterModel } from '../../../../Models/Generics/PagingFilterModel';
declare var bootstrap: any;

@Component({
  selector: 'app-hr-salaries',
  templateUrl: './hr-salaries.component.html',
  styleUrl: './hr-salaries.component.css'
})
export class HrSalariesComponent implements OnInit {
  @ViewChild('fileInput') fileInput: ElementRef;
  @ViewChild('SalarySidePanel', { static: true }) SalarySidePanel!: TemplateRef<any>;
  @ViewChild('PrintSalaries', { static: false }) PrintSalaries: ElementRef;
  TitleList = ['الإدارة المالية', 'رواتب الموظفين'];
  data: any[] = [];
  headers: string[] = [];
  SalariesData: any[] = [];
  AllSalariesData: any[] = [];
  isFilter = true;
  monthValue: any;
  total = 0;
  pagingFilterModel: PagingFilterModel = {
    filterList: [],
    currentPage: 1,
    pageSize: 16,
    searchText: ''
  }
  TotalSalaries = {
    basicSalary: 0,
    secondShift: 0,
    overtime: 0,
    total: 0,
    insurance: 0,
    differenceBasicDays: 0,
    absence: 0,
    totalDeductions: 0,
    net: 0,
    taxes: 0,
    penalties: 0,
    loans: 0,
    vacation: 0,
    totalDays: 0,
    due: 0
  };

  constructor(private offcanvasService: NgbOffcanvas, private staffService: StaffService, private sharedService: SharedService) { }

  ngOnInit(): void {
    const today = new Date();
    this.monthValue = today.toISOString().slice(0, 7);
    this.GetAllStaffSalaries();
  }

  GetAllStaffSalaries() {
    this.staffService.GetAllStaffSalaries(this.pagingFilterModel).subscribe(data => {
      this.AllSalariesData = data.results;
      this.total = data.totalCount;
    })
  }

  openNewSidePanel() {
    this.offcanvasService.open(this.SalarySidePanel, { panelClass: 'add-new-panel', position: 'end' });
  }

  triggerFileInput(input: HTMLInputElement): void {
    input.value = '';
    input.click();
  }

  generateTemplateExcel() {
    const workbook = new ExcelJS.Workbook();
    const worksheet = workbook.addWorksheet('رواتب الموظفين');
    worksheet.views = [{ rightToLeft: true }];
    const headers = ['كود الموظف', 'الاسم', 'الاضافي', 'إجمالي الأيام']; const headerRow = worksheet.addRow(headers);
    headerRow.eachCell((cell) => {
      cell.protection = { locked: true };
      cell.font = { bold: true };
      cell.fill = {
        type: 'pattern',
        pattern: 'solid',
        fgColor: { argb: 'FFDDDDDD' }
      };
    });

    for (let i = 2; i <= 1000; i++) {
      for (let j = 1; j <= headers.length; j++) {
        const cell = worksheet.getCell(i, j);
        cell.protection = { locked: false };
      }
    }

    worksheet.protect('', {
      selectLockedCells: true,
      selectUnlockedCells: true,
      formatCells: false,
      formatColumns: true,
      formatRows: false,
      insertColumns: false,
      insertRows: false,
      deleteColumns: false,
      deleteRows: true,
      sort: false,
      autoFilter: false,
      pivotTables: false
    }).then(() => {
      workbook.xlsx.writeBuffer().then((buffer) => {
        const blob = new Blob([buffer], {
          type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
        });
        FileSaver.saveAs(blob, 'رواتب_الموظفين.xlsx');
      });
    });
  }

  onFileUpload(event: any) {
    const file: File = event.target.files[0];
    if (!file) return;

    const reader = new FileReader();
    const extension = file.name.split('.').pop()?.toLowerCase();

    if (extension === 'csv') {
      reader.onload = (e: any) => {
        const csv = e.target.result;
        const lines = csv.split('\n').map(line => line.trim()).filter(line => line);
        const allRows = lines.map(line => line.split(','));

        let headerRowIndex = allRows.findIndex(row =>
          row.some(cell =>
            typeof cell === 'string' &&
            (cell.includes('كود الموظف') ||
              cell.includes('الاسم') ||
              cell.includes('الاضافي') ||
              cell.includes('إجمالي الأيام')
            )
          )
        );
        if (headerRowIndex === -1) headerRowIndex = 0;

        this.headers = allRows[headerRowIndex];

        const seen = new Set<string>();
        this.headers = this.headers.filter((header: string, index: number) => {
          if (!header || seen.has(header)) return false;
          seen.add(header);
          return true;
        });
        this.headers.push('التاريخ');
        this.data = allRows.slice(headerRowIndex + 1).map(row => {
          const obj: any = {};
          this.headers.forEach((header, index) => {
            obj[header] = row[index]?.trim() ?? '';
          });
          return obj;
        });
      };
      reader.readAsText(file);
      this.openNewSidePanel();
    }

    else if (extension === 'xlsx' || extension === 'xls') {
      reader.onload = (e: any) => {
        const binary = new Uint8Array(e.target.result);
        const workbook = XLSX.read(binary, { type: 'array' });

        const sheetName = workbook.SheetNames[0];
        const worksheet = workbook.Sheets[sheetName];
        const rawData: any[][] = XLSX.utils.sheet_to_json(worksheet, {
          header: 1,
          defval: '',
        });

        const rows = rawData.filter(row => row.some(cell => cell !== ''));

        let headerRowIndex = rows.findIndex(row =>
          row.some(cell =>
            typeof cell === 'string' &&
            (cell.includes('كود الموظف') ||
              cell.includes('الاسم') ||
              cell.includes('الاضافي') ||
              cell.includes('إجمالي الأيام')
            )
          )
        );
        if (headerRowIndex === -1) headerRowIndex = 0;

        this.headers = rows[headerRowIndex];

        const seen = new Set<string>();
        this.headers = this.headers.filter((header: string, index: number) => {
          if (!header || seen.has(header)) return false;
          seen.add(header);
          return true;
        });
        this.headers.push('التاريخ');
        this.data = rows.slice(headerRowIndex + 1).map(row => {
          const obj: any = {};
          this.headers.forEach((header: string, index: number) => {
            obj[header] = row[index] ?? '';
          });
          return obj;
        });
      };
      reader.readAsArrayBuffer(file);
      this.openNewSidePanel();
    }

    else {
      alert('صيغة غير مدعومة. الرجاء اختيار ملف بصيغة CSV أو XLSX.');
    }
  }

  SaveStaffSalaryFile() {
    const mappedData = this.data.map(row => this.mapArabicToEnglish(row));
    if (mappedData.length == 0) {
      alert('برجاء إدخال ملف يحتوي على بيانات');
      return;
    }

    if (mappedData.some(item => item.staffId == null || item.staffName == null || item.overtime == null || item.totalDays == null)) {
      alert('برجاء إدخال ملف صالح ,قم بتحميل القالب وملئه ببيانات صالحة');
      return;
    }

    if (mappedData.some(item => item.date == '')) {
      alert('برجاء إدخال التاريخ لكل صف');
      return;
    }

    this.staffService.CalculateStaffSalaries(mappedData).subscribe(data => {
      this.SalariesData = data;
      this.SalariesData.forEach(item => {
        this.TotalSalaries.basicSalary += item.basicSalary;
        this.TotalSalaries.secondShift += item.secondShift;
        this.TotalSalaries.overtime += item.overtime;
        this.TotalSalaries.total += item.total;
        this.TotalSalaries.insurance += item.insurance;
        this.TotalSalaries.differenceBasicDays += item.differenceBasicDays;
        this.TotalSalaries.absence += item.absence;
        this.TotalSalaries.totalDeductions += item.totalDeductions;
        this.TotalSalaries.net += item.net;
        this.TotalSalaries.taxes += item.taxes;
        this.TotalSalaries.penalties += item.penalties;
        this.TotalSalaries.loans += item.loans;
        this.TotalSalaries.vacation += item.vacation;
        this.TotalSalaries.totalDays += item.totalDays;
        this.TotalSalaries.due += item.due;
      });

      this.offcanvasService.dismiss();
    });
  }

  PrintSalariesClick() {
    if (this.SalariesData.length === 0) {
      alert('لا توجد بيانات للطباعة');
      return;
    }

    this.sharedService.generatePdf(this.PrintSalaries.nativeElement, 'رواتب_الموظفين', 'landscape');
  }

  mapArabicToEnglish(row: any): any {
    return {
      id: 0,
      staffId: row["كود الموظف"],
      staffName: row["الاسم"],
      overtime: row["الاضافي"],
      totalDays: row["إجمالي الأيام"],
      date: row["التاريخ"]
    };
  }

  filterChecked(filters: FilterModel[]) {
    this.pagingFilterModel.filterList = filters;
    this.pagingFilterModel.currentPage = 1;
    this.GetAllStaffSalaries();
  }

  onPageChange(obj: any) {
    this.pagingFilterModel.currentPage = obj.page;
    this.GetAllStaffSalaries();
  }

  CloseModal() {
    const modalElement = document.getElementById('CalcSalariesModal');
    const modal = bootstrap.Modal.getInstance(modalElement!);
    if (modal) {
      modal.hide();
      modal.dispose();
    }
    document.querySelectorAll('.modal-backdrop').forEach(b => b.remove());
    document.body.classList.remove('modal-open');
    document.body.style.overflow = '';
    document.body.style.paddingRight = '';
  }

  ApplyCalcSalaries() {
    if (!this.monthValue) {
      alert('برجاء اختيار الشهر لحساب الرواتب');
      return;
    }

    const fullDate = `${this.monthValue}-01`;
    this.staffService.CalculateStaffSalaries(fullDate).subscribe(data => {
      debugger;
      this.SalariesData = data.results;
      if (!this.SalariesData || this.SalariesData?.length == 0)
        alert('تم حفظ المرتبات لهذا الشهر برجاء التحقق من الجدول')
      else
        this.openNewSidePanel();
      this.CloseModal();
    });
  }

  SaveStaffSalaries() {
    this.staffService.AddStaffSalaries(this.SalariesData).subscribe(data => {
      if (data.isSuccess) {
        this.offcanvasService.dismiss();
        this.GetAllStaffSalaries();
      }
    })
  }
}
