import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StaffProgressionManagmentComponent } from './staff-progression-managment.component';

describe('StaffProgressionManagmentComponent', () => {
  let component: StaffProgressionManagmentComponent;
  let fixture: ComponentFixture<StaffProgressionManagmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StaffProgressionManagmentComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(StaffProgressionManagmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
