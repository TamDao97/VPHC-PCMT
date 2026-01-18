import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { RowDDService, SelectionService } from '@syncfusion/ej2-angular-treegrid';
import { MenuService } from '../service/menu.service';
import { Constants } from 'src/app/cores/shared/common/constants';
import { ComboboxCoreService } from 'src/app/cores/shared/services/combobox-core.service';
import { LanguageService } from 'src/app/cores/shared/services/language.service';
import { MessageService } from 'src/app/cores/shared/services/message.service';

@Component({
  selector: 'app-choose-permission-auto',
  templateUrl: './choose-permission-auto.component.html',
  styleUrls: ['./choose-permission-auto.component.scss'],
  encapsulation: ViewEncapsulation.None,
  providers: [RowDDService, SelectionService]
})
export class ChoosePermissionAutoComponent implements OnInit {

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
  isAction: boolean = false;
  checkAll: boolean = false;
  listPermissionChoose: any[] = [];
  listPermissionAuto: any[] = [];

  ngOnInit(): void {
    //Hứng sự kiện thay đổi ngôn ngữ để load lại Component
    this.lgService.onLanguageChanged.pipe().subscribe(languageCode => {
      if (languageCode) {
        this.translate.use(languageCode);
      }
    });
    this.getListOder();
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  save() {
    this.listPermissionChoose = this.listPermissionAuto.filter(a => a.isCheck);
    this.closeModal(true);
  }

  getListOder() {
    this.menuService.getFuntionAuto().subscribe(
      result => {
        if (result.isStatus) {
          this.listPermissionAuto = result.data;
          if (this.listPermissionChoose) {
            this.listPermissionAuto.forEach(itemFunc => {
              var itemChoose = this.listPermissionChoose.filter(a => a.id === itemFunc.id)[0];
              if (itemChoose) {
                itemFunc.isCheck = true;
              }
            });
          }

          this.checkAll = (this.listPermissionAuto.length > 0 && this.listPermissionAuto.length == this.listPermissionChoose.length) ? true : false;
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  //Chuyển trạng thái check all
  selectAll() {
    this.listPermissionAuto.forEach(itemFunc => {
      itemFunc.isCheck = this.checkAll;
    });
  }

  //Sự kiện thay đổi chek trong list quyền
  checkItem(id: string, check: boolean, index: number) {
    var itemsChoose = this.listPermissionAuto.filter(a => a.isCheck);
    if (itemsChoose.length == this.listPermissionAuto.length)
      this.checkAll = true;
    else
      this.checkAll = false;
  }
}
