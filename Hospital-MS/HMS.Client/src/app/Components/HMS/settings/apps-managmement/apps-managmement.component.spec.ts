import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AppsManagmementComponent } from './apps-managmement.component';

describe('AppsManagmementComponent', () => {
  let component: AppsManagmementComponent;
  let fixture: ComponentFixture<AppsManagmementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AppsManagmementComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AppsManagmementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
