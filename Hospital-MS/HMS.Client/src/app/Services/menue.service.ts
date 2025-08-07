import { Injectable } from '@angular/core';
import { MenuSidebarItem } from '../Models/Generics/MenuSidebarItem';

@Injectable({
  providedIn: 'root'
})
export class MenueService {
  menus: MenuSidebarItem[] = [
    {
      displayName: 'المرضى',
      icon: 'fa-solid fa-users',
      route: '/hms/patients/list',
      isGroup: false
    },
    {
      displayName: 'المواعيد والحجز',
      icon: 'fa-solid fa-calendar-check',
      route:'/hms/appointments/list',
      isGroup: false
    },
    {
      displayName: 'الطوارئ والإستقبال',
      icon: 'fa-solid fa-hospital-user',
      route: '/hms/emergency/emergency-reception',
      isGroup: false
    },
    {
      displayName: 'المشتريات',
      icon: 'fa-solid fa-boxes-stacked',
      isGroup: true,
      subMenus: [
        {
          displayName: 'طلب شراء',
          icon: 'fa-solid fa-circle',
          route: '/hms/fin-tree/purchase-request'
        },
        {
          displayName: 'عروض الأسعار',
          icon: 'fa-solid fa-circle',
          route: '/hms/fin-tree/offers'
        },
        {
          displayName: 'أمر توريد ',
          icon: 'fa-solid fa-circle',
          route: '/hms/fin-tree/purchase-order'
        },
        // {
        //   displayName: 'الموردين',
        //   icon: 'fa-solid fa-circle',
        //   route: '/hms/fin-tree/providers'
        // },
        
      ]
    },
    {
      displayName: 'المخازن',
      icon: 'fa-solid fa-boxes-stacked',
      isGroup: true,
      subMenus: [
        {
          displayName: 'إذن إستلام ',
          icon: 'fa-solid fa-circle',
          route: '/hms/fin-tree/add-items'
        },
        {
          displayName: 'طلب صرف ',
          icon: 'fa-solid fa-circle',
          route: '/hms/fin-tree/issue-request'
        },
        {
          displayName: 'إذن صرف',
          icon: 'fa-solid fa-circle',
          route: '/hms/fin-tree/issue-items'
        }
      ]
    },
    {
      displayName: 'الإدارة المالية',
      icon: 'fa-solid fa-money-bill-trend-up',
      isGroup: true,
      subMenus: [
        {
          displayName: 'حركة الخزينة',
          icon: 'fa-solid fa-circle',
          route: '/hms/fin-tree/treasury',
          // subMenus: [
          //   {
          //     displayName: 'إيصال توريد',
          //     icon: 'fa-solid fa-circle',
          //     route: '/hms/fin-tree/treasury/supply-receipt'
          //   },
          //   {
          //     displayName: 'إذن صرف نقدي',
          //     icon: 'fa-solid fa-circle',
          //     route: '/hms/fin-tree/treasury/exchange-permission'
          //   },
          //   {
          //     displayName: 'كشف حركة الخزينة',
          //     icon: 'fa-solid fa-circle',
          //     route: '/hms/fin-tree/treasury'
          //   },
          //   {
          //     displayName: 'إغلاق حركة الخزينة',
          //     icon: 'fa-solid fa-circle',
          //     route: '/hms/staff/classification'
          //   },
          // ]
        },
        {
          displayName: 'حركة البنك',
          icon: 'fa-solid fa-circle',
          subMenus: [
            {
              displayName: 'اشعار اضافة',
              icon: 'fa-solid fa-circle',
              route: '/hms/fin-tree/bank/add-notice'
            },
            {
              displayName: 'اشعار خصم',
              icon: 'fa-solid fa-circle',
              route: '/hms/fin-tree/bank/discount-notice'
            },
          ]
        },
        {
          displayName: 'القيود اليومية',
          icon: 'fa-solid fa-circle',
          route: '/hms/fin-tree/restrictions'
        },
        {
          displayName: 'رواتب الموظفين',
          icon: 'fa-solid fa-circle',
          route: '/hms/staff/salaries'
        },
        {
          displayName: 'سلف الموظفين',
          icon: 'fa-solid fa-circle',
          route: '/hms/staff/advances'
        }
      ]
    },
    {
      displayName: 'الموارد البشرية',
      icon: 'fa-solid fa-users-gear',
      isGroup: true,
      subMenus: [
        {
          displayName: 'بيانات الموظفين',
          icon: 'fa-solid fa-circle',
          subMenus: [
            {
              displayName: 'الموظفين',
              icon: 'fa-solid fa-circle',
              route: '/hms/staff/list'
            },
            {
              displayName: 'الأقسام',
              icon: 'fa-solid fa-circle',
              route: '/hms/staff/department-admin'
            },
            {
              displayName: 'المستويات الوظيفية',
              icon: 'fa-solid fa-circle',
              route: '/hms/staff/job-levels'
            },
            {
              displayName: 'تصنيف الوظائف',
              icon: 'fa-solid fa-circle',
              route: '/hms/staff/classification'
            },
            {
              displayName: 'الوظائف',
              icon: 'fa-solid fa-circle',
              route: '/hms/staff/job-management'
            }
          ]
        },
        {
          displayName: 'الأجازات',
          icon: 'fa-solid fa-circle',
          route: '/hms/staff/vacation'
        },
        {
          displayName: 'الجزاءات',
          icon: 'fa-solid fa-circle',
          route: '/hms/staff/penalty'
        },
        {
          displayName: 'الحضور والانصراف',
          icon: 'fa-solid fa-circle',
          route: '/hms/staff/attendance'
        }
      ]
    },
    {
      displayName: 'التقاير',
      icon: 'fa-solid fa-file-invoice',
      isGroup: true,
      subMenus: [
        {
          displayName: 'حساب الأستاذ',
          icon: 'fa-solid fa-circle',
          route: '/hms/reports/ledger-report',
        },
        {
          displayName: 'حركة المخزن',
          icon: 'fa-solid fa-circle',
          route: '/hms/reports/store-movement'
        },
        {
          displayName: 'حركة الصنف',
          icon: 'fa-solid fa-circle',
          route: '/hms/reports/item-movement'
        }
      ]
    },
    {
      displayName: 'إعدادات النظام',
      icon: 'fa-solid fa-gear',
      isGroup: true,
      subMenus: [
        {
          displayName: 'إعدادات المرضي',
          icon: 'fa-solid fa-circle',
          route: '/hms/patients/list',
        },
        {
          displayName: 'إعدادات المواعيد والحجز',
          icon: 'fa-solid fa-circle',
          route: '/hms/settings/medical-services-list'
        },
        {
          displayName: 'الأطباء',
          icon: 'fa-solid fa-boxes-stacked',
          isGroup: true,
          subMenus: [
            {
              displayName: 'الأطباء',
              icon: 'fa-solid fa-circle',
              route: '/hms/settings/doctors-list'
            },
            {
              displayName: 'نوع الخدمة',
              icon: 'fa-solid fa-circle',
              route: '/hms/settings/medical-service'
            }
          ]
        },
        {
          displayName: 'إعدادات وكلاء التامين',
          icon: 'fa-solid fa-circle',
          route: '/hms/insurance/insurance-list'
        },
        {
          displayName: 'إعدادات المخازن',
          icon: 'fa-solid fa-circle',
          subMenus:[
            {
              displayName: 'المجموعات الرئيسية',
              icon: 'fa-solid fa-circle',
              route: '/hms/fin-tree/main-groups'
            },
            {
              displayName: 'مجموعات الاصناف',
              icon: 'fa-solid fa-circle',
              route: '/hms/fin-tree/items-group'
            },
            {
              displayName: 'الاصناف',
              icon: 'fa-solid fa-circle',
              route: '/hms/fin-tree/items'
            },
            {
              displayName: 'الوحدات',
              icon: 'fa-solid fa-circle',
              route: '/hms/fin-tree/units'
            },
            {
              displayName: 'المخازن',
              icon: 'fa-solid fa-circle',
              route: '/hms/fin-tree/stores'
            },
            {
              displayName: 'أنواع المخازن',
              icon: 'fa-solid fa-circle',
              route: '/hms/fin-tree/stores-types'
            }
          ]
        },
        {
          displayName: 'إعدادات المشتريات',
          icon: 'fa-solid fa-circle',
          subMenus: [
            {
              displayName: 'الموردين',
              icon: 'fa-solid fa-circle',
              route: '/hms/fin-tree/providers'
            },
          ]
        },
        {
          displayName: 'إعدادات الإدارة المالية',
          icon: 'fa-solid fa-circle',
          subMenus: [
            {
              displayName: 'شجرة الحسابات',
              icon: 'fa-solid fa-circle',
              route: '/hms/settings/account-tree'
            },
            {
              displayName: 'الخزائن',
              icon: 'fa-solid fa-circle',
              route: '/hms/fin-tree/boxes'
            },
            {
              displayName: 'البنوك',
              icon: 'fa-solid fa-circle',
              route: '/hms/fin-tree/banks'
            },
            {
              displayName: 'إعدادات السنة المالية',
              icon: 'fa-solid fa-circle',
              route: '/hms/fin-tree/year-fin-settings'
            },
            {
              displayName: 'مراكز التكلفة',
              icon: 'fa-solid fa-circle',
              route: '/hms/settings/cost-center-tree'
            },
          ]
        },
        {
          displayName: 'إعدادات الموارد البشرية',
          icon: 'fa-solid fa-circle',
          route: ''
        },
      ]
    },
    {
      displayName: 'الإعدادات العامة',
      icon: 'fa-solid fa-gear',
      isGroup: true,
      subMenus: [
        {
          displayName: 'إدارة التطبيقات',
          icon: 'fa-solid fa-circle',
          route: '/hms/settings/apps-managmement',
        },
        {
          displayName: 'صلاحيات المستخدم',
          icon: 'fa-solid fa-circle',
          route: '/hms/settings/permissions'
        }
      ]
    }
  ];
}
