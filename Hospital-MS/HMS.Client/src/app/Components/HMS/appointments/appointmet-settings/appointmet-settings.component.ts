import { Component } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-appointmet-settings',
  templateUrl: './appointmet-settings.component.html',
  styleUrl: './appointmet-settings.component.css'
})
export class AppointmetSettingsComponent {
  settingsForm!: FormGroup;

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.settingsForm = this.fb.group({
      slotInterval: [15],
      daysAllowed: [7],
      onlineBookingEnabled: [true]
    });
  }

  saveSettings() {
    const settings = this.settingsForm.value;
  }

}
