import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, shareReplay, tap } from 'rxjs';
import { environment } from '../../environments/environment';
import { User } from '../Models/HMS/user';
import { PagingFilterModel } from '../Models/Generics/PagingFilterModel';
import { PagedResponseModel } from '../Models/Generics/PagedResponseModel';
import { ErrorResponseModel } from '../Models/Generics/ErrorResponseModel';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.baseUrl;
  private allowedPagesCache = new Map<string, Observable<any>>();

  constructor(private http: HttpClient) { }

  login(data: { email: string; password: string }): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}Auth/Login`, data).pipe(
      tap(response => {
        sessionStorage.setItem('userId', response.results.userId);
        sessionStorage.setItem('token', response.results.token);
        sessionStorage.setItem('firstName', response.results.firstName);
        sessionStorage.setItem('lastName', response.results.lastName);
        sessionStorage.setItem('role', response.results.role);
        sessionStorage.setItem('pages', response.results.pages);
      })
    );
  }

  register(data: any) {
    return this.http.post<ErrorResponseModel<string>>(`${this.baseUrl}Auth/register`, data);
  }

  getUserFromSession(): User | null {
    const user = sessionStorage.getItem('user');
    return user ? JSON.parse(user) : null;
  }

  GetAllUsers(pagingFilter: PagingFilterModel) {
    return this.http.post<PagedResponseModel<any>>(`${this.baseUrl}Users/GetAllUsers`, pagingFilter);
  }

  GetManageRolePages() {
    return this.http.get(`${this.baseUrl}ManageRoles/GetManageRolePages`);
  }

  AssignRoleToPages(Model: any) {
    return this.http.post(`${this.baseUrl}ManageRoles/AssignRoleToPages`, Model);
  }

  GetAllRoles() {
    return this.http.get<any>(`${this.baseUrl}ManageRoles/GetAllRoles`);
  }

  GetPagesByRoleId(RoleId: any) {
    return this.http.get(`${this.baseUrl}ManageRoles/GetPagesByRoleId?RoleId=${RoleId}`);
  }

  GetAllEmployees() {
    return this.http.get<PagedResponseModel<any>>(`${this.baseUrl}Users/GetAllEmployees`);
  }

  GetAllowedPagesByRoleName(RoleName: any): Observable<any> {
    if (this.allowedPagesCache.has(RoleName)) {
      return this.allowedPagesCache.get(RoleName)!;
    }

    const request = this.http
      .get(`${this.baseUrl}ManageRoles/GetAllowedPagesByRoleName?RoleName=${RoleName}`)
      .pipe(
        shareReplay(1)
      );

    this.allowedPagesCache.set(RoleName, request);
    return request;
  }

  logout(): void {
    sessionStorage.clear();
  }
  getToken(): string | null {
    return sessionStorage.getItem('token');
  }

  CheckUserAllowed(pageName: string): boolean {
    let pages = sessionStorage.getItem('pages').split(',');
    return pages ? pages.some(i => i == pageName) : false;
  }

  isInRole(roles: string[]): boolean {
    let role = sessionStorage.getItem('role');
    if (!role)
      return false;

    let ckeckRole = roles.some(i => i == role);
    return ckeckRole;
  }
}
