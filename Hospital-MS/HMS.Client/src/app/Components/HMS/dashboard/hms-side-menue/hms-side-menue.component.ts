import { Component, EventEmitter, Output } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import Swal from 'sweetalert2';
import { filter } from 'rxjs';
import { AuthService } from '../../../../Auth/auth.service';
import { AppsService } from '../../../../Services/Permissions/apps.service';
import { MenueService } from '../../../../Services/menue.service';
import { MenuSidebarItem } from '../../../../Models/Generics/MenuSidebarItem';

@Component({
  selector: 'app-hms-side-menue',
  templateUrl: './hms-side-menue.component.html',
  styleUrl: './hms-side-menue.component.css'
})
export class HMSSideMenueComponent {
  menusList: MenuSidebarItem[] = [];
  isCollapsed: boolean = false;
  activeMenu: string | null = null;
  activeChildMenu: string | null = null;
  activeSubMenu: string = '';
  activeRoute: string = '';
  permissions: { [key: string]: boolean } = {};
  RoleName: string;

  @Output() sidebarToggled = new EventEmitter<boolean>();

  constructor(
    private router: Router,
    private authService: AuthService,
    private permissionService: AppsService,
    private menuService: MenueService
  ) {
    this.menusList = this.menuService.menus;
  }

  ngOnInit() {
    this.RoleName = sessionStorage.getItem('role');

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

  setActiveMenuBasedOnRoute() {
  const currentRoute = this.router.url;
  console.log('Current Route:', currentRoute);

  for (const main of this.menusList) {
    if (main.subMenus) {
      for (const child of main.subMenus) {
        if (child.subMenus) {
          for (const subChild of child.subMenus) {
            console.log('Checking:', subChild.route);
            if (currentRoute.startsWith(subChild.route)) {
              this.activeMenu = main.displayName;
              this.activeChildMenu = child.displayName;
              return;
            }
          }
        }

        console.log('Checking:', child.route);
        if (currentRoute.startsWith(child.route)) {
          this.activeMenu = main.displayName;
          this.activeChildMenu = child.displayName;
          return;
        }
      }
    }

    console.log('Checking:', main.route);
    if (currentRoute.startsWith(main.route)) {
      this.activeMenu = main.displayName;
      this.activeChildMenu = null;
      return;
    }
  }

  this.activeMenu = null;
  this.activeChildMenu = null;
}




  toggleMainMenu(menu: string) {
    this.activeMenu = this.activeMenu === menu ? null : menu;
    this.activeChildMenu = null;
  }

  toggleChildMenu(childMenu: string) {
    this.activeChildMenu = this.activeChildMenu === childMenu ? null : childMenu;
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
