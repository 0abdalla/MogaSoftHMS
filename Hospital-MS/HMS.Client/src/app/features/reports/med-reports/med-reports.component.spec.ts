import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MedReportsComponent } from './med-reports.component';

describe('MedReportsComponent', () => {
  let component: MedReportsComponent;
  let fixture: ComponentFixture<MedReportsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [MedReportsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(MedReportsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
