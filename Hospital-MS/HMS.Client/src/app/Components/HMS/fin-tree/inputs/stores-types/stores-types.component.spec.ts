import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StoresTypesComponent } from './stores-types.component';

describe('StoresTypesComponent', () => {
  let component: StoresTypesComponent;
  let fixture: ComponentFixture<StoresTypesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StoresTypesComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(StoresTypesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
