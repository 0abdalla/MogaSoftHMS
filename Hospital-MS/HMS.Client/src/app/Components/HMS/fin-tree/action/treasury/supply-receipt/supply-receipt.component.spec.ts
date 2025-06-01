import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SupplyReceiptComponent } from './supply-receipt.component';

describe('SupplyReceiptComponent', () => {
  let component: SupplyReceiptComponent;
  let fixture: ComponentFixture<SupplyReceiptComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SupplyReceiptComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SupplyReceiptComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
