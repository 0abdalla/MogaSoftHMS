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
      pageName: 'PatientList',
      isGroup: false
    },
    {
      displayName: 'المواعيد والحجز',
      icon: 'fa-solid fa-calendar-check',
      route: '/hms/appointments/list',
      pageName: 'AppointmentList',
      isGroup: false
    },
    {
      displayName: 'الطوارئ والإستقبال',
      icon: 'fa-solid fa-hospital-user',
      route: '/hms/emergency/emergency-reception',
      pageName: 'EmergencyReception',
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
          route: '/hms/fin-tree/purchase-request',
          pageName: 'PurchaseRequest'
        },
        {
          displayName: 'عروض الأسعار',
          icon: 'fa-solid fa-circle',
          route: '/hms/fin-tree/offers',
          pageName: 'Offers'
        },
        {
          displayName: 'أمر توريد ',
          icon: 'fa-solid fa-circle',
          route: '/hms/fin-tree/purchase-order',
          pageName: 'PurchaseOrder'
        }
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
          route: '/hms/fin-tree/add-items',
          pageName: 'AddItems'
        },
        {
          displayName: 'طلب صرف ',
          icon: 'fa-solid fa-circle',
          route: '/hms/fin-tree/issue-request',
          pageName: 'IssueRequest'
        },
        {
          displayName: 'إذن صرف',
          icon: 'fa-solid fa-circle',
          route: '/hms/fin-tree/issue-items',
          pageName: 'IssueItems'
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
          pageName: 'Treasury'
        },
        {
          displayName: 'حركة البنك',
          icon: 'fa-solid fa-circle',
          subMenus: [
            {
              displayName: 'اشعار اضافة',
              icon: 'fa-solid fa-circle',
              route: '/hms/fin-tree/bank/add-notice',
              pageName: 'AddNotice'
            },
            {
              displayName: 'اشعار خصم',
              icon: 'fa-solid fa-circle',
              route: '/hms/fin-tree/bank/discount-notice',
              pageName: 'DiscountNotice'
            },
          ]
        },
        {
          displayName: 'القيود اليومية',
          icon: 'fa-solid fa-circle',
          route: '/hms/fin-tree/restrictions',
          pageName: 'Restrictions'
        },
        {
          displayName: 'رواتب الموظفين',
          icon: 'fa-solid fa-circle',
          route: '/hms/staff/salaries',
          pageName: 'Salaries'
        },
        {
          displayName: 'سلف الموظفين',
          icon: 'fa-solid fa-circle',
          route: '/hms/staff/advances',
          pageName: 'Advances'
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
              route: '/hms/staff/list',
              pageName: 'StaffList'
            },
            {
              displayName: 'الأقسام',
              icon: 'fa-solid fa-circle',
              route: '/hms/staff/department-admin',
              pageName: 'DepartmentAdmin'
            },
            {
              displayName: 'المستويات الوظيفية',
              icon: 'fa-solid fa-circle',
              route: '/hms/staff/job-levels',
              pageName: 'JobLevels'
            },
            {
              displayName: 'تصنيف الوظائف',
              icon: 'fa-solid fa-circle',
              route: '/hms/staff/classification',
              pageName: 'Classification'
            },
            {
              displayName: 'الوظائف',
              icon: 'fa-solid fa-circle',
              route: '/hms/staff/job-management',
              pageName: 'JobManagement'
            }
          ]
        },
        {
          displayName: 'الأجازات',
          icon: 'fa-solid fa-circle',
          route: '/hms/staff/vacation',
          pageName: 'Vacation'
        },
        {
          displayName: 'الجزاءات',
          icon: 'fa-solid fa-circle',
          route: '/hms/staff/penalty',
          pageName: 'Penalty'
        },
        {
          displayName: 'الحضور والانصراف',
          icon: 'fa-solid fa-circle',
          route: '/hms/staff/attendance',
          pageName: 'Attendance'
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
          pageName: 'LedgerReport'
        },
        {
          displayName: 'حركة المخزن',
          icon: 'fa-solid fa-circle',
          route: '/hms/reports/store-movement',
          pageName: 'StoreMovement'
        },
        {
          displayName: 'حركة الصنف',
          icon: 'fa-solid fa-circle',
          route: '/hms/reports/item-movement',
          pageName: 'ItemMovement'
        },
        {
          displayName: 'حد الطلب',
          icon: 'fa-solid fa-circle',
          route: '/hms/reports/item-order-limit',
          pageName: 'ItemOrderLimit'
        },
        {
          displayName: 'تقييم المخزون',
          icon: 'fa-solid fa-circle',
          route: '/hms/reports/store-rate',
          pageName: 'StoreRate'
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
          pageName: 'PatientsListSettings'
        },
        {
          displayName: ' المواعيد',
          icon: 'fa-solid fa-circle',
          route: '/hms/settings/medical-services-list',
          pageName: 'MedicalServicesList',
          isGroup: true,
          subMenus:[
            {
              displayName: 'الطوابق',
              icon: 'fa-solid fa-circle',
              route: '/hms/settings/floors',
              pageName: 'Floors'
            },
            {
              displayName: 'الغرف',
              icon: 'fa-solid fa-circle',
              route: '/hms/settings/rooms',
              pageName: 'Rooms'
            },
            {
              displayName: 'الأسرّة',
              icon: 'fa-solid fa-circle',
              route: '/hms/settings/beds',
              pageName: 'Beds'
            }
          ]
        },
        {
          displayName: 'الأطباء',
          icon: 'fa-solid fa-circle',
          isGroup: true,
          subMenus: [
            {
              displayName: 'الأطباء',
              icon: 'fa-solid fa-circle',
              route: '/hms/settings/doctors-list',
              pageName: 'DoctorList'
            },
            {
              displayName: 'نوع الخدمة',
              icon: 'fa-solid fa-circle',
              route: '/hms/settings/medical-services-list',
              pageName: 'MedicalService'
            },
            {
              displayName: 'الأقسام',
              icon: 'fa-solid fa-circle',
              route: '/hms/settings/medical-departments-list',
              pageName: 'DoctorsDepartmentsList'
            }
          ]
        },
        {
          displayName: ' وكلاء التامين',
          icon: 'fa-solid fa-circle',
          route: '/hms/insurance/insurance-list',
          pageName: 'InsuranceList'
        },
        {
          displayName: 'المخازن',
          icon: 'fa-solid fa-circle',
          subMenus: [
            {
              displayName: 'المجموعات الرئيسية',
              icon: 'fa-solid fa-circle',
              route: '/hms/fin-tree/main-groups',
              pageName: 'MainGroups'
            },
            {
              displayName: 'مجموعات الاصناف',
              icon: 'fa-solid fa-circle',
              route: '/hms/fin-tree/items-group',
              pageName: 'ItemsGroup'
            },
            {
              displayName: 'الوحدات',
              icon: 'fa-solid fa-circle',
              route: '/hms/fin-tree/units',
              pageName: 'Units'
            },
            {
              displayName: 'الاصناف',
              icon: 'fa-solid fa-circle',
              route: '/hms/fin-tree/items',
              pageName: 'Items'
            },
            {
              displayName: 'أنواع المخازن',
              icon: 'fa-solid fa-circle',
              route: '/hms/fin-tree/stores-types',
              pageName: 'StoreTypes'
            },
            {
              displayName: 'المخازن',
              icon: 'fa-solid fa-circle',
              route: '/hms/fin-tree/stores',
              pageName: 'Stores'
            },
          ]
        },
        {
          displayName: 'المشتريات',
          icon: 'fa-solid fa-circle',
          subMenus: [
            {
              displayName: 'الموردين',
              icon: 'fa-solid fa-circle',
              route: '/hms/fin-tree/providers',
              pageName: 'Providers'
            },
          ]
        },
        {
          displayName: 'الإدارة المالية',
          icon: 'fa-solid fa-circle',
          subMenus: [
            {
              displayName: 'شجرة الحسابات',
              icon: 'fa-solid fa-circle',
              route: '/hms/settings/account-tree',
              pageName: 'AccountTree'
            },
            {
              displayName: 'التوجيهات المحاسبية',
              icon: 'fa-solid fa-circle',
              route: '/hms/fin-tree/account-guidance',
              pageName: 'AccountGuidance'
            },
            {
              displayName: 'الخزائن',
              icon: 'fa-solid fa-circle',
              route: '/hms/fin-tree/boxes',
              pageName: 'Boxes'
            },
            {
              displayName: 'البنوك',
              icon: 'fa-solid fa-circle',
              route: '/hms/fin-tree/banks',
              pageName: 'Banks'
            },
            {
              displayName: 'السنة المالية',
              icon: 'fa-solid fa-circle',
              route: '/hms/fin-tree/year-fin-settings',
              pageName: 'YeaFinSettings'
            },
            {
              displayName: 'مراكز التكلفة',
              icon: 'fa-solid fa-circle',
              route: '/hms/settings/cost-center-tree',
              pageName: 'CostCenterTree'
            }
          ]
        }
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
          pageName: 'AppsManagmement'
        },
        {
          displayName: 'صلاحيات المستخدم',
          icon: 'fa-solid fa-circle',
          route: '/hms/settings/permissions',
          pageName: 'Permissions'
        }
      ]
    }
  ];

  getFilteredMenus(): MenuSidebarItem[] {
    const pagesStr = sessionStorage.getItem('pages') ?? '';
    const allowedPages = pagesStr.split(',').map(p => p.trim()).filter(Boolean);
    const allowedSet = new Set(allowedPages);

    const filterRecursive = (items: MenuSidebarItem[]): MenuSidebarItem[] => {
      return (items || [])
        .map(menu => {
          const filteredChildren = menu.subMenus?.length
            ? filterRecursive(menu.subMenus)
            : undefined;

          const hasPage = !!menu.pageName && allowedSet.has(menu.pageName);
          const hasChildren = !!filteredChildren && filteredChildren.length > 0;

          if (!hasPage && !hasChildren) return null;

          return {
            ...menu,
            ...(hasChildren ? { subMenus: filteredChildren } : {})
          } as MenuSidebarItem;
        })
        .filter((x): x is MenuSidebarItem => x !== null);
    };

    return filterRecursive(this.menus);
  }

  flattenMenuLevels(menuList: MenuSidebarItem[]): MenuSidebarItem[] {
    return menuList.map(menu => {
      let flatChildren: MenuSidebarItem[] = [];
      if (menu.subMenus && menu.subMenus.length > 0) {
        function collectChildren(items: MenuSidebarItem[]) {
          for (const item of items) {
            const { subMenus, ...rest } = item;
            if (item.route) {
              flatChildren.push({ ...rest, route: item.route, subMenus: [] });
            }

            if (subMenus && subMenus.length > 0) {
              collectChildren(subMenus);
            }
          }
        }
        collectChildren(menu.subMenus);
      }

      return {
        ...menu,
        subMenus: flatChildren.length > 0 ? flatChildren : []
      };
    });
  }
}
