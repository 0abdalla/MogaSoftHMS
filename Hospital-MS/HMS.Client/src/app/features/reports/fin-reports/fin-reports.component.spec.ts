import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FinReportsComponent } from './fin-reports.component';

describe('FinReportsComponent', () => {
  let component: FinReportsComponent;
  let fixture: ComponentFixture<FinReportsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [FinReportsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(FinReportsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
