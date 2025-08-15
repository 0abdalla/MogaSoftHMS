import { Component, OnInit } from '@angular/core';
import { trigger, transition, style, animate } from '@angular/animations';
import { PagingFilterModel } from '../../../../Models/Generics/PagingFilterModel';
import { AuthService } from '../../../../Auth/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SharedService } from '../../../../Services/shared.service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-permissions',
  templateUrl: './permissions.component.html',
  styleUrl: './permissions.component.css',
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
export class PermissionsComponent implements OnInit {
  TitleList = ['الإعدادات العامة', 'صلاحيات المستخدم'];
  UserForm!: FormGroup;
  pagingFilterModel: PagingFilterModel = {
    searchText: '',
    currentPage: 1,
    pageSize: 16,
    filterList: []
  };
  branchPagingFilterModel: PagingFilterModel = {
    searchText: '',
    currentPage: 1,
    pageSize: 50,
    filterList: []
  };
  UsersData: any[] = [];
  BranchList: any[] = [];
  RolesList: any[] = [];
  Total = 0;
  showPassword = false;
  isEdit = false;

  constructor(private authService: AuthService, private fb: FormBuilder, private sharedService: SharedService, private messageService: MessageService) {
    this.UserForm = this.fb.group({
      branchId: ['', Validators.required],
      roleName: ['', Validators.required],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.pattern('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$')]],
      password: ['', []],
      address: [''],
      userId: ['']
    });
  }

  ngOnInit(): void {
    this.GetAllUsers();
    this.GetBranches();
    this.GetAllRoles();
  }

  openAddEditUserModal(user: any) {
    this.UserForm.reset();
    this.UserForm.patchValue({ branchId: '', roleName: '' });
    if (user) {
      this.isEdit = true;
      this.UserForm.patchValue({
        branchId: user?.branchId,
        userId: user?.userId,
        roleName: user?.roleNAmeEn,
        firstName: user?.firstName,
        lastName: user?.lastName,
        email: user?.email,
        address: user?.address
      });
      this.setPasswordValidators(true);
    } else {
      this.isEdit = false;
      this.setPasswordValidators(false);
    }
  }

  setPasswordValidators(isEditMode: boolean) {
    const passwordControl = this.UserForm.get('password');

    if (!passwordControl) return;

    if (!isEditMode) {
      passwordControl.setValidators([
        Validators.required,
        Validators.minLength(6),
        Validators.pattern(
          '^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^&*()_+\\-=\\[\\]{};:"\\\\|,.<>\\/?]).{8,}$'
        )
      ]);
    } else {
      passwordControl.setValidators([
        Validators.minLength(6),
        Validators.pattern(
          '^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^&*()_+\\-=\\[\\]{};:"\\\\|,.<>\\/?]).{8,}$'
        )
      ]);
    }

    passwordControl.updateValueAndValidity();
  }

  GetAllUsers() {
    this.authService.GetAllUsers(this.pagingFilterModel).subscribe(response => {
      this.UsersData = response.results;
      this.Total = response.totalCount;
    });
  }

  GetBranches() {
    this.authService.GetBranches(this.branchPagingFilterModel).subscribe(response => {
      this.BranchList = response.results;
    });
  }

  GetAllRoles() {
    this.authService.GetAllRoles().subscribe(response => {
      this.RolesList = response;
    });
  }

  onPageChange(page: number) {
    this.pagingFilterModel.currentPage = page;
    this.GetAllUsers();
  }

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  Register() {
    if (this.UserForm.invalid) {
      this.UserForm.markAllAsTouched();
      return;
    }

    if (!this.isEdit) {
      this.authService.register(this.UserForm.value).subscribe(response => {
        if (response.isSuccess) {
          this.UserForm.reset();
          this.GetAllUsers();
          this.sharedService.closeModal('UserModal');
          this.messageService.add({ severity: 'success', summary: 'تم التسجيل', detail: response.message });
        } else
          this.messageService.add({ severity: 'error', summary: 'فشل التسجيل', detail: response.message });
      });
    } else {
      this.authService.UpdateUserAsync(this.UserForm.value).subscribe(response => {
        if (response.isSuccess) {
          this.UserForm.reset();
          this.GetAllUsers();
          this.sharedService.closeModal('UserModal');
          this.messageService.add({ severity: 'success', summary: 'تم التعديل', detail: response.message });
        } else
          this.messageService.add({ severity: 'error', summary: 'فشل التعديل', detail: response.message });
      });
    }
  }
}
