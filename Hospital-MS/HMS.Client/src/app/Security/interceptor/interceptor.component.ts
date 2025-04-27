import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { LoaderService } from '../../Services/HMS/loader.service';

@Component({
  selector: 'app-interceptor',
  templateUrl: './interceptor.component.html',
  styleUrl: './interceptor.component.css'
})
export class InterceptorComponent {
  loading$: Observable<boolean>;

  constructor(private loaderService: LoaderService) {
    this.loading$ = this.loaderService.loading$;
  }
}
