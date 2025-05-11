import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AppsService {
  private permissionsSubject = new BehaviorSubject<{ [key: string]: boolean }>(this.loadPermissions());
  permissions$: Observable<{ [key: string]: boolean }> = this.permissionsSubject.asObservable();

  private defaultPermissions: { [key: string]: boolean } = {
    patients_list: true,
    patients_add: true,
    appointments_list: true,
    appointments_add: true,
    appointments_settings: true,
    emergency_reception: true,
    insurance_list: true,
    insurance_add: true,
    finance_accounts: true,
    finance_transactions: true,
    inventory_stock: true,
    inventory_purchases: true,
    staff_list: true,
    staff_add: true,
    staff_department: true,
    staff_job_levels: true,
    staff_classification: true,
    staff_job_management: true,
    reports_financial: true,
    reports_medical: true,
    settings_general: true,
    settings_doctors_list: true,
    settings_doctors: true,
    settings_medical_services: true,
    settings_permissions: true,
    settings_apps_management: true
  };

  constructor() {
    const savedPermissions = this.loadPermissions();
    this.permissionsSubject.next(savedPermissions);
  }

  private loadPermissions(): { [key: string]: boolean } {
    const saved = localStorage.getItem('permissions');
    if (saved) {
      return { ...this.defaultPermissions, ...JSON.parse(saved) };
    }
    return { ...this.defaultPermissions };
  }

  private savePermissions(permissions: { [key: string]: boolean }): void {
    localStorage.setItem('permissions', JSON.stringify(permissions));
  }

  getPermissions(): { [key: string]: boolean } {
    return this.permissionsSubject.getValue();
  }

  updatePermission(key: string, value: boolean): void {
    const currentPermissions = this.permissionsSubject.getValue();
    currentPermissions[key] = value;
    this.permissionsSubject.next(currentPermissions);
    this.savePermissions(currentPermissions);
  }

  resetPermissions(): void {
    this.permissionsSubject.next({ ...this.defaultPermissions });
    this.savePermissions(this.defaultPermissions);
  }
}
