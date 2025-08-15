import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { PagingFilterModel, FilterModel } from '../../../../Models/Generics/PagingFilterModel';
import { AdmissionService } from '../../../../Services/HMS/admission.service';

@Component({
  selector: 'app-doctors-departments-list',
  templateUrl: './doctors-departments-list.component.html',
  styleUrl: './doctors-departments-list.component.css'
})
export class DoctorsDepartmentsListComponent {
  TitleList = ['إدارة الأطباء', 'الأقسام الطبية'];
    filterForm!: FormGroup;
    jobDepsForm!: FormGroup
    // 
    jobDeps!: any;
    jobDepartment!: any;
    // 
    pagingFilterModel: PagingFilterModel = {
      searchText: '',
      currentPage: 1,
      pageSize: 16,
      filterList: []
    };
    total!: number;
    isFilter = true;
    constructor(private admissionService: AdmissionService, private fb: FormBuilder, private messageService: MessageService) {
      this.filterForm = this.fb.group({
        SearchText: ['']
      });
      this.jobDepsForm = this.fb.group({
        name: ['', [Validators.required, Validators.minLength(3)]],
        status: ['Active', Validators.required],
        description: [''],
      })
      this.filterForm.get('SearchText')?.valueChanges.pipe(
        debounceTime(300),
        distinctUntilChanged()
      ).subscribe((value: string) => {
        this.pagingFilterModel.currentPage = 1;
        this.pagingFilterModel.searchText = value
        this.getDeps();
      });
    }
    ngOnInit(): void {
      this.getDeps();
    }
    getDeps() {
      this.admissionService.getDepartments().subscribe({
        next: (data: any) => {
          this.jobDeps = data.results;
          this.total = data.totalCount;
        }, error: (err) => {
        }
      })
    }
    applyFilters(filter: FilterModel[]) {
      debugger;
      this.pagingFilterModel.currentPage = 1;
      this.pagingFilterModel.filterList = filter;
      this.getDeps();
    }
  
    onPageChange(obj: any) {
      this.pagingFilterModel.currentPage = obj.page;
      this.getDeps();
    }
    // 
    addDep() {
      this.admissionService.addDepartment(this.jobDepsForm.value).subscribe({
        next: (data: any) => {
          this.getDeps();
          this.jobDepsForm.reset();
          if(data.success){
            this.messageService.add({ severity: 'success', summary: 'عملية ناجحة', detail: 'تم إضافة القسم بنجاح' });
          }else{
            this.messageService.add({ severity: 'error', summary: 'حدث خطأ', detail: 'حدث خطأ أثناء إضافة القسم' });
          }
        }, error: (err) => {
          this.messageService.add({ severity: 'error', summary: 'حدث خطأ', detail: 'حدث خطأ أثناء إضافة القسم' });
        }
      })
    }
    // editDep() {
    //   this.admissionService.updateDepartment(this.jobDepartment.id, this.jobDepsForm.value).subscribe({
    //     next: (data: any) => {
    //       this.getDeps();
    //       this.jobDepsForm.reset();
    //       this.messageService.add({ severity: 'success', summary: 'عملية ناجحة', detail: 'تم تعديل القسم بنجاح' });
  
    //     }, error: (err) => {
    //       this.messageService.add({ severity: 'error', summary: 'حدث خطأ', detail: 'حدث خطأ أثناء تعديل القسم' });
  
    //     }
    //   })
    // }
}
