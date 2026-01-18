import {
  Component,
  Input,
  OnDestroy,
  OnInit,
  ViewEncapsulation,
} from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { LayoutType } from '../../../core/configs/config';
import { LayoutService } from '../../../core/layout.service';
import { AuthService, UserType } from 'src/app/modules/auth';

@Component({
  selector: 'app-sidebar-department',
  templateUrl: './sidebar-department.component.html',
  styleUrls: ['./sidebar-department.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class SidebarDepartmentComponent implements OnInit, OnDestroy {
  private unsubscribe: Subscription[] = [];
  @Input() toggleButtonClass: string = '';
  @Input() toggleEnabled: boolean;
  @Input() toggleType: string = '';
  @Input() toggleState: string = '';
  currentLayoutType: LayoutType | null;

  toggleAttr: string;

  constructor(private layout: LayoutService, private auth: AuthService) {}
  user$: Observable<UserType>;
  ngOnInit(): void {
    this.toggleAttr = `app-sidebar-${this.toggleType}`;
    const layoutSubscr = this.layout.currentLayoutTypeSubject
      .asObservable()
      .subscribe((layout) => {
        this.currentLayoutType = layout;
      });
    this.unsubscribe.push(layoutSubscr);

    this.user$ = this.auth.currentUserSubject.asObservable();
  }

  ngOnDestroy() {
    this.unsubscribe.forEach((sb) => sb.unsubscribe());
  }
}
