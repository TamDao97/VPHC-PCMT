import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { MenuService } from '../service/menu.service';
import { RowDDService, SelectionService } from '@syncfusion/ej2-angular-treegrid';
import { Constants } from 'src/app/cores/shared/common/constants';
import { ComboboxCoreService } from 'src/app/cores/shared/services/combobox-core.service';
import { LanguageService } from 'src/app/cores/shared/services/language.service';
import { MessageService } from 'src/app/cores/shared/services/message.service';
//import { Guid } from "guid-typescript";

@Component({
  selector: 'app-permission-create',
  templateUrl: './permission-create.component.html',
  styleUrls: ['./permission-create.component.scss'],
  encapsulation: ViewEncapsulation.None,
  providers: [RowDDService, SelectionService]
})
export class PermissionCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public constant: Constants,
    private translate: TranslateService,
    private lgService: LanguageService,
    private comboboxService: ComboboxCoreService,
    private menuService: MenuService,
  ) { this.translate.use(this.lgService.getLanguage()); }

  modalInfo = {
    Title: 'Chọn quyền'
  };
  listPermissionChoose: any = [];
  isSaveContinue: boolean = false;
  isUpdate: boolean = false;
  model: any = {
    id: '',
    name: '',
    code: ''
  };

  ngOnInit(): void {
    //Hứng sự kiện thay đổi ngôn ngữ để load lại Component
    this.lgService.onLanguageChanged.pipe().subscribe(languageCode => {
      if (languageCode) {
        this.translate.use(languageCode);
      }
    });

    if (!this.isUpdate) {
      //this.model.id = Guid.create().toString();
    }
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK || this.isSaveContinue ? true : false);
  }

  save(isContinue: boolean = false) {
    var permissionExit = this.listPermissionChoose.filter((a:any) => a.id != this.model.id && a.code == this.model.code);
    if (permissionExit.length > 0) {
      this.messageService.showWarning("Mã quyền " + this.model.code + " đã tồn tại!");
      return;
    }

    if (isContinue) {
      this.listPermissionChoose.push(this.model);
      this.model = {
        id: '',
        name: '',
        code: ''
      };
      this.isSaveContinue = true;
    }
    else
      this.closeModal(true);
  }
}
