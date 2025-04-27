import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoaderService {
  requestCount = 0;
  loadingSubject = new BehaviorSubject<boolean>(false);
  loading$: Observable<boolean> = this.loadingSubject.asObservable();
  

show(): void {
  this.requestCount++;
  if (this.requestCount > 0) {
    this.loadingSubject.next(true);
  }
}

hide(): void {
  this.requestCount--;
  if (this.requestCount <= 0) {
    this.requestCount = 0;
    this.loadingSubject.next(false);
  }
}
  reset(): void {
    this.requestCount = 0;
    this.loadingSubject.next(false);
  }
}