import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TreasuriesComponent } from './treasuries.component';

describe('TreasuriesComponent', () => {
  let component: TreasuriesComponent;
  let fixture: ComponentFixture<TreasuriesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TreasuriesComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TreasuriesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
