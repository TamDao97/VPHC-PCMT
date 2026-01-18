import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-nts-status-badge',
  templateUrl: './nts-status-badge.component.html',
  styleUrls: ['./nts-status-badge.component.scss']
})
export class NtsStatusBadgeComponent implements OnInit {

  constructor() { }

  badgeClass = '';
  statusName = '';
  private _items: any[] = [];
  private _value: any[] = [];
  private _displayText: any = true;

  @Input()
  get items() { return this._items };
  set items(item: any[]) {
    this._items = item;
    this.viewBadge();
  };

  @Input()
  get value() { return this._value };
  set value(value: any) {
    this._value = value;
    this.viewBadge();
  };

  @Input()
  get displayText() { return this._displayText };
  set displayText(value: any) {
    this._displayText = value;
    this.viewBadge();
  };

  ngOnInit(): void {
    //this.viewBadge();
  }

  viewBadge() {
    this._items.forEach(item => {
      if (item.Id == this._value || item.Name == this._value) {
        if (this._displayText == true) {
          this.statusName = item.Name;
        }
        this.badgeClass = item.BadgeClass;
      }
    });
  }

}
