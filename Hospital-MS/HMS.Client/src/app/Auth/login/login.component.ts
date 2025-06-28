import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { trigger, transition, style, animate } from '@angular/animations';
import { AuthService } from '../auth.service';
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
  message:string = '';
  constructor(private router : Router , private fb : FormBuilder , private authService : AuthService) { } 
  ngOnInit(): void { 
    this.loignForm = this.fb.group({
      userName: ['' , Validators.required],
      password: ['' , Validators.required]
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

  showPassword = false;

  togglePassword() {
    this.showPassword = !this.showPassword;
  }
}
