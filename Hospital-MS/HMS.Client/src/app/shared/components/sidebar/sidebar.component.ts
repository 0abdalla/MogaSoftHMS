import { Component, EventEmitter, Output } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/auth/auth.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent {
  isCollapsed: boolean = false;
  activeMenu: string | null = null;
  activeSubmenuRoute: string = '';

  @Output() sidebarToggled = new EventEmitter<boolean>();

  constructor(private router: Router , private authService : AuthService) { }

  toggleSubMenu(menu: string) {
    this.activeMenu = this.activeMenu === menu ? null : menu;
  }

  toggleSidebar() {
    this.isCollapsed = !this.isCollapsed;
    this.sidebarToggled.emit(this.isCollapsed);
  }

  logout() {
    Swal.fire({
      title: "هل أنت متأكد؟",
      icon: "question",
      showCancelButton: true,
      confirmButtonColor: "#3D5DA7",
      cancelButtonColor: "#ED3B93",
      confirmButtonText: "نعم",
      cancelButtonText: "لا",
    }).then((result) => {
      if (result.isConfirmed) {
        this.authService.logout();
        this.router.navigate(['/login']);
      }
    });
    
  }
}
