import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
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
  constructor(private router : Router , private fb : FormBuilder , private authService : AuthService) { } 
  ngOnInit(): void { 
    this.loignForm = this.fb.group({
      email: [''],
      password: ['']
    })
  }
  onLogin(){
    this.authService.login(this.loignForm.value).subscribe({
      next:(data:any) => {
        console.log('Login successful', data);
        this.router.navigate(['/hms']);
      },
      error:(err) => {
        console.error('Login failed', err);
      }
    })
  }
}
