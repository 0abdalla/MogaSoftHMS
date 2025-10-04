import { Component } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Hospital';

  constructor(private router: Router) {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        document.body.classList.remove('modal-open');
        document.body.style.removeProperty('overflow');
        document.querySelectorAll('.modal-backdrop').forEach(el => el.remove());
      }
    });
  }
}

