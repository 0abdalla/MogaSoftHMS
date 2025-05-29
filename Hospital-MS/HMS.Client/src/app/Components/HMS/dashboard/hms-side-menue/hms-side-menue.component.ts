import { Component, EventEmitter, Output } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import Swal from 'sweetalert2';
import { filter } from 'rxjs';
import { AuthService } from '../../../../Auth/auth.service';
import { AppsService } from '../../../../Services/Permissions/apps.service';

@Component({
  selector: 'app-hms-side-menue',
  templateUrl: './hms-side-menue.component.html',
  styleUrl: './hms-side-menue.component.css'
})
export class HMSSideMenueComponent {
  Pages: any[] = [];
  isCollapsed: boolean = false;
  activeMenu: string | null = null;
  activeSubmenuRoute: string = '';
  // 
  activeSubMenu: string = '';
  // 
  permissions: { [key: string]: boolean } = {};
  RoleName: string;

  @Output() sidebarToggled = new EventEmitter<boolean>();

  constructor(private router: Router, private authService: AuthService, private permissionService: AppsService) { }

  ngOnInit() {
    this.RoleName = sessionStorage.getItem('role');
    this.GetAllowedPagesByRoleName();
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {
        this.setActiveMenuBasedOnRoute();
      });

    this.setActiveMenuBasedOnRoute();
    this.permissionService.permissions$.subscribe(permissions => {
      this.permissions = permissions;
    });
  }

  GetAllowedPagesByRoleName() {
    this.authService.GetAllowedPagesByRoleName(this.RoleName).subscribe((data: any) => {
      this.Pages = data;
      this.Pages.forEach(i => {
        if (i.children.length == 0)
          i.isGroup = false;
        else
          i.isGroup = true;
      });
    });
  }

  setActiveMenuBasedOnRoute() {
    const currentRoute = this.router.url;
    for (const item of this.Pages) {
      if (item.isGroup && item.children) {
        for (const child of item.children) {
          if (currentRoute.startsWith(child.route)) {
            this.activeMenu = item.nameAR;
            this.activeSubmenuRoute = currentRoute;
            return;
          }
        }
      } else if (!item.isGroup && currentRoute.startsWith(item.route)) {
        this.activeMenu = item.nameAR;
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

  toggleSubSubMenu(subMenu: string) {
    this.activeSubMenu = this.activeSubMenu === subMenu ? '' : subMenu;
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
