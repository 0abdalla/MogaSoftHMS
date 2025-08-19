import { Component, OnInit } from '@angular/core';
import { FilterModel, PagingFilterModel } from '../../../../../Models/Generics/PagingFilterModel';
import { AppointmentService } from '../../../../../Services/HMS/appointment.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
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
  constructor(private appointmentService: AppointmentService , private fb : FormBuilder){
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
  addFloor(){
    this.appointmentService.addWard(this.floorForm.value).subscribe({
      next: (data) => {
        this.getWards();
        this.floorForm.reset();
        this.resetFormOnClose();
      },
      error: (err) => {
        console.log(err);
      }
    })
  }
  resetFormOnClose(){
    this.floorForm.reset();
    bootstrap.Modal.getInstance(document.getElementById('addFloorModal')!).hide();
  }
}
