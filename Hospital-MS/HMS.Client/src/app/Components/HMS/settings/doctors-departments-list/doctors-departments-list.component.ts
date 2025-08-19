import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { PagingFilterModel, FilterModel } from '../../../../Models/Generics/PagingFilterModel';
import { AdmissionService } from '../../../../Services/HMS/admission.service';
import Swal from 'sweetalert2';
declare var bootstrap:any;

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
    isEditMode: boolean = false;
      currentDepId: number | null = null;
      
      addDep() {
        if (this.jobDepsForm.invalid) {
          this.jobDepsForm.markAllAsTouched();
          return;
        }
      
        const formData = this.jobDepsForm.value;
      
        if (this.isEditMode && this.currentDepId !== null) {
          this.admissionService.updateDepartment(this.currentDepId, formData).subscribe({
            next: () => {
              this.getDeps();
              this.jobDepsForm.reset();
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
          this.admissionService.addDepartment(formData).subscribe({
            next: (res:any) => {
              this.getDeps();
              this.jobDepsForm.reset();
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
      
        this.admissionService.getDepartmentsById(id).subscribe({
          next: (data) => {
            this.dep=data.results;
            this.jobDepsForm.patchValue({
              name: this.dep.name,
            });
      
            const modal = new bootstrap.Modal(document.getElementById('adddepModal')!);
            modal.show();
          },
          error: (err) => {
            console.error('فشل تحميل بيانات القسم:', err);
          }
        });
      }
    // addDep() {
    //   this.admissionService.addDepartment(this.jobDepsForm.value).subscribe({
    //     next: (data: any) => {
    //       this.getDeps();
    //       this.jobDepsForm.reset();
    //       if(data.isSuccess == true){
    //         this.messageService.add({ severity: 'success', summary: 'عملية ناجحة', detail: 'تم إضافة القسم بنجاح' });
    //       }else{
    //         this.messageService.add({ severity: 'error', summary: 'حدث خطأ', detail: 'حدث خطأ أثناء إضافة القسم' });
    //       }
    //     }, error: (err) => {
    //       this.messageService.add({ severity: 'error', summary: 'حدث خطأ', detail: 'حدث خطأ أثناء إضافة القسم' });
    //     }
    //   })
    // }
    // currentDepId!:any;
    // editDep(id: number) {
    //     this.currentDepId = id;
      
    //     this.admissionService.getDepartmentsById(id).subscribe({
    //       next: (data) => {
    //         this.jobDepartment=data.results;
    //         this.jobDepsForm.patchValue({
    //           name: this.jobDepartment.name,
    //         });
      
    //         const modal = new bootstrap.Modal(document.getElementById('editdepModal')!);
    //         modal.show();
    //       },
    //       error: (err) => {
    //         console.error('فشل تحميل بيانات القسم:', err);
    //       }
    //     });
    // }
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
              this.admissionService.deleteDepartment(id).subscribe({
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
    resetFormOnClose() {
      this.jobDepsForm.reset();
    }
}
