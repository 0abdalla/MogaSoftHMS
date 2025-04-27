import { Component, EventEmitter, Input, Output } from '@angular/core';
import {   NavigationEnd, Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  currentDate: string = '';
  userProfileImage: string = 'assets/vendors/imgs/avatar.png';
  searchQuery: string = '';
  routeNow: string = '';
  // 
  @Input() isCollapsed: boolean = false;
  @Output() sidebarToggled = new EventEmitter<boolean>();
  activeMenu: string | null = null;
  activeSubmenuRoute: string = '';

  constructor(private router : Router) { }

  ngOnInit(): void {
    this.setCurrentDate();
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.routeNow = this.getArabicRouteName(event.url);
      }
    });    
  }

  setCurrentDate(): void {
    const today = new Date();
    const day = today.getDate();
    const month = today.toLocaleString('ar-EG', { month: 'short' });
    const weekday = today.toLocaleString('ar-EG', { weekday: 'short' });
    this.currentDate = `اليوم، ${weekday} ${day} ${month}`;
  }

  getArabicRouteName(url: string): string {
    let path = url;
    try {
      if (url.includes('://')) {
        const urlObj = new URL(url);
        path = urlObj.pathname;
      }
      path = path.split('?')[0].split('#')[0];
      path = path.startsWith('/') ? path : `/${path}`;
      path = path.endsWith('/') ? path.slice(0, -1) : path;
    } catch (e) {
      console.error('Invalid URL format:', url);
    }
    const doctorsDynamicRoutePattern = /^\/settings\/doctors\/\d+$/;
    if (doctorsDynamicRoutePattern.test(path)) {
      return 'إعدادات الأطباء';
    }
    switch (path) {
      case '/':
      case '/home':
        return 'لوحة البيانات';
      case '/patients/list':
        return 'إدارة المرضى';
      case '/patients/add':
        return 'إضافة مريض';
      case '/appointments/list':
        return 'إدارة الحجوزات';
      case '/appointments/add':
        return 'إضافة حجز';
      case '/appointments/settings':
        return 'إعدادات الحجوزات';
      case '/emergency/emergency-reception':
        return 'الطوارئ';
      case '/staff/list':
        return 'إدارة الموظفين';
      case '/staff/add':
        return 'إضافة موظف';
      case '/reports':
        return 'التقارير';
      case '/settings/doctors':
        return 'إعدادات الأطباء';
      case '/settings/doctors-list':
        return 'إدارة الأطباء';
      default:
        console.warn('No matching route found for:', path);
        return 'الصفحة';
    }
  }

  toggleSidebar() {
    this.isCollapsed = !this.isCollapsed;
    this.sidebarToggled.emit(this.isCollapsed);
  }

  toggleSubMenu(menu: string) {
    this.activeMenu = menu;
    this.activeSubmenuRoute = this.router.url;
  }
}
