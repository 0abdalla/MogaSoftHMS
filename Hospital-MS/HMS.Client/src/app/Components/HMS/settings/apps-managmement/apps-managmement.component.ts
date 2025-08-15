import { Component } from '@angular/core';
import { AppsService } from '../../../../Services/Permissions/apps.service';
import { AuthService } from '../../../../Auth/auth.service';
import { MessageService } from 'primeng/api';
import { MenueService } from '../../../../Services/menue.service';

@Component({
  selector: 'app-apps-managmement',
  templateUrl: './apps-managmement.component.html',
  styleUrl: './apps-managmement.component.css'
})
export class AppsManagmementComponent {
  TitleList = ['الإعدادات العامة', 'إدارة التطبيقات'];
  Pages: any[] = [];
  Roles: any[] = [];
  RoleId: any;
  permissions: { [key: string]: boolean } = {};

  constructor(private permissionService: AppsService, private authService: AuthService, private messageService: MessageService, private menueService: MenueService) {
    this.Pages = this.menueService.flattenMenuLevels(this.menueService.menus);
  }

  ngOnInit(): void {
    this.RoleId = '';
    this.GetAllRoles();
    this.GetAllPages();
    this.permissionService.permissions$.subscribe(permissions => {
      this.permissions = permissions;
    });
  }

  updatePermission(key: string): void {
    this.permissionService.updatePermission(key, this.permissions[key]);
  }

  GetAllRoles() {
    this.authService.GetAllRoles().subscribe((data: any) => {
      this.Roles = data;
    });
  }

  markSelected(menus, allowedPages) {
    for (const menu of menus) {
      if (menu.pageName) {
        menu.isSelected = allowedPages.includes(menu.pageName);
      }
      if (menu.subMenus && menu.subMenus.length > 0) {
        this.markSelected(menu.subMenus, allowedPages);
      }
    }
    return menus;
  }

  setPageId(menus, allowedPages) {
    for (const menu of menus) {
      if (menu.pageName) {
        let pageId = allowedPages.find(page => page.pageName == menu.pageName)?.pageId;
        menu.pageId = pageId;
      }
      if (menu.subMenus && menu.subMenus.length > 0) {
        this.setPageId(menu.subMenus, allowedPages);
      }
    }
    return menus;
  }

  GetAllPages() {
    this.authService.GetAllPages().subscribe((data: any) => {
      let allPages = data.results;
      this.Pages = this.setPageId(this.Pages, allPages);
    });
  }

  GetPagesByRoleId() {
    this.authService.GetPagesByRoleId(this.RoleId).subscribe((data: any) => {
      debugger;
      let allowedPages = data.results;
      this.Pages = this.markSelected(this.Pages, allowedPages);
    });
  }

  AssignRoleToPages() {
    debugger;
    if (!this.RoleId) {
      this.messageService.add({ severity: 'error', summary: 'خطأ', detail: 'برجاء اختيار صلاحية' });
      return;
    }

    const allUnselected = this.Pages.every(page => {
      const parentUnselected = !page.isSelected;
      const childrenUnselected = page.subMenus.every(child => !child.isSelected);
      return parentUnselected && childrenUnselected;
    });

    if (allUnselected) {
      this.messageService.add({ severity: 'error', summary: 'خطأ', detail: 'برجاء اختيار صفحة' });
      return;
    }

    let model = {
      roleId: this.RoleId,
      pageIds: this.Pages.flatMap(page => {
        const ids: number[] = [];
        if (page.isSelected) {
          ids.push(page.pageId);
        }
        page.subMenus.forEach(child => {
          if (child.isSelected) {
            ids.push(child.pageId);
          }
        });
        return ids;
      })
    }

    debugger;
    this.authService.AssignRoleToPages(model).subscribe((data: any) => {
      if (data.isSuccess) {
        this.messageService.add({ severity: 'success', summary: 'نجاح', detail: data.message });
      } else {
        this.messageService.add({ severity: 'error', summary: 'خطأ', detail: data.message });
      }
    });
  }
}
