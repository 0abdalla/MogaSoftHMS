import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-not-authorized',
  templateUrl: './not-authorized.component.html',
  styleUrl: './not-authorized.component.css'
})
export class NotAuthorizedComponent {

  constructor(private router: Router) {}


  Logout() {
    localStorage.clear();
    sessionStorage.clear();
    this.router.navigate(['/auth/login']);
  }
}
