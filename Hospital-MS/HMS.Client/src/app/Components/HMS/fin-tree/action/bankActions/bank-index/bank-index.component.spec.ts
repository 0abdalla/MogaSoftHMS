import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BankIndexComponent } from './bank-index.component';

describe('BankIndexComponent', () => {
  let component: BankIndexComponent;
  let fixture: ComponentFixture<BankIndexComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BankIndexComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(BankIndexComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
