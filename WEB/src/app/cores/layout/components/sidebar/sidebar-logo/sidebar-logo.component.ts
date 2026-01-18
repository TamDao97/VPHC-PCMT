import { Component, Input, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { Subscription } from 'rxjs';
import { LayoutType } from '../../../core/configs/config';
import { LayoutService } from '../../../core/layout.service';

@Component({
  selector: 'app-sidebar-logo',
  templateUrl: './sidebar-logo.component.html',
  styleUrls: ['./sidebar-logo.component.scss'],
   encapsulation: ViewEncapsulation.None,
})
export class SidebarLogoComponent implements OnInit, OnDestroy {
  private unsubscribe: Subscription[] = [];
  @Input() toggleButtonClass: string = '';
  @Input() toggleEnabled: boolean;
  @Input() toggleType: string = '';
  @Input() toggleState: string = '';
  currentLayoutType: LayoutType | null;

  toggleAttr: string;

  constructor(private layout: LayoutService) {}

  ngOnInit(): void {
    this.toggleAttr = `app-sidebar-${this.toggleType}`;
    const layoutSubscr = this.layout.currentLayoutTypeSubject
      .asObservable()
      .subscribe((layout) => {
        this.currentLayoutType = layout;
      });
    this.unsubscribe.push(layoutSubscr);
  }

  ngOnDestroy() {
    this.unsubscribe.forEach((sb) => sb.unsubscribe());
  }
}
