import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExchangePremssionComponent } from './exchange-premssion.component';

describe('ExchangePremssionComponent', () => {
  let component: ExchangePremssionComponent;
  let fixture: ComponentFixture<ExchangePremssionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ExchangePremssionComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ExchangePremssionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
