import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MedicalServicesListComponent } from './medical-services-list.component';

describe('MedicalServicesListComponent', () => {
  let component: MedicalServicesListComponent;
  let fixture: ComponentFixture<MedicalServicesListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [MedicalServicesListComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(MedicalServicesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
