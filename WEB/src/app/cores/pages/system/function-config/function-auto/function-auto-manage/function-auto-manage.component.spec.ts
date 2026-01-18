import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupUserManageComponent } from './function-auto-manage.component';

describe('GroupUserManageComponent', () => {
  let component: GroupUserManageComponent;
  let fixture: ComponentFixture<GroupUserManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GroupUserManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GroupUserManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
