import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NtsHorizontalStepperComponent } from './nts-horizontal-stepper.component';

describe('NtsHorizontalStepperComponent', () => {
  let component: NtsHorizontalStepperComponent;
  let fixture: ComponentFixture<NtsHorizontalStepperComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NtsHorizontalStepperComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NtsHorizontalStepperComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
