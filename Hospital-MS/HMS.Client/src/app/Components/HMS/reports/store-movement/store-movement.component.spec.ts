import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StoreMovementComponent } from './store-movement.component';

describe('StoreMovementComponent', () => {
  let component: StoreMovementComponent;
  let fixture: ComponentFixture<StoreMovementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StoreMovementComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(StoreMovementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
