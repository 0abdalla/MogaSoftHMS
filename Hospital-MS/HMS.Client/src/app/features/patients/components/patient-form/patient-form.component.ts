import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { StaffService } from '../../../../core/services/staff.service';
import { AdmissionService } from '../../../../core/services/admission.service';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-patient-form',
  templateUrl: './patient-form.component.html',
  styleUrls: ['./patient-form.component.css']
})
export class PatientFormComponent implements OnInit {
  patientForm: FormGroup;
  // 
  doctors!:any[];
  filteredDoctors!: any[];
  departments!:any;
  rooms!:any;
  filteredRooms!:any[];
  beds!:any;
  filteratedBeds!:any[]
  // 
  showSecondContact = false;
  // 
  currentDate!:any
  constructor(
    private fb: FormBuilder,
    private staffService: StaffService,
    private addmisionService: AdmissionService
  ) {
    this.patientForm = this.fb.group({
      patientName: ['' , Validators.required],
      patientPhone: ['' , Validators.required],
      patientBirthDate: ['' , Validators.required],
      patientNationalId: ['' , Validators.required],
      patientAddress: ['' , Validators.required],
      patientStatus: ['' , Validators.required],
      emergencyPhone01: ['' , Validators.required],
      emergencyContact01: ['' , Validators.required],
      emergencyPhone02: [''],
      emergencyContact02: [''],
      departmentId: ['' , Validators.required],
      doctorId: [{value: '', disabled: true}, Validators.required],
      roomType : ['' , Validators.required],
      roomId: [{value: '', disabled: true} , Validators.required],
      bedId: [{value: '', disabled: true} , Validators.required],
      insuranceCompanyId: [null],
      insuranceCategoryId: [null],
      insuranceNumber: [''],
      healthStatus: [''],
      initialDiagnosis: [''],
      hasCompanion: [false],
      companionName: [''],
      companionNationalId: [''],
      companionPhone: [''],
      notes: ['']
    });
  }
  addSecondContact() {
    this.showSecondContact = true;
  }
  ngOnInit(): void {
    this.loadAdmissionData();
    this.currentDate = new Date().toISOString().split('T')[0];
  }
  loadAdmissionData() {
    forkJoin({
      doctors: this.staffService.getStaff(),
      departments: this.addmisionService.getDepartments(),
      rooms: this.addmisionService.getRooms(),
      beds: this.addmisionService.getBeds()
    }).subscribe({
      next: (res) => {
        this.doctors = res.doctors;
        this.departments = res.departments;
        this.rooms = res.rooms;
        this.beds = res.beds;
        console.log('Doctors:', this.doctors);
        console.log('Departments:', this.departments);
        console.log('Rooms:', this.rooms);
        console.log('Beds:', this.beds);
      },
      error: (err) => {
        console.error('Error loading admission data', err);
      }
    });
  }
  onSubmit() {
    if (this.patientForm.valid) {
      this.addmisionService.addAdmision(this.patientForm.value).subscribe({
        next: () => {
          console.log("Done");
          this.patientForm.reset();
          this.showSecondContact = false;
        },
        error: (error) => {
          console.error(error);
        }
      });
    }else{
      console.log("error");
    }
  }
  onDepartmentChange() {
    const selectedDeptId = this.patientForm.get('departmentId')?.value;
    if (selectedDeptId) {
      this.filteredDoctors = this.doctors.filter(doctor => doctor.departmentId === +selectedDeptId);
      this.patientForm.get('doctorId')?.enable();
    } else {
      console.log("error");
      this.filteredDoctors = [];
      this.patientForm.get('doctorId')?.disable();
    }
    this.patientForm.get('doctorId')?.setValue('');
  }
  onRoomTypeChange(){
    const selectedRoomType = this.patientForm.get('roomType')?.value;
    if (selectedRoomType) {
      this.filteredRooms = this.rooms.filter((room:any) => room.type === selectedRoomType);
      this.patientForm.get('roomId')?.enable();
    } else {
      console.log("error");
      this.filteredRooms = [];
      this.patientForm.get('roomId')?.disable();
    }
    this.patientForm.get('roomId')?.setValue('');
  }
  onRoomChange(){
    const selectedRoomId = this.patientForm.get('roomId')?.value;
    if (selectedRoomId) {
      this.filteratedBeds = this.beds.filter((bed:any) => bed.roomId === +selectedRoomId);
      this.patientForm.get('bedId')?.enable();
    } else {
      console.log("error");
      this.filteratedBeds = [];
      this.patientForm.get('bedId')?.disable();
    }
    this.patientForm.get('bedId')?.setValue('');
  }
}
