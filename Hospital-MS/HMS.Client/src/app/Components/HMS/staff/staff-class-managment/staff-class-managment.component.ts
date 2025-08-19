import { Component, OnInit } from '@angular/core';
import { StaffService } from '../../../../Services/HMS/staff.service';
import { FilterModel, PagingFilterModel } from '../../../../Models/Generics/PagingFilterModel';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { debounceTime, distinctUntilChanged, Subject, takeUntil } from 'rxjs';
import Swal from 'sweetalert2';
declare var bootstrap: any;

@Component({
  selector: 'app-staff-class-managment',
  templateUrl: './staff-class-managment.component.html',
  styleUrl: './staff-class-managment.component.css'
})
export class StaffClassManagmentComponent implements OnInit {
  TitleList = ['الموارد البشرية', 'بيانات الموظفين', 'تصنيف الموظفين'];
  jobTypes: any[] = [];
  total!: number;
  pagingFilterModel: PagingFilterModel = {
    searchText: '',
    currentPage: 1,
    pageSize: 16,
    filterList: []
  };
  filterForm!: FormGroup;
  jobTypeForm!: FormGroup;
  selectedJobType: any;
  destroy$: Subject<void> = new Subject<void>();
  isFilter = true;
  constructor(private staffService: StaffService, private fb: FormBuilder, private messageservice: MessageService) {

  }
  ngOnInit(): void {
    this.filterForm = this.fb.group({
      SearchText: ['']
    });
    this.filterForm
      .get('SearchText')
      .valueChanges.pipe(
        debounceTime(300),
        distinctUntilChanged(),
        takeUntil(this.destroy$)
      )
      .subscribe((value: any) => {
        this.pagingFilterModel.currentPage = 1;
        this.pagingFilterModel.searchText = value
        this.getTypes();
      });
    this.jobTypeForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      status: ['Active', Validators.required],
      description: [''],
    })
    this.getTypes();
  }
  getTypes() {
    this.staffService.getJobTypes(this.pagingFilterModel.searchText, this.pagingFilterModel.currentPage, this.pagingFilterModel.pageSize, this.pagingFilterModel.filterList).subscribe({
      next: (res) => {
        this.jobTypes = res.results;
        this.total = res.totalCount;
      },
      error: (err) => {
      }
    })
  }
  applyFilters(filters: FilterModel[]) {
    this.pagingFilterModel.filterList = filters;
    this.pagingFilterModel.currentPage = 1;
    this.getTypes();
  }

  openjobTypeDetails(id: number) {
    this.staffService.getJobTypeById(id).subscribe({
      next: (res) => {
        this.selectedJobType = res.results;
        this.jobTypeForm.patchValue(this.selectedJobType);
      },
      error: (err) => {
      }
    })
  }
  onPageChange(event: any) {
    this.pagingFilterModel.currentPage = event.page;
    this.getTypes();
  }
  // 
  // addJobType() {
  //   this.staffService.addJobType(this.jobTypeForm.value).subscribe({
  //     next: (res) => {
  //       this.jobTypeForm.reset();
  //       this.getTypes();
  //       this.messageservice.add({ severity: 'success', summary: 'عملية ناجحة', detail: 'تم إضافة الفئة بنجاح' });
  //     },
  //     error: (err) => {
  //       this.messageservice.add({ severity: 'error', summary: 'حدث خطأ', detail: 'حدث خطأ أثناء إضافة الفئة' });
  //     }
  //   })
  // }
  // editJobType() {
  //   this.staffService.updateJobType(this.selectedJobType.id, this.jobTypeForm.value).subscribe({
  //     next: (res) => {
  //       this.jobTypeForm.patchValue(res.results);
  //       this.getTypes();
  //       this.messageservice.add({ severity: 'success', summary: 'عملية ناجحة', detail: 'تم تعديل الفئة بنجاح' });
  //     },
  //     error: (err) => {
  //       this.messageservice.add({ severity: 'error', summary: 'حدث خطأ', detail: 'حدث خطأ أثناء تعديل الفئة' });
  //     }
  //   })
  // }
  isEditMode: boolean = false;
    currentJobTypeId: number | null = null;
      addJobType() {
            if (this.jobTypeForm.invalid) {
              this.jobTypeForm.markAllAsTouched();
              return;
            }
          
            const formData = this.jobTypeForm.value;
          
            if (this.isEditMode && this.currentJobTypeId !== null) {
              this.staffService.updateJobType(this.currentJobTypeId, formData).subscribe({
                next: (data:any) => {
                  console.log(data);
                  this.getTypes();
                  // this.jobTypeForm.reset();
                  this.jobTypeForm.get('name')?.setValue('');
                  this.jobTypeForm.get('description')?.setValue('');
                  this.isEditMode = false;
                  this.currentJobTypeId = null;
                  this.messageservice.add({
                    severity: 'success',
                    summary: 'تم التعديل بنجاح',
                    detail: 'تم تعديل التصنيف بنجاح',
                  });
                  setTimeout(() => {
                    const modalElement = document.getElementById('addjobTypeModal');
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
              this.staffService.addJobType(formData).subscribe({
                next: (res:any) => {
                  console.log(res);
                  this.getTypes();
                  // this.jobTypeForm.reset();
                  this.jobTypeForm.get('name')?.setValue('');
                  this.jobTypeForm.get('description')?.setValue('');
                  if(res.isSuccess==true){
                    this.messageservice.add({
                      severity: 'success',
                      summary: 'تم الإضافة بنجاح',
                      detail: 'تم إضافة التصنيف بنجاح',
                    });
                  }else{
                    this.messageservice.add({
                      severity: 'error',
                      summary: 'فشل الإضافة',
                      detail: 'فشل إضافة التصنيف',
                    });
                  }
                },
                error: (err) => {
                  console.error('فشل الإضافة:', err);
                }
              });
            }
          }
          jobType!:any;
          editJobType(id: number) {
            this.isEditMode = true;
            this.currentJobTypeId = id;
          
            this.staffService.getJobTypeById(id).subscribe({
              next: (data) => {
                // this.jobTypeForm.reset();
                this.jobTypeForm.get('name')?.setValue('');
                this.jobTypeForm.get('description')?.setValue('');
                this.jobType=data.results;
                console.log(this.jobType);
                let statusValue = this.jobType.status === 'نشط' ? 'Active' : 'Inactive';
                this.jobTypeForm.patchValue({
                  name: this.jobType.name,
                  status: statusValue,
                  description: this.jobType.description,
                });
                const modal = new bootstrap.Modal(document.getElementById('addjobTypeModal')!);
                modal.show();
              },
              error: (err) => {
                console.error('فشل تحميل بيانات التصنيف:', err);
              }
            });
          }
      deleteJobType(id: number) {
        Swal.fire({
          title: 'هل أنت متأكد؟',
          text: 'هل تريد حذف هذا التصنيف؟',
          icon: 'warning',
          showCancelButton: true,
          confirmButtonColor: '#3085d6',
          cancelButtonColor: '#d33',
          confirmButtonText: 'نعم، حذف',
          cancelButtonText: 'إلغاء'
        }).then((result) => {
          if (result.isConfirmed) {
            this.staffService.deleteJobType(id).subscribe({
              next: (res:any) => {
                this.getTypes();
                if(res.isSuccess==true){
                  this.messageservice.add({
                    severity: 'success',
                    summary: 'تم الحذف بنجاح',
                    detail: 'تم حذف التصنيف بنجاح',
                  });
                }else{
                  this.messageservice.add({
                    severity: 'error',
                    summary: 'فشل الحذف',
                    detail: 'فشل حذف التصنيف',
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
        // this.jobTypeForm.reset();
        this.jobTypeForm.get('name')?.setValue('');
        this.jobTypeForm.get('description')?.setValue('');
        this.isEditMode = false;
        this.currentJobTypeId = null;
      }
}
