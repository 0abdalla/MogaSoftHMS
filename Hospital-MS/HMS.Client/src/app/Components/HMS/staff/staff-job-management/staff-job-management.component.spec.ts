import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StaffJobManagementComponent } from './staff-job-management.component';

describe('StaffJobManagementComponent', () => {
  let component: StaffJobManagementComponent;
  let fixture: ComponentFixture<StaffJobManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StaffJobManagementComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(StaffJobManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
