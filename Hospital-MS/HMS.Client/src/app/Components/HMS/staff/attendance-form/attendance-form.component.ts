import { Component } from '@angular/core';
import * as XLSX from 'xlsx';

@Component({
  selector: 'app-attendance-form',
  templateUrl: './attendance-form.component.html',
  styleUrl: './attendance-form.component.css'
})
export class AttendanceFormComponent {
  data: any[] = [];
  headers: string[] = [];

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
            (cell.includes('الاسم') || cell.includes('عدد') || cell.includes('شفت') || cell.includes('الراتب'))
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
    }

    else if (extension === 'xlsx') {
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
            (cell.includes('الاسم') || cell.includes('عدد') || cell.includes('شفت') || cell.includes('الراتب'))
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
    }

    else {
      alert('صيغة غير مدعومة. الرجاء اختيار ملف بصيغة CSV أو XLSX.');
    }
  }
}
