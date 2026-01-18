import { Component, OnInit, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'app-nts-switch-language',
  templateUrl: './switch-language.component.html',
  styleUrls: ['./switch-language.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SwitchLanguageComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  language:any = 'vn';

  changeLanguage() {
    // this.appSetting.Language = this.appSetting.Language == 'vn' ? 'en' : 'vn';
  }
}
