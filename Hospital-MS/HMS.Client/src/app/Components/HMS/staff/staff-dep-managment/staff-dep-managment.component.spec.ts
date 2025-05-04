import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StaffDepManagmentComponent } from './staff-dep-managment.component';

describe('StaffDepManagmentComponent', () => {
  let component: StaffDepManagmentComponent;
  let fixture: ComponentFixture<StaffDepManagmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StaffDepManagmentComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(StaffDepManagmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
