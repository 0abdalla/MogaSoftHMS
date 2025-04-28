import { Component } from '@angular/core';

@Component({
  selector: 'app-hms-layout',
  templateUrl: './hms-layout.component.html',
  styleUrl: './hms-layout.component.css'
})
export class HmsLayoutComponent {
  isSidebarCollapsed: boolean = false;

  onSidebarToggled(isCollapsed: boolean) {
    debugger;
    this.isSidebarCollapsed = isCollapsed;
  }

  constructor() {
   
  }
}
