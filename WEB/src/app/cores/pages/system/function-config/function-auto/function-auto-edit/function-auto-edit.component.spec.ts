import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FunctionAutoEditComponent } from './function-auto-edit.component';

describe('FunctionAutoEditComponent', () => {
  let component: FunctionAutoEditComponent;
  let fixture: ComponentFixture<FunctionAutoEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FunctionAutoEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FunctionAutoEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
