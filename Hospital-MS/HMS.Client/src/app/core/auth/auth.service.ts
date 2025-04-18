import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.prod';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/user';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) { }

  login(data: { email: string; password: string }): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/Auth/Login`, data).pipe(
      tap(response => {
        sessionStorage.setItem('userId', response.userId);
        sessionStorage.setItem('token', response.token);
        sessionStorage.setItem('firstName', response.firstName);
        sessionStorage.setItem('lastName', response.lastName);
      })
    );
  }

  register(data: User) {
    return this.http.post(`${this.baseUrl}/Auth/register`, data);
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
}
