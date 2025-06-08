import { Injectable } from '@angular/core';
import { MenuSidebarItem } from '../../Models/Generics/MenuSidebarItem';

@Injectable({
  providedIn: 'root'
})
export class MenueService {
  getMenuById(menuId: MenuType, subItemName: string = null): MenuSidebarItem {
    if (subItemName) {
      return this.menus.find(x => x.menuItemId == menuId)?.subMenus?.find(x => x.menuItem == subItemName);
    }
    return this.menus.find(x => x.menuItemId == menuId);
  }
  menus: MenuSidebarItem[] = [
    {
      menuItemId: MenuType.ZAInstitution,
      displayName: 'إدارة المستشفى',
      menuItem: 'HMS',
      subMenus: [
        {
          menuItemId: MenuType.ZAInstitution,
          displayName: 'المرضى',
          menuItem: '1',
          description: 'إدارة المرضى',
          icon: 'fa-solid fa-users',
          route: '/hms/home/1',
          subMenus: [
            {
              displayName: 'إدارة المرضى',
              menuItem: 'list',
              description: 'تتبع و إدارة المرضى',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/patients/list'
            },
            {
              displayName: 'إضافة مريض',
              menuItem: 'add',
              description: 'إضافة مريض جديد',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/patients/add'
            }
          ]
        },
        {
          menuItemId: MenuType.ZAInstitution,
          displayName: 'الحجوزات',
          menuItem: '2',
          description: 'إدارة و عرض الحجوزات',
          icon: 'fa-solid fa-calendar-check',
          route: '/hms/home/2',
          subMenus: [
            {
              displayName: 'إدارة الحجوزات',
              menuItem: 'list',
              description: 'إدارة و عرض الحجوزات',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/appointments/list'
            },
            {
              displayName: 'إضافة حجز',
              menuItem: 'add',
              description: 'إضافة حجز جديد',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/appointments/add'
            },
            {
              displayName: 'إعدادات الحجز',
              menuItem: 'settings',
              description: 'عرض إعدادات الحجز',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/appointments/settings'
            }
          ]
        },
        {
          menuItemId: MenuType.ZAInstitution,
          displayName: 'الطوارئ',
          menuItem: '3',
          description: 'إدارة الطوارئ',
          icon: 'fa-solid fa-hospital-user',
          route: '/hms/home/3',
          subMenus: [
            {
              displayName: 'الطوارئ',
              menuItem: 'emergency-reception',
              description: 'إدارة و عرض الطوارئ',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/emergency/emergency-reception'
            }
          ]
        },
        {
          menuItemId: MenuType.ZAInstitution,
          displayName: 'وكلاء التأمين',
          menuItem: '4',
          description: 'إدارة بيانات وكلاء التأمين',
          icon: 'fa-solid fa-file-contract',
          route: '/hms/home/4',
          subMenus: [
            {
              displayName: 'وكلاء التأمين',
              menuItem: 'insurance-list',
              description: 'إدارة بيانات وكلاء التأمين',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/insurance/insurance-list'
            },
            {
              displayName: 'إضافة وكيل التأمين',
              menuItem: 'add-insurance',
              description: 'إضافة وكيل التأمين جديد',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/insurance/add-insurance'
            }
          ]
        },
        {
          menuItemId: MenuType.ZAInstitution,
          displayName: 'الحسابات',
          menuItem: '5',
          description: 'إدارة الحسابات العامة',
          icon: 'fa-solid fa-money-bill-trend-up',
          route: '/hms/home/5',
          subMenus: [
            {
              displayName: 'إدارة الحسابات العامة',
              menuItem: 'accounts',
              description: 'عرض و إدارة الحسابات العامة',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/fin-tree/accounts'
            },
            {
              displayName: 'المعاملات المالية',
              menuItem: 'transactions',
              description: 'عرض و إدارة المعاملات المالية',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/finance/transactions'
            },
            {
              displayName: 'إدارة الخزائن',
              menuItem: 'boxes',
              description: 'عرض و إدارة الخزائن',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/fin-tree/boxes'
            },
            {
              displayName: 'إدارة البنوك',
              menuItem: 'banks',
              description: 'عرض و إدارة البنوك',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/fin-tree/banks'
            },
            {
              displayName: 'أذون الإضافة',
              menuItem: 'add-items',
              description: 'عرض و إدارة أذون الإضافة',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/fin-tree/add-items'
            },
            {
              displayName: 'أذون الصرف',
              menuItem: 'issue-items',
              description: 'عرض و إدارة أذون الصرف',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/fin-tree/issue-items'
            },
            {
              displayName: 'جرد المخازن',
              menuItem: 'fetch-inventory',
              description: 'عرض و إدارة جرد المخازن',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/fin-tree/fetch-inventory'
            },
            {
              displayName: 'حركة الخزينة',
              menuItem: 'treasury',
              description: 'عرض و إدارة حركة الخزينة',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/fin-tree/treasury'
            },
            {
              displayName: 'حركة البنك',
              menuItem: 'bank',
              description: 'عرض و إدارة حركة البنك',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/fin-tree/bank'
            },
            {
              displayName: 'القيود اليومية',
              menuItem: 'restrictions',
              description: 'عرض و إدارة القيود اليومية',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/fin-tree/restrictions'
            }
          ]
        },
        {
          menuItemId: MenuType.ZAInstitution,
          displayName: 'المشتريات والمخازن',
          menuItem: '6',
          description: 'تتبع و إدارة المشتريات',
          icon: 'fa-solid fa-boxes-stacked',
          route: '/hms/home/6',
          subMenus: [
            {
              displayName: 'المخزون',
              menuItem: 'stock',
              description: 'تتبع و إدارة المخزون',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/inventory/stock'
            },
            {
              displayName: 'المشتريات',
              menuItem: 'purchases',
              description: 'تتبع و إدارة المشتريات',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/inventory/purchases'
            },
            {
              displayName: 'المجموعات الرئيسية',
              menuItem: 'main-groups',
              description: 'تتبع و إدارة المجموعات',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/fin-tree/main-groups'
            },
            {
              displayName: 'مجموعة الأصناف',
              menuItem: 'items-group',
              description: 'تتبع و إدارة المجموعات',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/fin-tree/items-group'
            },
            {
              displayName: 'الأصناف',
              menuItem: 'items',
              description: 'تتبع و إدارة الأصناف',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/fin-tree/items'
            },
            {
              displayName: 'الموردين',
              menuItem: 'providers',
              description: 'تتبع و إدارة الموردين',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/fin-tree/providers'
            },
            {
              displayName: 'العملاء',
              menuItem: 'clients',
              description: 'تتبع و إدارة العملاء',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/fin-tree/clients'
            }

          ]
        },
        {
          menuItemId: MenuType.ZAInstitution,
          displayName: 'الموارد البشرية',
          menuItem: '7',
          description: 'إدارة الموارد البشرية',
          icon: 'fa-solid fa-users-gear',
          route: '/hms/home/7',
          subMenus: [
            {
              displayName: 'إدارة الموظفين',
              menuItem: 'list',
              description: 'عرض و إدارة الموظفين',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/staff/list'
            },
            {
              displayName: 'إضافة موظف',
              menuItem: 'add',
              description: 'إضافة موظف جديد',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/staff/add'
            },
            {
              displayName: 'الأقسام',
              menuItem: 'department-admin',
              description: 'عرض و إدارة الأقسام',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/staff/department-admin'
            },
            {
              displayName: 'المستويات الوظيفية',
              menuItem: 'job-levels',
              description: 'عرض و إدارة المستويات الوظيفية',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/staff/job-levels'
            },
            {
              displayName: 'تصنيف الوظائف',
              menuItem: 'classification',
              description: 'عرض و إدارة تصنيف الوظائف',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/staff/classification'
            },
            {
              displayName: 'الوظائف',
              menuItem: 'job-management',
              description: 'عرض و إدارة الوظائف',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/staff/job-management'
            }
          ]
        },
        {
          menuItemId: MenuType.ZAInstitution,
          displayName: 'التقارير',
          menuItem: '8',
          description: 'إدارة التقارير',
          icon: 'fa-solid fa-chart-column',
          route: '/hms/home/8',
          subMenus: [
            {
              displayName: 'تقارير المالية',
              menuItem: 'financial',
              description: 'عرض التقارير المالية',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/reports/financial'
            },
            {
              displayName: 'تقارير الطبية',
              menuItem: 'medical',
              description: 'عرض التقارير الطبية',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/reports/medical'
            },
            {
              displayName: 'تقارير حركة المخزن',
              menuItem: 'stock-movement-report',
              description: 'عرض تقارير حركة المخزن',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/reports/stock-movement-report'
            }
          ]
        },
        {
          menuItemId: MenuType.ZAInstitution,
          displayName: 'الاعدادات',
          menuItem: '9',
          description: 'إدارة الاعدادات',
          icon: 'fa-solid fa-gear',
          route: '/hms/home/9',
          subMenus: [
            {
              displayName: 'الإعدادات العامة',
              menuItem: 'general',
              description: 'عرض و إدارة الإعدادات العامة',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/settings/general'
            },
            {
              displayName: 'إدارة الأطباء',
              menuItem: 'doctors-list',
              description: 'عرض و إدارة الأطباء',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/settings/doctors-list'
            },
            {
              displayName: 'إضافة طبيب',
              menuItem: 'doctors',
              description: 'إضافة طبيب جديد',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/settings/doctors'
            },
            {
              displayName: 'إدارة الخدمات الطبية',
              menuItem: 'medical-services-list',
              description: 'عرض و إدارة الخدمات الطبية',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/settings/medical-services-list'
            },
            {
              displayName: 'تعيين مستخدم',
              menuItem: 'permissions',
              description: 'عرض و إدارة المستخدمين',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/settings/permissions'
            },
            {
              displayName: 'إدارة التطبيقات ',
              menuItem: 'apps-managmement',
              description: 'عرض و إدارة التطبيقات ',
              icon: 'uil uil-sliders-v-alt',
              route: '/hms/settings/apps-managmement'
            }
          ]
        }
      ]
    }
  ];
}
export enum MenuType {
  ZAInstitution = 1,
}
