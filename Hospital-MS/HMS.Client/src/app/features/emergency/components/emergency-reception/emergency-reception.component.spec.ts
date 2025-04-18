import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmergencyReceptionComponent } from './emergency-reception.component';

describe('EmergencyReceptionComponent', () => {
  let component: EmergencyReceptionComponent;
  let fixture: ComponentFixture<EmergencyReceptionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [EmergencyReceptionComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(EmergencyReceptionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
