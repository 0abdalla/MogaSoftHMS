import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { trigger, transition, style, animate } from '@angular/animations';
import { AuthService } from '../auth.service';
import Swal from 'sweetalert2';
declare var bootstrap:any;
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
  animations: [
    trigger('fadeIn', [
      transition(':enter', [
        style({ opacity: 0 }),
        animate('200ms ease-in', style({ opacity: 1 })),
      ]),
      transition(':leave', [
        animate('200ms ease-out', style({ opacity: 0 })),
      ])
    ])
  ],
})
export class LoginComponent implements OnInit {
  loignForm!: FormGroup;
  forgetPassword!:FormGroup;
  message:string = '';
  forgetMessage:string = '';
  constructor(private router : Router , private fb : FormBuilder , private authService : AuthService) { } 
  ngOnInit(): void { 
    this.loignForm = this.fb.group({
      userName: ['' , Validators.required],
      password: ['' , Validators.required]
    })
    this.forgetPassword = this.fb.group({
      userName: ['' , Validators.required],
    })
  }
  onLogin(){
    this.authService.login(this.loignForm.value).subscribe({
      next:(data:any) => {
        this.router.navigate(['/hms']);
      },
      error:(err) => {
        this.message = 'اسم المستخدم أو كلمة المرور غير صحيحين';
      }
    })
  }
  onForgetPassword() {
    if (this.forgetPassword.invalid) {
      this.forgetPassword.markAllAsTouched();
      return;
    }
  
    this.authService.forgetPassword(this.forgetPassword.value).subscribe({
      next: (data: any) => {
        // this.forgetMessage = 'تم إرسال رابط إعادة التعيين إلى البريد الإلكتروني المسجل';
        Swal.fire({
          title: 'تم إرسال رابط إعادة التعيين إلى البريد الإلكتروني المسجل',
          icon: 'success',
          showConfirmButton: false,
          timer: 2000
        })
        setTimeout(() => {
          const modalElement = document.getElementById('forgetPasswordModal');
          if (modalElement) {
            const modalInstance = bootstrap.Modal.getInstance(modalElement);
            modalInstance?.hide();
          }
        }, 1000);
      },
      error: (err) => {
        Swal.fire({
          title: 'اسم المستخدم غير صحيح',
          icon: 'error',
          showConfirmButton: false,
          timer: 2000
        })
      }
    });
  }
  
  showPassword = false;

  togglePassword() {
    this.showPassword = !this.showPassword;
  }
}
