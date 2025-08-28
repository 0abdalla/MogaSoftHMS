import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import Swal from 'sweetalert2';
import { PagingFilterModel, FilterModel } from '../../../../../Models/Generics/PagingFilterModel';
import { AppointmentService } from '../../../../../Services/HMS/appointment.service';
declare var bootstrap : any;

@Component({
  selector: 'app-beds',
  templateUrl: './beds.component.html',
  styleUrl: './beds.component.css'
})
export class BedsComponent {
  TitleList = ['إعدادات النظام', 'الحجوزات' , 'الأسرّة'];
  isFilter = true;
    pagingFilterModel: PagingFilterModel = {
      searchText: '',
      currentPage: 1,
      pageSize: 16,
      filterList: []
    };
    rooms!:any[];
    beds!:any[];
    total!:any;
    bedForm!:FormGroup;
    constructor(private appointmentService: AppointmentService , private fb : FormBuilder , private messageService : MessageService){
      this.bedForm = this.fb.group({
        roomId : ['' , Validators.required],
        number:['' , Validators.required ],
        status:['' , Validators.required ],
        dailyPrice:['' , Validators.required]
      })
    }
    ngOnInit(): void {
      this.getRooms();
      this.getBeds();
    }
    getRooms(){
      this.appointmentService.getRooms().subscribe({
        next: (data) => {
          this.rooms = data.results;
          this.total = data.totalCount;
          console.log(this.rooms);
        },
        error: (err) => {
          this.rooms = [];
        }
      });
    }
    getBeds(){
      this.appointmentService.getBeds().subscribe({
        next: (data) => {
          this.beds = data.results;
          this.total = data.totalCount;
          console.log(this.beds);
        },
        error: (err) => {
          this.beds = [];
        }
      });
    }
    openBedDetails(id:any){
      
    }
    onPageChange(page: number) {
      this.pagingFilterModel.currentPage = page;
      this.getBeds();
    }
    applyFilters(filters: FilterModel[]) {
      this.pagingFilterModel.filterList = filters;
      this.pagingFilterModel.currentPage = 1;
      this.getBeds();
    }
    // 
    isEditMode: boolean = false;
    currentBedId: number | null = null;
    addBed() {
        if (this.bedForm.invalid) {
          this.bedForm.markAllAsTouched();
          return;
        }
      
        const formData = this.bedForm.value;
      
        if (this.isEditMode && this.currentBedId !== null) {
          this.appointmentService.editBed(this.currentBedId, formData).subscribe({
            next: (res:any) => {
              this.getBeds();
              this.bedForm.reset();
              console.log(res);
              this.isEditMode = false;
              this.currentBedId = null;
              this.messageService.add({
                severity: 'success',
                summary: 'تم التعديل بنجاح',
                detail: 'تم تعديل السرير بنجاح',
              });
              setTimeout(() => {
                const modalElement = document.getElementById('addBedModal');
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
          this.appointmentService.addBed(formData).subscribe({
            next: (res:any) => {
              this.getRooms();
              this.bedForm.reset();
              console.log(res);

              if(res.isSuccess==true){
                this.messageService.add({
                  severity: 'success',
                  summary: 'تم الإضافة بنجاح',
                  detail: 'تم إضافة السرير بنجاح',
                });
              }else{
                this.messageService.add({
                  severity: 'error',
                  summary: 'فشل الإضافة',
                  detail: 'فشل إضافة السرير',
                });
              }
            },
            error: (err) => {
              console.error('فشل الإضافة:', err);
            }
          });
        }
      }
      bed!:any;
      editBed(id: number) {
        this.isEditMode = true;
        this.currentBedId = id;
      
        this.appointmentService.getBedsById(id).subscribe({
          next: (data) => {
            this.bed=data.results;
            this.bedForm.patchValue({
              roomId: this.bed.roomId,
              number: this.bed.number,
              status: this.bed.status,
              dailyPrice: this.bed.dailyPrice,
            });
      
            const modal = new bootstrap.Modal(document.getElementById('addBedModal')!);
            modal.show();
          },
          error: (err) => {
            console.error('فشل تحميل بيانات السرير:', err);
          }
        });
      }
      deleteBed(id: number) {
        Swal.fire({
          title: 'هل أنت متأكد؟',
          text: 'هل تريد حذف هذا السرير؟',
          icon: 'warning',
          showCancelButton: true,
          confirmButtonColor: '#3085d6',
          cancelButtonColor: '#d33',
          confirmButtonText: 'نعم، حذف',
          cancelButtonText: 'إلغاء'
        }).then((result) => {
          if (result.isConfirmed) {
            this.appointmentService.deleteBed(id).subscribe({
              next: (res:any) => {
                this.getRooms();
                if(res.isSuccess==true){
                  this.messageService.add({
                    severity: 'success',
                    summary: 'تم الحذف بنجاح',
                    detail: 'تم حذف السرير بنجاح',
                  });
                }else{
                  this.messageService.add({
                    severity: 'error',
                    summary: 'فشل الحذف',
                    detail: 'فشل حذف السرير',
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
      this.bedForm.reset();
      bootstrap.Modal.getInstance(document.getElementById('addBedModal')!).hide();
    }
}
