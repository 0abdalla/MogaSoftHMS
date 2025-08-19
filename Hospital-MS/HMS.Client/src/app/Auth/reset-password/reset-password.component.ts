import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.css'
})
export class ResetPasswordComponent {
  resetForm: FormGroup;
  message: string = '';
  passwordMismatch = false;

  private userName: string = '';
  private code: string = '';

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.userName = params['user-name'] || '';
      this.code = params['code'] || '';
    });

    this.resetForm = this.fb.group({
      newPassword: ['', Validators.required],
      confirmPassword: ['', Validators.required],
    });
  }

  onResetPassword() {
    if (this.resetForm.invalid) {
      this.resetForm.markAllAsTouched();
      return;
    }

    const newPassword = this.resetForm.value.newPassword;
    const confirmPassword = this.resetForm.value.confirmPassword;

    if (newPassword !== confirmPassword) {
      this.passwordMismatch = true;
      return;
    } else {
      this.passwordMismatch = false;
    }

    const payload = {
      userName: this.userName,
      code: this.code,
      newPassword: newPassword,
    };

    this.authService.resetPassword(payload).subscribe({
      next: (res) => {
        this.message = 'تم تغيير كلمة المرور بنجاح';
        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 2000);
      },
      error: (err) => {
        this.message = 'حدث خطأ، حاول مرة أخرى';
      },
    });
  }
}
