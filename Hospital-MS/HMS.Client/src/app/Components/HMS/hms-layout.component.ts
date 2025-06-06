import { Component } from '@angular/core';
import { MenuSidebarItem } from '../../Models/Generics/MenuSidebarItem';
import { MenueService, MenuType } from '../../Services/Generics/menue.service';

@Component({
  selector: 'app-hms-layout',
  templateUrl: './hms-layout.component.html',
  styleUrl: './hms-layout.component.css'
})
export class HmsLayoutComponent {
isToggle = false;
  toggler = false;
  menuItem: MenuSidebarItem;

  constructor(private menuService: MenueService) {
    this.menuItem = this.menuService.getMenuById(MenuType.ZAInstitution);
  }

  onToggleContent() {
    this.isToggle = !this.isToggle;
    const htmlElement = document.querySelector('html');
    if (this.isToggle) {
      htmlElement.style.cssText = `overflow: hidden`;
    } else {
      htmlElement.style.cssText = `overflow: auto`;
    }
  }

  onOverlayClicked() {
    this.isToggle = false;
    const htmlElement = document.querySelector('html');
    htmlElement.style.cssText = `overflow: auto`;
  }
  
  onToggler() {
    this.toggler = !this.toggler;
  }
  
  toggleMenu(menu: HTMLElement) {
    menu.classList.toggle('show');
  }
}
