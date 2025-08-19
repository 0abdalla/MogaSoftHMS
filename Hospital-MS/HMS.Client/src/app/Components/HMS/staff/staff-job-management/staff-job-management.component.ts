import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { FilterModel, PagingFilterModel } from '../../../../Models/Generics/PagingFilterModel';
import { StaffService } from '../../../../Services/HMS/staff.service';
import { MessageService } from 'primeng/api';
import Swal from 'sweetalert2';
declare var bootstrap:any;
@Component({
  selector: 'app-staff-job-management',
  templateUrl: './staff-job-management.component.html',
  styleUrl: './staff-job-management.component.css'
})
export class StaffJobManagementComponent {
  TitleList = ['الموارد البشرية', 'بيانات الموظفين', 'الوظائف'];
  jobTitles: any[] = [];
  total!: number;
  pagingFilterModel: PagingFilterModel = {
    searchText: '',
    currentPage: 1,
    pageSize: 16,
    filterList: []
  };
  // 
  filterForm!: FormGroup
  jobTitleForm!: FormGroup
  selectedJobTitle: any;
  // 
  jobDeps!: any;
  isFilter = true;
  constructor(private staffService: StaffService, private fb: FormBuilder, private messageService: MessageService) { }
  ngOnInit(): void {
    this.jobTitleForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      status: ['Active', Validators.required],
      jobDepartmentId: ['', Validators.required],
      description: [''],
    })
    this.getJobTitles();
    this.getDeps();
    this.filterForm = this.fb.group({
      SearchText: ['']
    });
    this.filterForm
      .get('SearchText')
      .valueChanges.pipe(
        debounceTime(300),
        distinctUntilChanged(),
      )
      .subscribe((value: any) => {
        this.pagingFilterModel.currentPage = 1;
        this.pagingFilterModel.searchText = value
        this.getJobTitles();
      });
  }

  getJobTitles() {
    this.staffService.getJobTitles(this.pagingFilterModel.searchText, this.pagingFilterModel.currentPage, this.pagingFilterModel.pageSize, this.pagingFilterModel.filterList).subscribe({
      next: (res) => {
        this.jobTitles = res.results;
        this.total = res.totalCount;
      },
      error: (err) => {
      }
    })
  }
  // 
  applyFilters(filters: FilterModel[]) {
    this.pagingFilterModel.filterList = filters;
    this.pagingFilterModel.currentPage = 1;
    this.getJobTitles();
  }

  openjobTitleDetails(id: number) {
    this.staffService.getJobTitleById(id).subscribe({
      next: (res) => {
        this.selectedJobTitle = res.results;
        this.jobTitleForm.patchValue(this.selectedJobTitle);
      },
      error: (err) => {
      }
    })
  }
  // addjobTitle() {
  //   this.staffService.addJobTitle(this.jobTitleForm.value).subscribe({
  //     next: (data: any) => {
  //       this.getJobTitles();
  //       this.jobTitleForm.reset();
  //       this.messageService.add({ severity: 'success', summary: 'عملية ناجحة', detail: 'تم إضافة الوظيفة بنجاح' });
  //     },
  //     error: (err) => {
  //       this.messageService.add({ severity: 'error', summary: 'حدث خطأ', detail: 'حدث خطأ أثناء إضافة الوظيفة' });
  //     }
  //   })
  // }
  // editjobTitle() {
  //   this.staffService.updateJobTitle(this.selectedJobTitle.id, this.jobTitleForm.value).subscribe({
  //     next: (data: any) => {
  //       this.getJobTitles();
  //       this.jobTitleForm.reset();
  //       this.messageService.add({ severity: 'success', summary: 'عملية ناجحة', detail: 'تم تعديل الوظيفة بنجاح' });
  //     },
  //     error: (err) => {
  //       this.messageService.add({ severity: 'error', summary: 'حدث خطأ', detail: 'حدث خطأ أثناء تعديل الوظيفة' });
  //     }
  //   })
  // }
  isEditMode: boolean = false;
  currentTitleId: number | null = null;
  title!:any;
  addJobTitle() {
          if (this.jobTitleForm.invalid) {
            this.jobTitleForm.markAllAsTouched();
            return;
          }
        
          const formData = this.jobTitleForm.value;
        
          if (this.isEditMode && this.currentTitleId !== null) {
            this.staffService.updateJobTitle(this.currentTitleId, formData).subscribe({
              next: (data:any) => {
                console.log(data);
                this.getJobTitles();
                this.jobTitleForm.reset();
                this.isEditMode = false;
                this.currentTitleId = null;
                this.messageService.add({
                  severity: 'success',
                  summary: 'تم التعديل بنجاح',
                  detail: 'تم تعديل الوظيفة بنجاح',
                });
                setTimeout(() => {
                  const modalElement = document.getElementById('addjobTitleModal');
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
            this.staffService.addJobTitle(formData).subscribe({
              next: (res:any) => {
                console.log(res);
                this.getJobTitles();
                this.jobTitleForm.reset();
                if(res.isSuccess==true){
                  this.messageService.add({
                    severity: 'success',
                    summary: 'تم الإضافة بنجاح',
                    detail: 'تم إضافة الوظيفة بنجاح',
                  });
                }else{
                  this.messageService.add({
                    severity: 'error',
                    summary: 'فشل الإضافة',
                    detail: 'فشل إضافة الوظيفة',
                  });
                }
              },
              error: (err) => {
                console.error('فشل الإضافة:', err);
              }
            });
          }
  }
  editJobTitle(id: number) {
          this.isEditMode = true;
          this.currentTitleId = id;
        
          this.staffService.getJobTitleById(id).subscribe({
            next: (data) => {
              this.title = data.results;
              console.log('title:', this.title);
              // this.jobTitleForm.reset();
              this.jobTitleForm.get('name')?.setValue('');
              this.jobTitleForm.get('jobDepartmentId')?.setValue('');
              this.jobTitleForm.get('description')?.setValue('');
              let statusValue = this.title.status === 'نشط' ? 'Active' : 'Inactive';
              this.jobTitleForm.patchValue({
                name: this.title.name,
                jobDepartmentId: this.title.jobDepartmentId,
                status: statusValue,
                description: this.title.description,
              });
              console.log('Form value after patch:', this.jobTitleForm.value);
              console.log('typeof statusValue:', typeof statusValue);
              console.log('statusValue set to:', statusValue);
              const modal = new bootstrap.Modal(document.getElementById('addjobTitleModal')!);
              modal.show();
            },
            error: (err) => {
              console.error('فشل تحميل بيانات القسم:', err);
            }
          });
  }
  deleteJobTitle(id: number) {
      Swal.fire({
        title: 'هل أنت متأكد؟',
        text: 'هل تريد حذف هذه الوظيفة',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'نعم، حذف',
        cancelButtonText: 'إلغاء'
      }).then((result) => {
        if (result.isConfirmed) {
          this.staffService.deleteJobTitle(id).subscribe({
            next: (res:any) => {
              this.getJobTitles();
              if(res.isSuccess==true){
                this.messageService.add({
                  severity: 'success',
                  summary: 'تم الحذف بنجاح',
                  detail: 'تم حذف الوظيفة بنجاح',
                });
              }else{
                this.messageService.add({
                  severity: 'error',
                  summary: 'فشل الحذف',
                  detail: 'فشل حذف الوظيفة',
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
      // this.jobTitleForm.reset();
      this.jobTitleForm.get('name')?.setValue('');
      this.jobTitleForm.get('jobDepartmentId')?.setValue('');
      this.jobTitleForm.get('description')?.setValue('');
      this.isEditMode = false;
      this.currentTitleId = null;
  }
  onPageChange(event: any) {
    this.pagingFilterModel.currentPage = event.page;
    this.getJobTitles();
  }
  // 
  getDeps() {
    this.staffService.getJobDepartment('', 1, 100).subscribe({
      next: (data: any) => {
        this.jobDeps = data.results;
      }
    })
  }
}
