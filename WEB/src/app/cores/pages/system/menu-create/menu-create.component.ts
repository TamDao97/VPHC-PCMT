import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { MenuService } from '../service/menu.service';
import {
  RowDDService,
  SelectionService,
  TreeGridComponent,
} from '@syncfusion/ej2-angular-treegrid';
import { ChoosePermissionAutoComponent } from '../choose-permission-auto/choose-permission-auto.component';
import { PermissionCreateComponent } from '../permission-create/permission-create.component';
import { MessageService } from 'src/app/cores/shared/services/message.service';
import { Constants } from 'src/app/cores/shared/common/constants';
import { ComboboxCoreService } from 'src/app/cores/shared/services/combobox-core.service';
import { LanguageService } from 'src/app/cores/shared/services/language.service';


@Component({
  selector: 'app-menu-create',
  templateUrl: './menu-create.component.html',
  styleUrls: ['./menu-create.component.scss'],
  encapsulation: ViewEncapsulation.None,
  providers: [RowDDService, SelectionService],
})
export class MenuCreateComponent implements OnInit {
  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public constant: Constants,
    private translate: TranslateService,
    private lgService: LanguageService,
    private comboboxService: ComboboxCoreService,
    private menuService: MenuService,
    private modalService: NgbModal
  ) {
    this.translate.use(this.lgService.getLanguage());
  }

  @ViewChild('treegrid')
  public treegrid: TreeGridComponent;

  modalInfo = {
    Title: 'Thêm mới nhóm người dùng',
    SaveText: 'Lưu',
  };
  isAction: boolean = false;
  id: string;
  parentid: string;
  listMenu: [];
  listSystemFunctionAuto: any[] = [];
  model: any = {
    titleDefault: '',
    titleKeyTranslate: '',
    icon: '',
    url: '',
    type: '',
    parentId: null,
    isDefaultMenu: false,
    functionAuto: false,
    systemFunctionConfigId: null,
    listPermission: [],
  };

  ngOnInit(): void {
    //Hứng sự kiện thay đổi ngôn ngữ để load lại Component
    this.lgService.onLanguageChanged.pipe().subscribe((languageCode) => {
      if (languageCode) {
        this.translate.use(languageCode);
      }
    });

    this.getDataCombobox();

    if (this.parentid) {
      this.model.parentId = this.parentid;
    }
    if (this.id) {
      this.modalInfo.Title = 'Chỉnh sửa menu';
      this.modalInfo.SaveText = 'Lưu';
      this.getMenuByid();
    } else {
      this.modalInfo.Title = 'Thêm mới menu';
    }
  }

  save() {
    if (this.id) {
      this.updateMenu();
    } else {
      this.createMenu();
    }
  }

  getDataCombobox() {
    this.comboboxService.getListMenu().subscribe(
      (result) => {
        if (result.isStatus) {
          this.listMenu = result.data;
        }
      },
      (error) => {
        this.messageService.showError(error);
      }
    );

    this.comboboxService.getSystemFunctionAuto().subscribe(
      (result) => {
        if (result.isStatus) {
          this.listSystemFunctionAuto = result.data;
        }
      },
      (error) => {
        this.messageService.showError(error);
      }
    );
  }

  getMenuByid() {
    if (this.id) {
      this.menuService.getMenuById(this.id).subscribe(
        (result: any) => {
          this.model = result.data;
        },
        (error) => {
          this.messageService.showError(error);
        }
      );
    }
  }

  createMenu() {
    this.menuService.createMenu(this.model).subscribe(
      (data: any) => {
        this.messageService.showSuccess('Thêm mới thành công');
        this.activeModal.close(true);
      },
      (error) => {
        this.messageService.showError(error);
      }
    );
  }

  updateMenu() {
    this.menuService.updateMenu(this.id, this.model).subscribe(
      (data: any) => {
        this.messageService.showSuccess('Chỉnh sửa thành công');
        this.activeModal.close(true);
      },
      (error) => {
        this.messageService.showError(error);
      }
    );
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  clear() {
    this.model = {
      titleDefault: '',
      titleKeyTranslate: '',
      icon: '',
      url: '',
      type: '',
      parentId: '',
      isDefaultMenu: false,
      functionAuto: false,
      listPermission: [],
    };
  }

  //Thay đổi trạng thái DefaultMenu
  selectDefaultMenu() {
    if (this.model.listPermission.length > 0) {
      this.messageService.showConfirm('Xóa hết quyền?').then((data) => {
        this.model.listPermission = [];
      });
    }
  }

  //Thay đổi trạng thái FunctionAuto
  selectFunctionAuto() {
    this.model.listPermission = [];
    this.model.systemFunctionConfigId = null;
    this.model.url = '';
  }

  onChangeSystemFunctionAuto() {
    if (this.model.systemFunctionConfigId) {
      this.listSystemFunctionAuto.forEach((item) => {
        if (item.id == this.model.systemFunctionConfigId) {
          this.model.url = '/system-config/function/manage/' + item.code;
          return;
        }
      });
    } else this.model.url = '';
  }

  //Show chọn quyền cho chức năng động
  choosePermission() {
    let activeModal = this.modalService.open(ChoosePermissionAutoComponent, {
      container: 'body',
      windowClass: 'app-choose-permission-auto',
      backdrop: 'static',
    });
    activeModal.componentInstance.listPermissionChoose =
      this.model.listPermission;
    activeModal.result.then(
      (result) => {
        if (result) {
          this.model.listPermission =
            activeModal.componentInstance.listPermissionChoose;
        }
      },
      (reason) => {}
    );
  }

  //Thêm quyền cho chức năng tĩnh
  addPermission() {
    let activeModal = this.modalService.open(PermissionCreateComponent, {
      container: 'body',
      windowClass: 'app-permission-create',
      backdrop: 'static',
    });
    activeModal.componentInstance.listPermissionChoose = [
      ...this.model.listPermission,
    ];
    activeModal.result.then(
      (result) => {
        if (result) {
          if (activeModal.componentInstance.listPermissionChoose.length > 0) {
            activeModal.componentInstance.listPermissionChoose.forEach(
              (item: any) => {
                this.model.listPermission.push(item);
              }
            );
          } else {
            this.model.listPermission.push(activeModal.componentInstance.model);
          }
          this.treegrid.refresh();
        }
      },
      (reason) => {}
    );
  }

  //Cập nhật quyền cho chức năng tĩnh
  updatePermission(item: any) {
    let activeModal = this.modalService.open(PermissionCreateComponent, {
      container: 'body',
      windowClass: 'app-permission-create',
      backdrop: 'static',
    });
    activeModal.componentInstance.listPermissionChoose =
      this.model.listPermission;
    activeModal.componentInstance.isUpdate = true;
    activeModal.componentInstance.model = item;
    activeModal.result.then(
      (result) => {
        if (result) {
          this.model.listPermission.forEach((itemUpdate: any) => {
            if (itemUpdate.id == item.id) {
              itemUpdate.name = activeModal.componentInstance.model.name;
              itemUpdate.code = activeModal.componentInstance.model.code;
              this.treegrid.refresh();
              return;
            }
          });
        }
      },
      (reason) => {}
    );
  }

  //Xóa quyền
  showConfirmDelete(item: any) {
    this.messageService
      .showConfirm('Bạn có chắc muốn xóa quyền này không?')
      .then((data) => {
        this.model.listPermission = this.model.listPermission.filter(
          (a: any) => a.code != item.code
        );
        this.treegrid.refresh();
      });
  }
}
