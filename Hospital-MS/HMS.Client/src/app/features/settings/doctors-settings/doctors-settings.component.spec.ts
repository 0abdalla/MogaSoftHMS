import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DoctorsSettingsComponent } from './doctors-settings.component';

describe('DoctorsSettingsComponent', () => {
  let component: DoctorsSettingsComponent;
  let fixture: ComponentFixture<DoctorsSettingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DoctorsSettingsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DoctorsSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
