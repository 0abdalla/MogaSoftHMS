import { Component } from '@angular/core';
import { AppsService } from '../../../../Services/Permissions/apps.service';

@Component({
  selector: 'app-apps-managmement',
  templateUrl: './apps-managmement.component.html',
  styleUrl: './apps-managmement.component.css'
})
export class AppsManagmementComponent {
  permissions: { [key: string]: boolean } = {};

  constructor(private permissionService: AppsService) {}

  ngOnInit(): void {
    this.permissionService.permissions$.subscribe(permissions => {
      this.permissions = permissions;
    });
  }

  updatePermission(key: string): void {
    this.permissionService.updatePermission(key, this.permissions[key]);
  }
}
