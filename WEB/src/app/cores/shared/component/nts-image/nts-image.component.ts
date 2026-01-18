import { Component, OnInit, Input, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'app-nts-image',
  templateUrl: './nts-image.component.html',
  styleUrls: ['./nts-image.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class NtsImageComponent implements OnInit {

  @Input()
  set ntsSrc(value: string) {
    this.imgErrorPath = "";
    this._ntsSrc = value;
  }

  @Input()
  set ntsBorder(value: string) {
    this._ntsBorderTop = value + 'px solid rgba(0, 0, 0, 0.125)';
    this._ntsBorderRight = value + 'px solid rgba(0, 0, 0, 0.125)';
    this._ntsBorderBottom = value + 'px solid rgba(0, 0, 0, 0.125)';
    this._ntsBorderLeft = value + 'px solid rgba(0, 0, 0, 0.125)';
  }

  @Input()
  set ntsBorderTop(value: string) {
    this._ntsBorderTop = value + 'px solid rgba(0, 0, 0, 0.125)';
  }

  @Input()
  set ntsBorderRight(value: string) {
    this._ntsBorderRight = value + 'px solid rgba(0, 0, 0, 0.125)';
  }

  @Input()
  set ntsBorderBottom(value: string) {
    this._ntsBorderBottom = value + 'px solid rgba(0, 0, 0, 0.125)';
  }

  @Input()
  set ntsBorderLeft(value: string) {
    this._ntsBorderLeft = value + 'px solid rgba(0, 0, 0, 0.125)';
  }

  @Input()
  set ntsHeight(value: string) {
    this._ntsHeight = value + 'px';
  }

  @Input()
  set ntsHorizontal(value: string) {
    this._ntsHorizontal = value == 'left' ? 'start' : value == 'right' ? 'end' : 'center';
  }

  @Input()
  set ntsVertical(value: string) {
    this._ntsVertical = value == 'top' ? 'start' : value == 'bottom' ? 'end' : 'center';
  }

  @Input()
  set ntsRadius(value: string) {
    this._ntsRadius = value + 'px';
  }

  _ntsSrc: string;
  _ntsBorderTop: string;
  _ntsBorderRight: string;
  _ntsBorderBottom: string;
  _ntsBorderLeft: string;
  _ntsHeight: string;
  _ntsHorizontal: string = 'center';
  _ntsVertical: string = 'center';
  _ntsRadius: string;
  imgErrorPath: string;

  constructor() { }

  ngOnInit(): void {

  }

  errorImage(event:any) {
    this.imgErrorPath = './assets/img/noimage.png';
  }
}
