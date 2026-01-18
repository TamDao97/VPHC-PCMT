import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { UserService } from '../../service/user.service';
import { TranslateService } from '@ngx-translate/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TreeGridComponent } from '@syncfusion/ej2-angular-treegrid';
import { PermissionUpdateComponent } from '../permission-update/permission-update.component';
import { Constants } from 'src/app/cores/shared/common/constants';
import { MessageService } from 'src/app/cores/shared/services/message.service';
import { LanguageService } from 'src/app/cores/shared/services/language.service';

@Component({
  selector: 'app-permission-manage',
  templateUrl: './permission-manage.component.html',
  styleUrls: ['./permission-manage.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class PermissionManageComponent implements OnInit {

  constructor(
    public constant: Constants,
    private userService: UserService,
    private messageService: MessageService,
    private translate: TranslateService,
    private lgService: LanguageService,
    private modalService: NgbModal,
  ) {
    this._unsubscribeAll = new Subject();
    this.translate.use(this.lgService.getLanguage());
  }
  height = 0;
  @ViewChild('treegrid')
  public treegrid: TreeGridComponent;
  _unsubscribeAll: Subject<any>;

  groupFunctions: any[] = []; 
  groupSelect: any = {};
  groupSelectIndex: number = 0;
  listPermission: any[] = [];

  ngOnInit(): void {
    //Hứng sự kiện thay đổi ngôn ngữ để load lại Component
    this.lgService.onLanguageChanged.pipe().subscribe(languageCode => {
      if (languageCode) {
        this.translate.use(languageCode);
      }
    });
    this.height = window.innerHeight - 620;
    this.getPermission();
  }
  getPermission() {
    this.userService.getPermission('').subscribe(
      result => {
        if (result.isStatus) {
          setTimeout(() => {
            this.groupFunctions = result.data;
          }, 200);
         
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }
  rowGroupSelected($event: any) {
    this.groupSelectIndex = $event.rowIndex;
    this.groupSelect = $event.data;
    //kiểm tra nếu collapsable thì set  Permission = []
    if (this.groupSelect.children == 0) {
      this.listPermission = $event.data.permissions;
    } else {
      this.listPermission = [];
    }    
  }  
 
  viewUserPermission(code: string, namePermission: string){
    let activeModal = this.modalService.open(PermissionUpdateComponent, { container: 'body', windowClass: 'permission-update', backdrop: 'static' })
    activeModal.componentInstance.code = code;
    activeModal.componentInstance.namePermission = namePermission;
    activeModal.result.then((result) => {
      if (result) {
        
      }
    }, (reason) => {
    });
  }
}
