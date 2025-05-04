import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StaffClassManagmentComponent } from './staff-class-managment.component';

describe('StaffClassManagmentComponent', () => {
  let component: StaffClassManagmentComponent;
  let fixture: ComponentFixture<StaffClassManagmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StaffClassManagmentComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(StaffClassManagmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
