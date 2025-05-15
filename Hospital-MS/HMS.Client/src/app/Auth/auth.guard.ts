import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from './auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const token = authService.getToken();
  if (token) {
    const allowedPage: string = route.data["pageName"];
    if (allowedPage) {
      if (authService.CheckUserAllowed(allowedPage)) {
        return true;
      } else {
        router.navigateByUrl('/not-authorized');
        return false;
      }
    }
    return true;
  } else {
    router.navigate(['/login']);
    return false;
  }
};
