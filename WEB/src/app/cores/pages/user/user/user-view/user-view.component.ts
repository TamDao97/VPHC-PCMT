import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { UserService } from '../../service/user.service';
import { TranslateService } from '@ngx-translate/core';
import { TreeGridComponent } from '@syncfusion/ej2-angular-treegrid';
import { FileProcess } from 'src/app/cores/shared/common/file-process';
import { MessageService } from 'src/app/cores/shared/services/message.service';
import { Constants } from 'src/app/cores/shared/common/constants';
import { DateUtils } from 'src/app/cores/shared/common/date-utils';
import { LanguageService } from 'src/app/cores/shared/services/language.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-user-view',
  templateUrl: './user-view.component.html',
  styleUrls: ['./user-view.component.scss'],
})
export class UserViewComponent implements OnInit {
  @ViewChild('grid')
  public grid!: TreeGridComponent;
  constructor(
    public fileProcess: FileProcess,
    private routeA: ActivatedRoute,
    private messageService: MessageService,
    public constant: Constants,
    public dateUtils: DateUtils,
    private userService: UserService,
    private translate: TranslateService,
    private lgService: LanguageService
  ) {
    this.translate.use(this.lgService.getLanguage());
  }

  height = 0;
  @ViewChild('scrollPracticeMaterial') scrollPracticeMaterial: ElementRef;
  @ViewChild('scrollPracticeMaterialHeader')
  scrollPracticeMaterialHeader: ElementRef;

  @ViewChild('scrollPermession', { static: false })
  scrollPermession: ElementRef;
  @ViewChild('scrollPermessionHeader', { static: false })
  scrollPermessionHeader: ElementRef;
  id: string;
  type: string;
  filedata: any = null;
  minDateNotificationV: NgbDateStruct;
  listFunction: any[] = [];
  listPermission: any[] = [];
  listUserGroup: any[] = [];
  groupFunctions: any[] = [];

  isSelectAll = false;
  listFunctionIndex = 0;
  model: any = {
    userName: '',
    fullName: '',
    email: '',
    phoneNumber: '',
    imageLink: '',
    password: '',
    lockoutEnabled: 'false',
    description: '',
    isChecked: false,
    nameGroupUser: '',
  };
  isIndeterminate = false;
  groupSelectIndex = -1;

  ngOnInit(): void {
    //Hứng sự kiện thay đổi ngôn ngữ để load lại Component
    this.lgService.onLanguageChanged.pipe().subscribe((languageCode) => {
      if (languageCode) {
        this.translate.use(languageCode);
      }
    });

    this.fileProcess.fileModel = {};
    this.fileProcess.FileDataBase = null;
    this.id = this.routeA.snapshot.paramMap.get('id') ?? '';
    this.height = window.innerHeight - 450;
    if (this.id != null) {
      // this.appSetting.PageTitle = "Xem thông tin tài khoản";
      this.getUserById();
    }
  }
  getUserById() {
    this.userService.getUserById(this.id).subscribe(
      (data) => {
        if (data.isStatus) {
          this.model = data.data;
          this.model.lockoutEnabled = this.model.lockoutEnabled.toString();
          this.groupFunctions = data.data.listGroupFunction;
          if (this.model.avatar != null && this.model.avatar != '') {
            this.filedata = environment.apiUrl + this.model.avatar;
          }
        }
      },
      (error) => {
        this.messageService.showError(error);
      }
    );
  }
  rowSelected($event: any) {
    this.listPermission = $event.data.permissions;
  }
  changeGroupFunctionCheck(group: any, index: number) {
    group.permissions.forEach((permission: any) => {
      if (permission.isChecked && !group.isChecked) {
        group.checkCount--;
      }
      if (!permission.isChecked && group.isChecked) {
        group.checkCount++;
      }
      permission.isChecked = group.isChecked;
    });

    if (index == this.groupSelectIndex) {
      this.isSelectAll = group.isChecked;
    }
    this.groupFunctions[index].isIndeterminate = false;
  }
}
