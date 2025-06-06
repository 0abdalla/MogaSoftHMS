import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  @Input() showToggler: boolean = true;
  @Output() toggler = new EventEmitter<boolean>();
  collapsed = true;
  showMenu: boolean = false;
  systemUrl: string; //environment.systemUrl;
  UserModel: any;
  selectedModuleName: string = 'الأنظمة';
  modulesMenu: any[] = [];
  constructor() {
    // this.modulesMenu = this.menuService.getMenuById(MenuType.MainModules)?.subMenus;
    // this.UserModel = this.authService.getCurrentUser();
    // this.routerSubscriber();
  }

  ngOnInit(): void {
  }

  routerSubscriber() {
    // this.setSelectedModule(this.router.url);

    // this.router.events.pipe(filter(event => event instanceof NavigationStart)).subscribe((event: NavigationStart) => {
    //   this.setSelectedModule(event.url);
    // });
  }
  setSelectedModule(url: string) {
    var selectedModule = url.split('/') ? url.split('/')[1] : '';
    if (selectedModule) {
      this.selectedModuleName = this.modulesMenu.find(x => x.route == `/${selectedModule}`)?.displayName;
    }
  }
  onToggler() {
    this.showMenu = !this.showMenu;
    console.log(this.toggler);
  }
  logout() {
    // this.authService.logout();
  }
}