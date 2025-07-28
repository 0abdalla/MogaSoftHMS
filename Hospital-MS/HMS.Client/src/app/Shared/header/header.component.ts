import { Component, EventEmitter, HostListener, Input, Output } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {
  currentDate: string = '';
  userProfileImage: string = 'assets/vendors/imgs/avatar.png';
  searchQuery: string = '';
  routeNow: string = '';

  @Input() isCollapsed: boolean = false;
  @Output() sidebarToggled = new EventEmitter<boolean>();
  activeMenu: string | null = null;
  activeSubmenuRoute: string = '';
  // 
  showNotifications = false;
  notifications = ['إشعار 1', 'إشعار 2', 'إشعار 3'];


  constructor(private router: Router) {}

  ngOnInit(): void {
    this.setCurrentDate();
    this.routeNow = this.getArabicRouteName(this.router.url);
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
      case '/hms/home':
        return 'لوحة البيانات';
      case '/hms/patients/list':
        return 'إدارة المرضى';
      case '/hms/patients/add':
        return 'إضافة مريض';
      case '/hms/appointments/list':
        return 'المواعيد والحجز ';
      case '/hms/appointments/add':
        return 'إضافة حجز';
      case '/hms/appointments/settings':
        return 'إعدادات الحجوزات';
      case '/hms/emergency/emergency-reception':
        return 'الطوارئ والاستقبال' ;
      case '/hms/staff/list':
        return 'إدارة الموظفين';
      case '/hms/staff/add':
        return 'إضافة موظف';
      case '/hms/reports':
        return 'التقارير';
      case '/hms/settings/doctors':
        return 'إعدادات الأطباء';
      case '/hms/settings/doctors-list':
        return 'إدارة الأطباء';
      case '/hms/settings/medical-services-list':
        return 'إدارة الخدمات الطبية';
      case '/hms/staff/progression':
        return 'إدارة التدرج الوظيفي';
      case '/hms/staff/classification':
        return 'إدارة التصنيف الوظيفي';
      case '/hms/insurance/insurance-list':
        return 'إدارة التأمينات';
      case '/hms/insurance/add-insurance':
        return 'إضافة وكيل تأمين';
      case '/hms/fin-tree/main-groups':
        return 'المجموعات الرئيسية';
      case '/hms/fin-tree/items-group':
        return 'مجموعة الأصناف';
      case '/hms/fin-tree/items':
        return 'الأصناف';
      case '/hms/fin-tree/units':
        return 'وحدات الأصناف';
      case '/hms/fin-tree/providers':
        return 'الموردين';
      case '/hms/fin-tree/clients':
        return 'العملاء';
      case '/hms/fin-tree/boxes':
        return 'الخزائن';
      case '/hms/fin-tree/banks':
        return 'البنوك';
      case '/hms/fin-tree/accounts':
        return 'الحسابات العامة';
      case '/hms/fin-tree/add-items':
        return 'أذون الإستلام';
      case '/hms/fin-tree/issue-items':
        return 'أذون الصرف';
      case '/hms/fin-tree/issue-request':
        return 'طلبات الصرف';
      case '/hms/fin-tree/fetch-inventory':
        return 'جرد المخازن';
      case '/hms/fin-tree/treasury':
        return 'حركة الخزينة';
      case '/hms/fin-tree/treasury/supply-receipt':
        return 'إيصال توريد';
      case '/hms/fin-tree/treasury/exchange-permission':
        return 'إذن صرف نقدي';
      case '/hms/fin-tree/bank':
        return 'حركة البنوك';
      case '/hms/fin-tree/bank/add-notice':
        return 'إشعار إضافة';
      case '/hms/fin-tree/bank/discount-notice':
        return 'إشعار خصم';
      case '/hms/fin-tree/restrictions':
        return 'قيود اليومية';
      case '/hms/fin-tree/purchase-order':
        return 'أوامر شراء';
      case '/hms/fin-tree/purchase-request':
        return 'طلبات شراء';
      case '/hms/fin-tree/offers':
        return 'عروض أسعار';
      default:
        console.warn('No matching route found for:', path);
        return 'Infinity Clinic';
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
  // 
  toggleNotifications() {
    this.showNotifications = !this.showNotifications;
  }

  @HostListener('document:click', ['$event'])
  handleClickOutside(event: Event) {
    const target = event.target as HTMLElement;
    const bellClicked = target.closest('.fa-bell');
    const boxClicked = target.closest('.notification-box');
    if (!bellClicked && !boxClicked) {
      this.showNotifications = false;
    }
  }
}
