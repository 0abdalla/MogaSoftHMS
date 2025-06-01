import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TreasuryIndexComponent } from './treasury-index.component';

describe('TreasuryIndexComponent', () => {
  let component: TreasuryIndexComponent;
  let fixture: ComponentFixture<TreasuryIndexComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TreasuryIndexComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TreasuryIndexComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
