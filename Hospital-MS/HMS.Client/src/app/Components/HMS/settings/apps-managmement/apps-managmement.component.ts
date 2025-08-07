import { Component } from '@angular/core';
import { AppsService } from '../../../../Services/Permissions/apps.service';
import { AuthService } from '../../../../Auth/auth.service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-apps-managmement',
  templateUrl: './apps-managmement.component.html',
  styleUrl: './apps-managmement.component.css'
})
export class AppsManagmementComponent {
  TitleList = ['الإعدادات العامة','إدارة التطبيقات'];
  Pages: any[] = [];
  Roles: any[] = [];
  RoleId: any;
  permissions: { [key: string]: boolean } = {};

  constructor(private permissionService: AppsService, private authService: AuthService, private messageService: MessageService) { }

  ngOnInit(): void {
    this.RoleId = '';
    this.GetAllRoles();
    this.GetManageRolePages();
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

  GetManageRolePages() {
    this.authService.GetManageRolePages().subscribe((data: any) => {
      this.Pages = data;
      this.Pages.forEach(page => {
        if (page.children.length == 0) {
          page.children = [
            {
              id: page.id,
              nameAR: page.nameAR,
            }
          ];
          console.log(page.children);
          
        }
      });
    });
  }

  GetPagesByRoleId() {
    this.authService.GetPagesByRoleId(this.RoleId).subscribe((data: any) => {
      this.Pages.forEach(page => {
        page.isSelected = false;
        page.children.forEach(child => {
          child.isSelected = false;
        });
      });
      this.Pages.forEach(page => {
        page.children.forEach(child => {
          let obj = data.results.find((item: any) => item.pageId == child.id);
          if (obj) {
            child.isSelected = true;
          }
        });
      })
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
      const childrenUnselected = page.children.every(child => !child.isSelected);
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
          ids.push(page.id);
        }
        page.children.forEach(child => {
          if (child.isSelected) {
            ids.push(child.id);
          }
        });
        return ids;
      })
    }

    this.authService.AssignRoleToPages(model).subscribe((data: any) => {
      if (data.isSuccess) {
        this.messageService.add({ severity: 'success', summary: 'نجاح', detail: data.message });
      } else {
        this.messageService.add({ severity: 'error', summary: 'خطأ', detail: data.message });
      }
    });
  }
}
