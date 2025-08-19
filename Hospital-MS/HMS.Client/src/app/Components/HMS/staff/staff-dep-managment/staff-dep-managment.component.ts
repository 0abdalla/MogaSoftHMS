import { Component, OnInit } from '@angular/core';
import { StaffService } from '../../../../Services/HMS/staff.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FilterModel, PagingFilterModel } from '../../../../Models/Generics/PagingFilterModel';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { MessageService } from 'primeng/api';
import Swal from 'sweetalert2';
declare var bootstrap:any;

@Component({
  selector: 'app-staff-dep-managment',
  templateUrl: './staff-dep-managment.component.html',
  styleUrl: './staff-dep-managment.component.css'
})
export class StaffDepManagmentComponent implements OnInit {
  TitleList = ['الموارد البشرية', 'بيانات الموظفين', 'الأقسام'];
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
  constructor(private staffService: StaffService, private fb: FormBuilder, private messageService: MessageService) {
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
    this.staffService.getJobDepartment(this.pagingFilterModel.searchText, this.pagingFilterModel.currentPage, this.pagingFilterModel.pageSize, this.pagingFilterModel.filterList).subscribe({
      next: (data: any) => {
        this.jobDeps = data.results;
        this.total = data.totalCount;
      }, error: (err) => {
      }
    })
  }
  opendepDetails(id: number) {
    this.staffService.getJobDepartmentById(id).subscribe({
      next: (data: any) => {
        this.jobDepartment = data.results;
        this.jobDepsForm.patchValue(this.jobDepartment)
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
  isEditMode: boolean = false;
  currentDepId: number | null = null;
  addDep() {
        if (this.jobDepsForm.invalid) {
          this.jobDepsForm.markAllAsTouched();
          return;
        }
      
        const formData = this.jobDepsForm.value;
      
        if (this.isEditMode && this.currentDepId !== null) {
          this.staffService.updateJobDeprtment(this.currentDepId, formData).subscribe({
            next: (data:any) => {
              console.log(data);
              this.getDeps();
              // this.jobDepsForm.reset();
              this.jobDepsForm.get('name')?.setValue('');
              this.jobDepsForm.get('description')?.setValue('');
              this.isEditMode = false;
              this.currentDepId = null;
              this.messageService.add({
                severity: 'success',
                summary: 'تم التعديل بنجاح',
                detail: 'تم تعديل القسم بنجاح',
              });
              setTimeout(() => {
                const modalElement = document.getElementById('adddepModal');
                if (modalElement) {
                  const modalInstance = bootstrap.Modal.getInstance(modalElement);
                  modalInstance?.hide();
                }
                this.resetFormOnClose();
              }, 1000);
            },
            error: (err) => {
              console.error('فشل التعديل:', err);
            }
          });
        } else {
          this.staffService.addJobDepartment(formData).subscribe({
            next: (res:any) => {
              console.log(res);
              this.getDeps();
              // this.jobDepsForm.reset();
              this.jobDepsForm.get('name')?.setValue('');
              this.jobDepsForm.get('description')?.setValue('');
              if(res.isSuccess==true){
                this.messageService.add({
                  severity: 'success',
                  summary: 'تم الإضافة بنجاح',
                  detail: 'تم إضافة القسم بنجاح',
                });
              }else{
                this.messageService.add({
                  severity: 'error',
                  summary: 'فشل الإضافة',
                  detail: 'فشل إضافة القسم',
                });
              }
            },
            error: (err) => {
              console.error('فشل الإضافة:', err);
            }
          });
        }
      }
      dep!:any;
      editDep(id: number) {
        this.isEditMode = true;
        this.currentDepId = id;
      
        this.staffService.getJobDepartmentById(id).subscribe({
          next: (data) => {
            this.dep = data.results;
            console.log('dep:', this.dep);
            // this.jobDepsForm.reset();
            this.jobDepsForm.get('name')?.setValue('');
            this.jobDepsForm.get('description')?.setValue('');
            let statusValue = this.dep.status === 'نشط' ? 'Active' : 'Inactive';
            this.jobDepsForm.patchValue({
              name: this.dep.name,
              status: statusValue,
              description: this.dep.description,
            });
            console.log('Form value after patch:', this.jobDepsForm.value);
            console.log('typeof statusValue:', typeof statusValue);
            console.log('statusValue set to:', statusValue);
            const modal = new bootstrap.Modal(document.getElementById('adddepModal')!);
            modal.show();
          },
          error: (err) => {
            console.error('فشل تحميل بيانات القسم:', err);
          }
        });
      }
  deleteDep(id: number) {
    Swal.fire({
      title: 'هل أنت متأكد؟',
      text: 'هل تريد حذف هذا القسم؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم، حذف',
      cancelButtonText: 'إلغاء'
    }).then((result) => {
      if (result.isConfirmed) {
        this.staffService.deleteJobDepartment(id).subscribe({
          next: (res:any) => {
            this.getDeps();
            if(res.isSuccess==true){
              this.messageService.add({
                severity: 'success',
                summary: 'تم الحذف بنجاح',
                detail: 'تم حذف القسم بنجاح',
              });
            }else{
              this.messageService.add({
                severity: 'error',
                summary: 'فشل الحذف',
                detail: 'فشل حذف القسم',
              });
            }
          },
          error: (err) => {
            console.error('فشل الحذف:', err);
          }
        });
      }
    });
  }
  resetFormOnClose() {
    // this.jobDepsForm.reset();
    this.jobDepsForm.get('name')?.setValue('');
    this.jobDepsForm.get('description')?.setValue('');
    this.isEditMode = false;
    this.currentDepId = null;
  }
}
