import { Component, EventEmitter, HostListener, Input, Output } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { Notifications } from '../../Models/HMS/notifications';
import { NotificationsService } from '../../Services/HMS/notifications.service';
import { NotificationHubService } from '../../Services/HMS/notification-hub.service';

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
  notifications: Notifications[] = [];
  private subscriptions: Subscription[] = [];

  // 
  showNotifications = false;


  constructor(private router: Router ,  private notificationsService: NotificationsService, private notificationHubService: NotificationHubService) {}

  ngOnInit(): void {
    this.setCurrentDate();
    this.routeNow = this.getArabicRouteName(this.router.url);
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.routeNow = this.getArabicRouteName(event.url);
      }
    });
    const sub1 = this.notificationsService.getNotifications().subscribe(data => {
      this.notifications = data;
    });
    const userId = Number(sessionStorage.getItem('userId'));
    if (userId) {
      this.notificationHubService.startConnection(userId);
    }
    const sub2 = this.notificationHubService.notifications$.subscribe(latestList => {
      this.notifications = latestList;
    });

    this.subscriptions.push(sub1, sub2);
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
      case 'hms/staff/department-admin':
        return 'إدارة الأقسام';
      case '/hms/staff/job-levels':
        return 'إدارة المستويات الوظيفية';
      case '/hms/staff/job-classification':
        return 'إدارة التصنيف الوظيفي';
      case '/hms/staff/job-management':
        return 'إدارة الوظائف';
      case '/hms/reports':
        return 'التقارير';
      case '/hms/settings/doctors':
        return 'إعدادات الأطباء';
      case '/hms/settings/doctors-list':
        return 'إدارة الأطباء';
      case '/hms/settings/medical-services-list':
        return 'إدارة الخدمات الطبية';
      case '/hms/settings/medical-departments-list':
        return 'إدارة الأقسام';
      case '/hms/settings/account-tree':
        return 'شجرة الحسابات'
      case 'hms/settings/cost-center-tree':
        return 'مراكز التكلفة'
      case 'hms/fin-tree/year-fin-settings':
        return 'إعدادات السنة المالية'
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
        return 'أوامر توريد';
      case '/hms/fin-tree/purchase-request':
        return 'طلبات شراء';
      case '/hms/fin-tree/offers':
        return 'عروض أسعار';
      case '/hms/fin-tree/stores':
        return 'المخازن';
      case '/hms/fin-tree/stores-types':
        return 'أنواع المخازن';
      
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
  ngOnDestroy(): void {
    this.subscriptions.forEach(s => s.unsubscribe());
    this.notificationHubService.stopConnection();
  }
}
