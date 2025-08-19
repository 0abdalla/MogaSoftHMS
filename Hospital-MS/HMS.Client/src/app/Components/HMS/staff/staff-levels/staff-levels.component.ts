import { Component, OnInit } from '@angular/core';
import { StaffService } from '../../../../Services/HMS/staff.service';
import { FilterModel, PagingFilterModel } from '../../../../Models/Generics/PagingFilterModel';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import Swal from 'sweetalert2';
declare var bootstrap: any;

@Component({
  selector: 'app-staff-levels',
  templateUrl: './staff-levels.component.html',
  styleUrl: './staff-levels.component.css'
})
export class StaffLevelsComponent implements OnInit {
  TitleList = ['الموارد البشرية', 'بيانات الموظفين', 'المستويات الوظيفية'];
  jobLevels!: any;
  pagingFilterModel: PagingFilterModel = {
    searchText: '',
    currentPage: 1,
    pageSize: 16,
    filterList: []
  };
  total!: number;
  // 
  filterForm!: FormGroup;
  jobLevelForm!: FormGroup;
  // 
  selectedJobLevel!: any;
  isFilter = true;
  constructor(private staffService: StaffService, private fb: FormBuilder, private messageservice: MessageService) { }
  ngOnInit(): void {
    this.getJobLevels();
    this.filterForm = this.fb.group({
      SearchText: [''],
    });
    this.filterForm.get('SearchText')?.valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged()
    ).subscribe((value: string) => {
      this.pagingFilterModel.currentPage = 1;
      this.pagingFilterModel.searchText = value
      this.getJobLevels();
    });
    this.jobLevelForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      status: ['Active', Validators.required],
      description: [''],
    });
  }
  getJobLevels() {
    this.staffService.getJobLevels(this.pagingFilterModel.searchText, this.pagingFilterModel.currentPage, this.pagingFilterModel.pageSize, this.pagingFilterModel.filterList).subscribe((res: any) => {
      this.jobLevels = res.results;

    })
  }
  // 
  applyFilters(filters: FilterModel[]) {
    this.pagingFilterModel.filterList = filters;
    this.pagingFilterModel.currentPage = 1;
    this.getJobLevels();
  }
  resetFilters() {
    this.filterForm.reset();
    this.pagingFilterModel.currentPage = 1;
    this.pagingFilterModel.searchText = '';
    this.getJobLevels();
  }
  // 
  openjobLevelDetails(id: number) {
    this.staffService.getJobLevelById(id).subscribe({
      next: (res) => {
        this.selectedJobLevel = res.results;
        this.jobLevelForm.patchValue(this.selectedJobLevel);
      },
      error: (err) => {
      }
    })
  }
  onPageChange(event: any) {
    this.pagingFilterModel.currentPage = event.page;
    this.getJobLevels();
  }
  // 
  // addjobLevel() {
  //   this.staffService.addJobLevel(this.jobLevelForm.value).subscribe({
  //     next: (res) => {
  //       this.jobLevelForm.reset();
  //       this.getJobLevels();
  //       this.messageservice.add({ severity: 'success', summary: 'عملية ناجحة', detail: 'تم إضافة المستوى بنجاح' });
  //     },
  //     error: (err) => {
  //       this.messageservice.add({ severity: 'error', summary: 'حدث خطأ', detail: 'حدث خطأ أثناء إضافة المستوى' });
  //     }
  //   })
  // }
  // editjobLevel() {
  //   this.staffService.updateJobLevel(this.selectedJobLevel.id, this.jobLevelForm.value).subscribe({
  //     next: (res) => {
  //       this.jobLevelForm.reset();
  //       this.getJobLevels();
  //       this.messageservice.add({ severity: 'success', summary: 'عملية ناجحة', detail: 'تم تعديل المستوى بنجاح' });
  //     },
  //     error: (err) => {
  //       this.messageservice.add({ severity: 'error', summary: 'حدث خطأ', detail: 'حدث خطأ أثناء تعديل المستوى' });
  //     }
  //   })
  // }
  isEditMode: boolean = false;
  currentJobLevelId: number | null = null;
    addJobLevel() {
          if (this.jobLevelForm.invalid) {
            this.jobLevelForm.markAllAsTouched();
            return;
          }
        
          const formData = this.jobLevelForm.value;
        
          if (this.isEditMode && this.currentJobLevelId !== null) {
            this.staffService.updateJobLevel(this.currentJobLevelId, formData).subscribe({
              next: (data:any) => {
                console.log(data);
                this.getJobLevels();
                this.jobLevelForm.reset();
                this.isEditMode = false;
                this.currentJobLevelId = null;
                this.messageservice.add({
                  severity: 'success',
                  summary: 'تم التعديل بنجاح',
                  detail: 'تم تعديل المستوى الوظيفي بنجاح',
                });
                setTimeout(() => {
                  const modalElement = document.getElementById('addjobLevelModal');
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
            this.staffService.addJobLevel(formData).subscribe({
              next: (res:any) => {
                console.log(res);
                this.getJobLevels();
                this.jobLevelForm.reset();
                if(res.isSuccess==true){
                  this.messageservice.add({
                    severity: 'success',
                    summary: 'تم الإضافة بنجاح',
                    detail: 'تم إضافة المستوى الوظيفي بنجاح',
                  });
                }else{
                  this.messageservice.add({
                    severity: 'error',
                    summary: 'فشل الإضافة',
                    detail: 'فشل إضافة المستوى الوظيفي',
                  });
                }
              },
              error: (err) => {
                console.error('فشل الإضافة:', err);
              }
            });
          }
        }
        jobLevel!:any;
        editJobLevel(id: number) {
          this.isEditMode = true;
          this.currentJobLevelId = id;
        
          this.staffService.getJobLevelById(id).subscribe({
            next: (data) => {
              // this.jobLevelForm.reset();
              this.jobLevelForm.get('name')?.setValue('');
              this.jobLevelForm.get('description')?.setValue('');
              this.jobLevel=data.results;
              let statusValue = this.jobLevel.status === 'نشط' ? 'Active' : 'Inactive';
              this.jobLevelForm.patchValue({
                name: this.jobLevel.name,
                status: statusValue,
                description: this.jobLevel.description,
              });
              console.log('Form value after patch:', this.jobLevelForm.value);
              console.log('typeof statusValue:', typeof statusValue);
              console.log('statusValue set to:', statusValue);
              const modal = new bootstrap.Modal(document.getElementById('addjobLevelModal')!);
              modal.show();
            },
            error: (err) => {
              console.error('فشل تحميل بيانات المستوى الوظيفي:', err);
            }
          });
        }
    deleteJobLevel(id: number) {
      Swal.fire({
        title: 'هل أنت متأكد؟',
        text: 'هل تريد حذف هذا المستوى الوظيفي؟',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'نعم، حذف',
        cancelButtonText: 'إلغاء'
      }).then((result) => {
        if (result.isConfirmed) {
          this.staffService.deleteJobLevel(id).subscribe({
            next: (res:any) => {
              this.getJobLevels();
              if(res.isSuccess==true){
                this.messageservice.add({
                  severity: 'success',
                  summary: 'تم الحذف بنجاح',
                  detail: 'تم حذف المستوى الوظيفي بنجاح',
                });
              }else{
                this.messageservice.add({
                  severity: 'error',
                  summary: 'فشل الحذف',
                  detail: 'فشل حذف المستوى الوظيفي',
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
      // this.jobLevelForm.reset();
      this.jobLevelForm.get('name').setValue('');
      this.jobLevelForm.get('description').setValue('');
      this.isEditMode = false;
      this.currentJobLevelId = null;
    }
}
