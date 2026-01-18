import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SidebarDepartmentComponent } from './sidebar-department.component';

describe('SidebarDepartmentComponent', () => {
  let component: SidebarDepartmentComponent;
  let fixture: ComponentFixture<SidebarDepartmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SidebarDepartmentComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SidebarDepartmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
