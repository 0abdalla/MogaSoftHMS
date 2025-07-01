import { Component, ElementRef, OnInit, TemplateRef, ViewChild } from '@angular/core';
import * as XLSX from 'xlsx';
import * as ExcelJS from 'exceljs';
import * as FileSaver from 'file-saver';
import { FilterModel, PagingFilterModel } from '../../../../Models/Generics/PagingFilterModel';
import { NgbOffcanvas } from '@ng-bootstrap/ng-bootstrap';
import { StaffService } from '../../../../Services/HMS/staff.service';

@Component({
  selector: 'app-attendance-form',
  templateUrl: './attendance-form.component.html',
  styleUrl: './attendance-form.component.css'
})
export class AttendanceFormComponent implements OnInit {
  @ViewChild('fileInput') fileInput: ElementRef;
  @ViewChild('AttendanceSidePanel', { static: true }) attendanceSidePanel!: TemplateRef<any>;
  TitleList = ['الموارد البشرية', 'الحضور والانصراف'];
  data: any[] = [];
  headers: string[] = [];
  AttendanceList: any[] = [];
  total = 0;
  isFilter = true;
  pagingFilterModel: PagingFilterModel = {
    filterList: [],
    currentPage: 1,
    pageSize: 16,
    searchText: ''
  }

  constructor(private offcanvasService: NgbOffcanvas, private staffService: StaffService) { }

  ngOnInit(): void {
    this.GetAllAttendanceSalaries();
  }

  openNewSidePanel() {
    this.offcanvasService.open(this.attendanceSidePanel, { panelClass: 'add-new-panel', position: 'end' });
  }

  triggerFileInput(input: HTMLInputElement): void {
    input.value = '';
    input.click();
  }

  GetAllAttendanceSalaries() {
    this.staffService.GetAllAttendanceSalaries(this.pagingFilterModel).subscribe(data => {
      this.AttendanceList = data?.results ?? [];
      this.total = data?.totalCount ?? 0;
    });
  }

  filterChecked(filters: FilterModel[]) {
    this.pagingFilterModel.filterList = filters;
    this.pagingFilterModel.currentPage = 1;
    this.GetAllAttendanceSalaries();
  }

  onPageChange(obj: any) {
    this.pagingFilterModel.currentPage = obj.page;
    this.GetAllAttendanceSalaries();
  }

  generateTemplateExcel() {
    const workbook = new ExcelJS.Workbook();
    const worksheet = workbook.addWorksheet('الحضور و الانصراف');
    worksheet.views = [{ rightToLeft: true }];
    const headers = ['رقم البصمة', 'الاسم', 'عدد الساعات', 'ايام العمل', 'الساعات المطلوبة', 'إجمالي ساعات البصمة', 'الايام الفعلية', 'اخرى', 'ايام الجمع', 'إجمالي الأيام', 'الاضافي']; const headerRow = worksheet.addRow(headers);
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
        FileSaver.saveAs(blob, 'الحضور_و_الانصراف.xlsx');
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
            (cell.includes('رقم البصمة') ||
              cell.includes('الاسم') ||
              cell.includes('عدد الساعات') ||
              cell.includes('ايام العمل')
              || cell.includes('الساعات المطلوبة')
              || cell.includes('إجمالي ساعات البصمة')
              || cell.includes('الايام الفعلية')
              || cell.includes('اخرى')
              || cell.includes('ايام الجمع')
              || cell.includes('إجمالي الأيام')
              || cell.includes('الاضافي')
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
            (cell.includes('رقم البصمة') ||
              cell.includes('الاسم') ||
              cell.includes('عدد الساعات') ||
              cell.includes('ايام العمل')
              || cell.includes('الساعات المطلوبة')
              || cell.includes('إجمالي ساعات البصمة')
              || cell.includes('الايام الفعلية')
              || cell.includes('اخرى')
              || cell.includes('ايام الجمع')
              || cell.includes('إجمالي الأيام')
              || cell.includes('الاضافي')
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

  SaveAttendanceFile() {
    const mappedData = this.data.map(row => this.mapArabicToEnglish(row));
    if (mappedData.length == 0) {
      alert('برجاء إدخال ملف يحتوي على بيانات');
      return;
    }

    this.staffService.AddAttendaceSalaries(mappedData).subscribe(data => {
      this.GetAllAttendanceSalaries();
      this.offcanvasService.dismiss();
    });
  }

  mapArabicToEnglish(row: any): any {
    return {
      Id: 0,
      code: row["رقم البصمة"],
      name: row["الاسم"],
      workHours: row["عدد الساعات"],
      workDays: row["ايام العمل"],
      requiredHours: row["الساعات المطلوبة"],
      totalFingerprintHours: row["إجمالي ساعات البصمة"],
      sickDays: row["الايام الفعلية"],
      otherDays: row["اخرى"],
      fridays: row["ايام الجمع"],
      totalDays: row["إجمالي الأيام"],
      overtime: row["الاضافي"]
    };
  }
}
