import { Component, OnInit } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { LanguageService } from '../../services/language.service';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.css']
})
export class MessageComponent implements OnInit {

  message: string = '';
  exception: string = '';
  messages: string[] = [];
  isList: boolean = false;

  constructor(private activeModal: NgbActiveModal,
    public translate: TranslateService,
    public lgService: LanguageService) {this.translate.use(this.lgService.getLanguage()); }

  ngOnInit() {
    this.isList = this.messages && this.messages.length > 0 ? true : false;
  }

  closeModal() {
    this.activeModal.close();
  }
}
