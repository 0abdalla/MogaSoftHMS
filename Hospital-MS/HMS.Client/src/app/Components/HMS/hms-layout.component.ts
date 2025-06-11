import { Component, OnInit } from '@angular/core';
import { MenuSidebarItem } from '../../Models/Generics/MenuSidebarItem';
import { MenueService, MenuType } from '../../Services/Generics/menue.service';
import { NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs';
import { trigger, state, style, transition, animate } from '@angular/animations';

@Component({
  selector: 'app-hms-layout',
  templateUrl: './hms-layout.component.html',
  styleUrl: './hms-layout.component.css',
  animations: [
    trigger('expandCollapse', [
      state('collapsed', style({
        height: '0px',
        overflow: 'hidden',
        opacity: 0
      })),
      state('expanded', style({
        height: '*',
        overflow: 'hidden',
        opacity: 1
      })),
      transition('collapsed <=> expanded', animate('300ms ease-in-out'))
    ])
  ]
})
export class HmsLayoutComponent implements OnInit {
  expandedMenuKey: string | null = null;
  menuItem: MenuSidebarItem;

  constructor(private menuService: MenueService, private router: Router) {
    this.menuItem = this.menuService.getMenuById(MenuType.ZAInstitution);
  }

  ngOnInit(): void {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      this.expandParentForCurrentRoute();
    });

    this.expandParentForCurrentRoute();
  }

  toggleSubMenu(parent: MenuSidebarItem): void {
    const key = parent.menuItem;
    this.expandedMenuKey = this.expandedMenuKey === key ? null : key;
  }

  isExpanded(parent: MenuSidebarItem): boolean {
    return this.expandedMenuKey === parent.menuItem;
  }

  expandParentForCurrentRoute(): void {
    const currentUrl = this.router.url;

    for (const parent of this.menuItem.subMenus) {
      if (
        parent.route === currentUrl ||
        parent.subMenus?.some(child => currentUrl.startsWith(child.route))
      ) {
        this.expandedMenuKey = parent.menuItem;
        return;
      }
    }

    this.expandedMenuKey = null;
  }
}