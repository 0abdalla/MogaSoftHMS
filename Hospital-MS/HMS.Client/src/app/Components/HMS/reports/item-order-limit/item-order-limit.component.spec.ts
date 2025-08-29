import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ItemOrderLimitComponent } from './item-order-limit.component';

describe('ItemOrderLimitComponent', () => {
  let component: ItemOrderLimitComponent;
  let fixture: ComponentFixture<ItemOrderLimitComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ItemOrderLimitComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ItemOrderLimitComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
