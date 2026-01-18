import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NtsImageComponent } from './nts-image.component';

describe('NtsTextMoreComponent', () => {
  let component: NtsImageComponent;
  let fixture: ComponentFixture<NtsImageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NtsImageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NtsImageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
