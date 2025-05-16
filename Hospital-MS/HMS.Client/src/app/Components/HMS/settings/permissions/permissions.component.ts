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
  UserForm!: FormGroup;
  pagingFilterModel: PagingFilterModel = {
    searchText: '',
    currentPage: 1,
    pageSize: 16,
    filterList: []
  };
  UsersData: any[] = [];
  UsersList: any[] = [];
  RolesList: any[] = [];
  Total = 0;
  showPassword = false;

  constructor(private authService: AuthService, private fb: FormBuilder, private sharedService: SharedService, private messageService: MessageService) {
    this.UserForm = this.fb.group({
      staffId: ['', Validators.required],
      roleName: ['', Validators.required],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.pattern('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$')]],
      password: ['', [Validators.required, Validators.minLength(6), Validators.pattern(
        '^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^&*()_+\\-=\\[\\]{};:"\\\\|,.<>\\/?]).{8,}$'
      ),]],
      Address: ['']
    });
  }

  ngOnInit(): void {
    this.GetAllUsers();
    this.GetAllEmployees();
    this.GetAllRoles();
  }

  GetAllUsers() {
    this.authService.GetAllUsers(this.pagingFilterModel).subscribe(response => {
      this.UsersData = response.results;
      this.Total = response.totalCount;
    });
  }

  GetAllEmployees() {
    this.authService.GetAllEmployees().subscribe(response => {
      this.UsersList = response.results;
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

    this.authService.register(this.UserForm.value).subscribe(response => {
      if (response.isSuccess) {
        this.UserForm.reset();
        this.GetAllUsers();
        this.sharedService.closeModal('UserModal');
        this.messageService.add({ severity: 'success', summary: 'تم التسجيل', detail: response.message });
      } else
        this.messageService.add({ severity: 'error', summary: 'فشل التسجيل', detail: response.message });
    });
  }


}
