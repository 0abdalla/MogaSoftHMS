import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AppointmetSettingsComponent } from './appointmet-settings.component';

describe('AppointmetSettingsComponent', () => {
  let component: AppointmetSettingsComponent;
  let fixture: ComponentFixture<AppointmetSettingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AppointmetSettingsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AppointmetSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
