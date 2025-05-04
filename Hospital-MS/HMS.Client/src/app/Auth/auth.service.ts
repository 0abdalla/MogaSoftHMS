import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment';
import { User } from '../Models/HMS/user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) { }

  login(data: { email: string; password: string }): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}Auth/Login`, data).pipe(
      tap(response => {
       sessionStorage.setItem('userId', response.results.userId);
        sessionStorage.setItem('token', response.results.token);
        sessionStorage.setItem('firstName', response.results.firstName);
        sessionStorage.setItem('lastName', response.results.lastName);
      })
    );
  }

  register(data: User) {
    return this.http.post(`${this.baseUrl}Auth/register`, data);
  }

  getUserFromSession(): User | null {
    const user = sessionStorage.getItem('user');
    return user ? JSON.parse(user) : null;
  }

  logout(): void {
    sessionStorage.removeItem('userId');
    sessionStorage.removeItem('token');
    sessionStorage.removeItem('firstName');
    sessionStorage.removeItem('lastName');
  }
  getToken(): string | null {
    return sessionStorage.getItem('token');
  }

  isInRole(roles: string[]): boolean {
    let userModel = JSON.parse(localStorage.getItem('UserModel'));
    if (!userModel)
      return false;

    let ckeckRole = roles.some(i => i == userModel?.role);
    return ckeckRole;
  }
}
