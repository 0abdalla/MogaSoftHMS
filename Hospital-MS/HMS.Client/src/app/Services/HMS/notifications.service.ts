import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Notifications } from '../../Models/HMS/notifications';
@Injectable({
  providedIn: 'root'
})
export class NotificationsService {
  baseUrl = environment.baseUrl;
  constructor(private http : HttpClient) { }
  getNotifications(): Observable<Notifications[]> {
    return this.http.get<Notifications[]>(`${this.baseUrl}/Notifications`);
  }
}
