import { Injectable, OnDestroy } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Notifications } from '../../Models/HMS/notifications';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class NotificationHubService implements OnDestroy {
  private hubConnection!: signalR.HubConnection;
  private notificationsSubject = new BehaviorSubject<Notifications[]>([]);
  private latestNotificationSubject = new BehaviorSubject<Notifications | null>(null);

  notifications$ = this.notificationsSubject.asObservable();
  latestNotification$ = this.latestNotificationSubject.asObservable();

  private readonly hubUrl = `${environment.baseUrl}/hub/notifications`;

  constructor(private http: HttpClient) {}

  startConnection(userId: number): void {
    if (this.hubConnection?.state === signalR.HubConnectionState.Connected) {
      console.log('SignalR: Already connected');
      return;
    }

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${this.hubUrl}?userId=${userId}`, { withCredentials: true })
      .withAutomaticReconnect()
      .build();

    this.registerHandlers();

    this.hubConnection.start()
      .then(() => console.log(`SignalR: Connected for userId ${userId}`))
      .catch(err => {
        console.error('SignalR: Initial connection failed', err);
        setTimeout(() => this.startConnection(userId), 5000);
      });
  }

  private registerHandlers(): void {
    this.hubConnection.on('ReceiveNotification', (notification: Notifications) => {
      if (notification) {
        this.notificationsSubject.next([notification, ...this.notificationsSubject.getValue()]);
        this.latestNotificationSubject.next(notification);
      }
    });

    this.hubConnection.onreconnecting(err => console.warn('SignalR: Reconnecting...', err));
    this.hubConnection.onreconnected(() => console.log('SignalR: Reconnected'));
    this.hubConnection.onclose(err => console.warn('SignalR: Closed', err));
  }

  stopConnection(): void {
    if (this.hubConnection) {
      this.hubConnection.off('ReceiveNotification');
      this.hubConnection.stop()
        .then(() => console.log('SignalR: Disconnected'))
        .catch(err => console.error('SignalR: Error stopping connection', err));
    }
  }
  ngOnDestroy(): void {
    this.stopConnection();
  }
}
