import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DoctorsSettingsEditComponent } from './doctors-settings-edit.component';

describe('DoctorsSettingsEditComponent', () => {
  let component: DoctorsSettingsEditComponent;
  let fixture: ComponentFixture<DoctorsSettingsEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DoctorsSettingsEditComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DoctorsSettingsEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
