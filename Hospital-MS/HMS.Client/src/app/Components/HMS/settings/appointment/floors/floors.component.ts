import { Component, OnInit } from '@angular/core';
import { FilterModel, PagingFilterModel } from '../../../../../Models/Generics/PagingFilterModel';
import { AppointmentService } from '../../../../../Services/HMS/appointment.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import Swal from 'sweetalert2';
declare var bootstrap:any;
@Component({
  selector: 'app-floors',
  templateUrl: './floors.component.html',
  styleUrl: './floors.component.css'
})
export class FloorsComponent implements OnInit {
  TitleList = ['إعدادات النظام', 'الحجوزات' , 'الطوابق'];
  isFilter = true;
  pagingFilterModel: PagingFilterModel = {
    searchText: '',
    currentPage: 1,
    pageSize: 16,
    filterList: []
  };
  floors!:any[];
  total!:any;
  floorForm!:FormGroup;
  constructor(private appointmentService: AppointmentService , private fb : FormBuilder , private messageService : MessageService){
    this.floorForm = this.fb.group({
      name:['' , [Validators.required , Validators.minLength(3)]],
    })
  }
  ngOnInit(): void {
    this.getWards();
  }
  getWards(){
    this.appointmentService.getWards().subscribe({
      next: (data) => {
        this.floors = data.results;
        this.total = data.totalCount;
        console.log(this.floors);
      },
      error: (err) => {
        this.floors = [];
      }
    });
  }
  openFloorDetails(id:any){
    
  }
  onPageChange(page: number) {
    this.pagingFilterModel.currentPage = page;
    this.getWards();
  }
  applyFilters(filters: FilterModel[]) {
    this.pagingFilterModel.filterList = filters;
    this.pagingFilterModel.currentPage = 1;
    this.getWards();
  }
  // 
  isEditMode: boolean = false;
  currentWardId: number | null = null;
  addWard() {
    if (this.floorForm.invalid) {
      this.floorForm.markAllAsTouched();
      return;
    }
  
    const formData = this.floorForm.value;
  
    if (this.isEditMode && this.currentWardId !== null) {
      this.appointmentService.editWard(this.currentWardId, formData).subscribe({
        next: (res:any) => {
          this.getWards();
          this.floorForm.reset();
          this.isEditMode = false;
          this.currentWardId = null;
          this.messageService.add({
            severity: 'success',
            summary: 'تم التعديل بنجاح',
            detail: 'تم تعديل الطابق بنجاح',
          });
          setTimeout(() => {
            const modalElement = document.getElementById('addFloorModal');
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
      this.appointmentService.addWard(formData).subscribe({
        next: (res) => {
          console.log(res);
          console.log(this.floorForm.value);
          
          this.getWards();
          // this.floorForm.reset();
          if(res.isSuccess == true){
            this.messageService.add({
              severity: 'success',
              summary: 'تم الإضافة بنجاح',
              detail: 'تم إضافة الطابق بنجاح',
            });
          }else{
            this.messageService.add({
              severity: 'error',
              summary: 'فشل الإضافة',
              detail: 'فشل إضافة الطابق',
            });
          } 
        },
        error: (err) => {
          console.error('فشل الإضافة:', err);
        }
      });
    }
  }
  Ward!:any;
  editWard(id: number) {
    this.isEditMode = true;
    this.currentWardId = id;
  
    this.appointmentService.getWardsById(id).subscribe({
      next: (data) => {
        this.Ward=data.results;
        this.floorForm.patchValue({
          name: this.Ward.name,
        });
  
        const modal = new bootstrap.Modal(document.getElementById('addFloorModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات الطابق:', err);
      }
    });
  }
  deleteFloor(id: number) {
    Swal.fire({
      title: 'هل أنت متأكد؟',
      text: 'هل تريد حذف هذا الطابق',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم، حذف',
      cancelButtonText: 'إلغاء'
    }).then((result) => {
      if (result.isConfirmed) {
        this.appointmentService.deleteWard(id).subscribe({
          next: (res:any) => {
            this.getWards();
            if(res.isSuccess == true){
              this.messageService.add({
                severity: 'success',
                summary: 'تم الحذف بنجاح',
                detail: 'تم حذف الطابق بنجاح',
              });
            }else{
              this.messageService.add({
                severity: 'error',
                summary: 'فشل الحذف',
                detail: 'فشل حذف الطابق',
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
  resetFormOnClose(){
    this.floorForm.reset();
    // bootstrap.Modal.getInstance(document.getElementById('addFloorModal')!).hide();
    this.currentWardId = null;
    this.isEditMode = false;
  }
}
