import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StoreRateComponent } from './store-rate.component';

describe('StoreRateComponent', () => {
  let component: StoreRateComponent;
  let fixture: ComponentFixture<StoreRateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StoreRateComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(StoreRateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
