import { HttpEvent, HttpHandlerFn, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable, catchError, finalize, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { LoaderService } from '../Services/HMS/loader.service';

export const loadingInterceptor: HttpInterceptorFn = (
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> => {
  const loaderService = inject(LoaderService);
  const router = inject(Router);

  loaderService.show();

  const token = sessionStorage.getItem('token');

  const authReq = token
    ? req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      })
    : req;

  return next(authReq).pipe(
    catchError(error => {
      if (error.status === 401) {
        console.warn('Unauthorized');
      }
      return throwError(() => error);
    }),
    finalize(() => {
      loaderService.hide();
    })
  );
};
