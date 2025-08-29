import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { PagingFilterModel, FilterModel } from '../../../../../Models/Generics/PagingFilterModel';
import { AppointmentService } from '../../../../../Services/HMS/appointment.service';
import { MessageService } from 'primeng/api';
import Swal from 'sweetalert2';
export declare var bootstrap : any

@Component({
  selector: 'app-rooms',
  templateUrl: './rooms.component.html',
  styleUrl: './rooms.component.css'
})
export class RoomsComponent {
  TitleList = ['إعدادات النظام', 'الحجوزات' , 'الغرف'];
  isFilter = true;
    pagingFilterModel: PagingFilterModel = {
      searchText: '',
      currentPage: 1,
      pageSize: 16,
      filterList: []
    };
    wards!:any[];
    rooms!:any[];
    total!:any;
    roomForm!:FormGroup;
    constructor(private appointmentService: AppointmentService , private fb : FormBuilder , private messageService : MessageService){
      this.roomForm = this.fb.group({
        wardId : ['' , Validators.required],
        number:['' , Validators.required ],
        type:['' , Validators.required ],
        status:['' , Validators.required ],
      })
    }
    ngOnInit(): void {
      this.getRooms();
      this.getWards();
    }
    getWards(){
      this.appointmentService.getWards().subscribe({
        next: (data) => {
          this.wards = data.results;
          this.total = data.totalCount;
          console.log(this.wards);
        },
        error: (err) => {
          this.wards = [];
        }
      });
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
    openRoomsDetails(id:any){
      
    }
    onPageChange(page: number) {
      this.pagingFilterModel.currentPage = page;
      this.getRooms();
    }
    applyFilters(filters: FilterModel[]) {
      this.pagingFilterModel.filterList = filters;
      this.pagingFilterModel.currentPage = 1;
      this.getRooms();
    }
    // 
    isEditMode: boolean = false;
      currentRoomId: number | null = null;
      
      addRoom() {
        if (this.roomForm.invalid) {
          this.roomForm.markAllAsTouched();
          return;
        }
      
        const formData = this.roomForm.value;
      
        if (this.isEditMode && this.currentRoomId !== null) {
          this.appointmentService.editRoom(this.currentRoomId, formData).subscribe({
            next: () => {
              this.getRooms();
              this.roomForm.reset();
              this.isEditMode = false;
              this.currentRoomId = null;
              this.messageService.add({
                severity: 'success',
                summary: 'تم التعديل بنجاح',
                detail: 'تم تعديل الغرفة بنجاح',
              });
              setTimeout(() => {
                const modalElement = document.getElementById('addRoomModal');
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
          this.appointmentService.addRoom(formData).subscribe({
            next: (res:any) => {
              this.getRooms();
              this.roomForm.reset();
              console.log(res);
              if(res.isSuccess==true){
                this.messageService.add({
                  severity: 'success',
                  summary: 'تم الإضافة بنجاح',
                  detail: 'تم إضافة الغرفة بنجاح',
                });
              }else{
                this.messageService.add({
                  severity: 'error',
                  summary: 'فشل الإضافة',
                  detail: 'فشل إضافة الغرفة',
                });
              }
            },
            error: (err) => {
              console.error('فشل الإضافة:', err);
            }
          });
        }
      }
      room!:any;
      editRoom(id: number) {
        this.isEditMode = true;
        this.currentRoomId = id;
      
        this.appointmentService.getRoomsById(id).subscribe({
          next: (data) => {
            this.room=data.results;
            this.roomForm.patchValue({
              wardId: this.room.wardId,
              number: this.room.number,
              type: this.room.type,
              status: this.room.status,
            });
      
            const modal = new bootstrap.Modal(document.getElementById('addRoomModal')!);
            modal.show();
          },
          error: (err) => {
            console.error('فشل تحميل بيانات الغرفة:', err);
          }
        });
      }
      deleteRoom(id: number) {
        Swal.fire({
          title: 'هل أنت متأكد؟',
          text: 'هل تريد حذف هذه الغرفة؟',
          icon: 'warning',
          showCancelButton: true,
          confirmButtonColor: '#3085d6',
          cancelButtonColor: '#d33',
          confirmButtonText: 'نعم، حذف',
          cancelButtonText: 'إلغاء'
        }).then((result) => {
          if (result.isConfirmed) {
            this.appointmentService.deleteRoom(id).subscribe({
              next: (res:any) => {
                this.getRooms();
                if(res.isSuccess==true){
                  this.messageService.add({
                    severity: 'success',
                    summary: 'تم الحذف بنجاح',
                    detail: 'تم حذف الغرفة بنجاح',
                  });
                }else{
                  this.messageService.add({
                    severity: 'error',
                    summary: 'فشل الحذف',
                    detail: 'فشل حذف الغرفة',
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
      this.roomForm.reset();
      bootstrap.Modal.getInstance(document.getElementById('addRoomModal')!).hide();
    }
}
