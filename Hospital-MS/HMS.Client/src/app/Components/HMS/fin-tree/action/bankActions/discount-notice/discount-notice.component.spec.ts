import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DiscountNoticeComponent } from './discount-notice.component';

describe('DiscountNoticeComponent', () => {
  let component: DiscountNoticeComponent;
  let fixture: ComponentFixture<DiscountNoticeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DiscountNoticeComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DiscountNoticeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
