import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DoctorsDepartmentsListComponent } from './doctors-departments-list.component';

describe('DoctorsDepartmentsListComponent', () => {
  let component: DoctorsDepartmentsListComponent;
  let fixture: ComponentFixture<DoctorsDepartmentsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DoctorsDepartmentsListComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DoctorsDepartmentsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
