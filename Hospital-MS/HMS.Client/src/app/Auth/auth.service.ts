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
        sessionStorage.setItem('role', response.results.role);
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
    sessionStorage.clear();
  }
  getToken(): string | null {
    return sessionStorage.getItem('token');
  }

  isInRole(roles: string[]): boolean {
    let role = sessionStorage.getItem('role');
    if (!role)
      return false;

    let ckeckRole = roles.some(i => i == role);
    return ckeckRole;
  }
}
