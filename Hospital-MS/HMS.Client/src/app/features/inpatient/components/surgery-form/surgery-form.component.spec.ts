import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SurgeryFormComponent } from './surgery-form.component';

describe('SurgeryFormComponent', () => {
  let component: SurgeryFormComponent;
  let fixture: ComponentFixture<SurgeryFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SurgeryFormComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SurgeryFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
