import { Component, EventEmitter, Output } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import Swal from 'sweetalert2';
import { filter } from 'rxjs';
import { AuthService } from '../../Auth/auth.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent {
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
      '/home': 'home',
      '/patients/list': 'patients',
      '/patients/add': 'patients',
      '/appointments/list': 'appointments',
      '/appointments/add': 'appointments',
      '/appointments/settings': 'appointments',
      '/emergency/emergency-reception': 'emergency',
      '/insurance/insurance-list': 'insurance',
      '/insurance/add-insurance': 'insurance',
      '/finance/accounts': 'finance',
      '/finance/transactions': 'finance',
      '/inventory/stock': 'inventory',
      '/inventory/purchases': 'inventory',
      '/staff/list': 'staff',
      '/staff/add': 'staff',
      '/reports/financial': 'reports',
      '/reports/medical': 'reports',
      '/settings/general': 'settings',
      '/settings/doctors': 'settings',
      '/settings/doctors-list': 'settings'
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
