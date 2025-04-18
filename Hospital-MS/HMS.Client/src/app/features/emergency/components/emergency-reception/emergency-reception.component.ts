import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AppointmentService } from '../../../../core/services/appointment.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-emergency-reception',
  templateUrl: './emergency-reception.component.html',
  styleUrl: './emergency-reception.component.css'
})
export class EmergencyReceptionComponent {
  emergencyForm: FormGroup;

  constructor(private fb: FormBuilder , private appointmentService : AppointmentService) {
    this.emergencyForm = this.fb.group({
      patientName: ['', Validators.required],
      patientPhone: [null],
      emergencyLevel: ['', Validators.required],
      companionName: ['', Validators.required],
      appointmentType : ['Emergency'],
      companionNationalId: ['', [Validators.required, Validators.minLength(14)]],
      companionPhone: ['', Validators.required],
    });
  }

  submitForm() {
    if(this.emergencyForm.invalid) {
      Swal.fire({
        title:"حدث خطأ",
        text:"تأكد من صحة البيانات",
        icon: 'error'
      })
      return;
    }
    this.appointmentService.createAppointment(this.emergencyForm.value).subscribe({
      next:(data)=>{
        console.log("Appointment Successfully Added" , data);
        this.emergencyForm.reset();
      },
      error : (err) =>{
        console.log("Failed to add appointment" , err);
      }
    })
  }
}
