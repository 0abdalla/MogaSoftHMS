import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StaffLevelsComponent } from './staff-levels.component';

describe('StaffLevelsComponent', () => {
  let component: StaffLevelsComponent;
  let fixture: ComponentFixture<StaffLevelsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StaffLevelsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(StaffLevelsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
