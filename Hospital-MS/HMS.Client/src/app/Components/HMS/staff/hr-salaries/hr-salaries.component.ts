import { Component, ElementRef, OnInit, TemplateRef, ViewChild } from '@angular/core';
import * as XLSX from 'xlsx';
import * as ExcelJS from 'exceljs';
import * as FileSaver from 'file-saver';
import { NgbOffcanvas } from '@ng-bootstrap/ng-bootstrap';
import { StaffService } from '../../../../Services/HMS/staff.service';
import { FilterModel, PagingFilterModel } from '../../../../Models/Generics/PagingFilterModel';

@Component({
  selector: 'app-hr-salaries',
  templateUrl: './hr-salaries.component.html',
  styleUrl: './hr-salaries.component.css'
})
export class HrSalariesComponent implements OnInit {
  @ViewChild('fileInput') fileInput: ElementRef;
  @ViewChild('SalarySidePanel', { static: true }) SalarySidePanel!: TemplateRef<any>;
  TitleList = ['الإدارة المالية', 'رواتب الموظفين'];
  data: any[] = [];
  headers: string[] = [];
  isFilter = true;
  total = 0;
  pagingFilterModel: PagingFilterModel = {
    filterList: [],
    currentPage: 1,
    pageSize: 16,
    searchText: ''
  }

  constructor(private offcanvasService: NgbOffcanvas, private staffService: StaffService) {

  }

  ngOnInit(): void {

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

  onPageChange(obj: any) {
    this.pagingFilterModel.currentPage = obj.page;
  }

  filterChecked(filters: FilterModel[]) {
    this.pagingFilterModel.filterList = filters;
    this.pagingFilterModel.currentPage = 1;
  }
}
