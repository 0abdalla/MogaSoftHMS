import { ComponentFixture, TestBed } from '@angular/core/testing';

import { YearFinSettingsComponent } from './year-fin-settings.component';

describe('YearFinSettingsComponent', () => {
  let component: YearFinSettingsComponent;
  let fixture: ComponentFixture<YearFinSettingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [YearFinSettingsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(YearFinSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
