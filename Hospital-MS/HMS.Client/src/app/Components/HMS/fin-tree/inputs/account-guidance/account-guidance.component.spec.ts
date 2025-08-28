import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountGuidanceComponent } from './account-guidance.component';

describe('AccountGuidanceComponent', () => {
  let component: AccountGuidanceComponent;
  let fixture: ComponentFixture<AccountGuidanceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AccountGuidanceComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AccountGuidanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
