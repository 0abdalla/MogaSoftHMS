import { Component, EventEmitter, Output } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import Swal from 'sweetalert2';
import { filter } from 'rxjs';
import { AuthService } from '../../../../Auth/auth.service';

@Component({
  selector: 'app-hms-side-menue',
  templateUrl: './hms-side-menue.component.html',
  styleUrl: './hms-side-menue.component.css'
})
export class HMSSideMenueComponent {
  isCollapsed: boolean = false;
  activeMenu: string | null = null;
  activeSubmenuRoute: string = '';

  @Output() sidebarToggled = new EventEmitter<boolean>();

  constructor(private router: Router, private authService: AuthService) {}

  ngOnInit() {
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {
        this.setActiveMenuBasedOnRoute();
      });

    this.setActiveMenuBasedOnRoute();
  }

  setActiveMenuBasedOnRoute() {
    const currentRoute = this.router.url
    const routeMenuMap: { [key: string]: string } = {
      '/hms/home': 'home',
      '/hms/patients/list': 'patients',
      '/hms/patients/add': 'patients',
      '/hms/appointments/list': 'appointments',
      '/hms/appointments/add': 'appointments',
      '/hms/appointments/settings': 'appointments',
      '/hms/emergency/emergency-reception': 'emergency',
      '/hms/insurance/insurance-list': 'insurance',
      '/hms/insurance/add-insurance': 'insurance',
      '/hms/finance/accounts': 'finance',
      '/hms/finance/transactions': 'finance',
      '/hms/inventory/stock': 'inventory',
      '/hms/inventory/purchases': 'inventory',
      '/hms/staff/list': 'staff',
      '/hms/staff/add': 'staff',
      '/hms/reports/financial': 'reports',
      '/hms/reports/medical': 'reports',
      '/hms/settings/general': 'settings',
      '/hms/settings/doctors': 'settings',
      '/hms/settings/doctors-list': 'settings'
    };

    for (const route in routeMenuMap) {
      if (currentRoute.startsWith(route)) {
        this.activeMenu = routeMenuMap[route];
        this.activeSubmenuRoute = currentRoute;
        return;
      }
    }
    this.activeMenu = null;
    this.activeSubmenuRoute = '';
  }

  toggleSubMenu(menu: string) {
    this.activeMenu = this.activeMenu === menu ? null : menu;
  }

  toggleSidebar() {
    this.isCollapsed = !this.isCollapsed;
    this.sidebarToggled.emit(this.isCollapsed);
  }

  logout() {
    Swal.fire({
      title: 'هل أنت متأكد؟',
      icon: 'question',
      showCancelButton: true,
      confirmButtonColor: '#3D5DA7',
      cancelButtonColor: '#ED3B93',
      confirmButtonText: 'نعم',
      cancelButtonText: 'لا'
    }).then((result) => {
      if (result.isConfirmed) {
        this.authService.logout();
        this.router.navigate(['/login']);
      }
    });
  }
}
