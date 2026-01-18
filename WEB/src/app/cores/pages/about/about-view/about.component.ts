import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Constants } from 'src/app/cores/shared/common/constants';
import { MessageService } from 'src/app/cores/shared/services/message.service';
import { AboutService } from '../service/about.service';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class AboutComponent implements OnInit {

  constructor(
    public constant: Constants,
    private aboutService: AboutService,
    private messageService: MessageService
  ) { }

  height: number;
  model: any = {
    content: ''
  }
  ngOnInit(): void {
    this.getAbout();
  }

  getAbout() {
    this.aboutService.getAbout().subscribe(
      data => {
        if (data.isStatus) {
          this.model = data.data;
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }
}
