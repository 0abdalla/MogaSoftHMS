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
      isGroup: true,
      subMenus: [
        {
          displayName: 'الحجوزات',
          icon: 'fa-solid fa-circle',
          route: '/hms/appointments/list'
        }
      ],
    },
    {
      displayName: 'الطوارئ والإستقبال',
      icon: 'fa-solid fa-hospital-user',
      route: '/hms/emergency/emergency-reception',
      isGroup: false
    },
    {
      displayName: 'وكلاء التأمين',
      icon: 'fa-solid fa-file-contract',
      isGroup: true,
      subMenus: [
        {
          displayName: 'قائمة وكلاء التأمين',
          icon: 'fa-solid fa-circle',
          route: '/hms/insurance/insurance-list'
        },
        {
          displayName: 'إضافة وكيل التأمين',
          icon: 'fa-solid fa-circle',
          route: '/hms/insurance/add-insurance'
        }
      ]
    },
    {
      displayName: 'المخازن',
      icon: 'fa-solid fa-boxes-stacked',
      isGroup: true,
      subMenus: [
        {
          displayName: 'إذن إضافة',
          icon: 'fa-solid fa-circle',
          route: '/hms/inventory/stock'
        },
        {
          displayName: 'إذن صرف',
          icon: 'fa-solid fa-circle',
          route: '/hms/inventory/purchases'
        }
      ]
    },
    {
      displayName: 'المشتريات',
      icon: 'fa-solid fa-boxes-stacked',
      isGroup: true,
      subMenus: [
        {
          displayName: 'طلب شراء',
          icon: 'fa-solid fa-circle',
          route: ''
        },
        {
          displayName: 'أمر شراء',
          icon: 'fa-solid fa-circle',
          route: ''
        },
        {
          displayName: 'الموردين',
          icon: 'fa-solid fa-circle',
          route: ''
        },
        {
          displayName: 'عروض الأسعار',
          icon: 'fa-solid fa-circle',
          route: ''
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
          route: ''
        },
        {
          displayName: 'الجزاءات',
          icon: 'fa-solid fa-circle',
          route: ''
        },
        {
          displayName: 'الحضور والانصراف',
          icon: 'fa-solid fa-circle',
          route: ''
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
          subMenus: [
            {
              displayName: 'ايصال استلام نقدية',
              icon: 'fa-solid fa-circle',
              route: ''
            },
            {
              displayName: 'إذن صرف نقدي',
              icon: 'fa-solid fa-circle',
              route: ''
            },
            {
              displayName: 'كشف حركة الخزينة',
              icon: 'fa-solid fa-circle',
              route: '/hms/fin-tree/treasury'
            },
            {
              displayName: 'إغلاق حركة الخزينة',
              icon: 'fa-solid fa-circle',
              route: '/hms/staff/classification'
            },
          ]
        },
        {
          displayName: 'حركة البنك',
          icon: 'fa-solid fa-circle',
          subMenus: [
            {
              displayName: 'اشعار اضافة',
              icon: 'fa-solid fa-circle',
              route: ''
            },
            {
              displayName: 'اشعار خصم',
              icon: 'fa-solid fa-circle',
              route: ''
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
          route: ''
        },
        {
          displayName: 'سلف الموظفين',
          icon: 'fa-solid fa-circle',
          route: ''
        }
      ]
    },
    {
      displayName: 'إعدادات النظام',
      icon: 'fa-solid fa-gear',
      isGroup: true,
      subMenus: [
        {
          displayName: 'إعدادات المرضى',
          icon: 'fa-solid fa-circle',
          route: '',
        },
        {
          displayName: 'إعدادات المواعيد والحجز',
          icon: 'fa-solid fa-circle',
          route: ''
        },
        {
          displayName: 'إعدادات وكلاء التامين',
          icon: 'fa-solid fa-circle',
          route: ''
        },
        {
          displayName: 'إعدادات المخازن',
          icon: 'fa-solid fa-circle',
          route: '',
        },
        {
          displayName: 'إعدادات المشتريات',
          icon: 'fa-solid fa-circle',
          subMenus: [
            {
              displayName: 'الموردين',
              icon: 'fa-solid fa-circle',
              route: ''
            },
          ]
        },
        {
          displayName: 'إعدادات الإدارة المالية',
          icon: 'fa-solid fa-circle',
          subMenus: [
            {
              displayName: 'الخزائن والبنوك',
              icon: 'fa-solid fa-circle',
              route: ''
            },
            {
              displayName: 'شجرة الحسابات',
              icon: 'fa-solid fa-circle',
              route: ''
            },
            {
              displayName: 'مراكز التكلفة',
              icon: 'fa-solid fa-circle',
              route: ''
            },
            {
              displayName: 'إعدادات السنة المالية',
              icon: 'fa-solid fa-circle',
              route: ''
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
          route: '',
        },
        {
          displayName: 'صلاحيات المستخدم',
          icon: 'fa-solid fa-circle',
          route: ''
        }
      ]
    }

    // {
    //   displayName: 'التقارير',
    //   icon: 'fa-solid fa-chart-column',
    //   subMenus: [
    //     {
    //       displayName: 'تقارير المالية',
    //       icon: 'fa-solid fa-circle',
    //       route: '/hms/reports/financial'
    //     },
    //     {
    //       displayName: 'تقارير الطبية',
    //       icon: 'fa-solid fa-circle',
    //       route: '/hms/reports/medical'
    //     },
    //     {
    //       displayName: 'تقارير حركة المخزن',
    //       icon: 'fa-solid fa-circle',
    //       route: '/hms/reports/stock-movement-report'
    //     }
    //   ]
    // },
  ];
}
