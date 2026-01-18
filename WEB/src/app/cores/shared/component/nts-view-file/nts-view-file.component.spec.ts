import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NtsViewFileComponent } from './nts-view-file.component';

describe('NtsViewFileComponent', () => {
  let component: NtsViewFileComponent;
  let fixture: ComponentFixture<NtsViewFileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NtsViewFileComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NtsViewFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
