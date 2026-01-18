import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { GroupUserService } from '../../service/group-user.service';
import { TranslateService } from '@ngx-translate/core';
import { TreeGridComponent, extendArray } from '@syncfusion/ej2-angular-treegrid';
import { MessageService } from 'src/app/cores/shared/services/message.service';
import { Constants } from 'src/app/cores/shared/common/constants';
import { LanguageService } from 'src/app/cores/shared/services/language.service';
import { ComboboxCoreService } from 'src/app/cores/shared/services/combobox-core.service';

@Component({
  selector: 'app-group-user-create',
  templateUrl: './group-user-create.component.html',
  styleUrls: ['./group-user-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class GroupUserCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private service: GroupUserService,
    public constant: Constants,
    private translate: TranslateService,
    private lgService: LanguageService,
    private comboboxService: ComboboxCoreService
  ) { this.translate.use(this.lgService.getLanguage()); }
  @ViewChild('treegrid')
  public treegrid: TreeGridComponent;

  listPermission: any[] = [];
  isSelectAll = false;
  checkAll: boolean = false;
  modalInfo = {
    Title: 'Thêm mới nhóm người dùng',
    SaveText: 'Lưu',
  };
  height = 0;
  isAction: boolean = false;
  id: string;

  model: any = {
    id: '',
    name: '',
    type: null,
    description: '',
    listPermission: []
  }
  listNhom: any[] = [];
  groupSelect: any = {};
  groupSelectIndex: number = 0;

  ngOnInit(): void {
    this.height = window.innerHeight - 580;
    if (this.id) {
      this.modalInfo.Title = 'Chỉnh sửa nhóm người dùng';
      this.modalInfo.SaveText = 'Lưu';
    }
    else {
      this.modalInfo.Title = "Thêm mới nhóm người dùng";
    }

    this.getGroupUserInfo();
  }

  getGroupUserInfo() {
    this.service.getGroupUserInfo(this.id).subscribe(result => {
      if (result.isStatus) {
        setTimeout(() => {
          this.model = result.data;
        }, 200);
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  create(isContinue: any) {
    this.service.createGroupUser(this.model).subscribe(
      result => {
        if (result.isStatus) {
          this.messageService.showSuccess('Thêm mới nhóm người dùng thành công!');
          if (isContinue) {
            this.isAction = true;
            this.clear();
          } else {
            this.closeModal(true);
          }
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }

  update() {
    this.service.updateGroupUser(this.id, this.model).subscribe(
      result => {
        if (result.isStatus) {
          this.activeModal.close(true);
          this.messageService.showSuccess('Cập nhật nhóm người dùng thành công!');
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }

  save(isContinue: boolean) {
    if (this.id) {
      this.update();
    } else {
      this.create(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  clear() {
    this.model = {
      id: '',
      name: '',
      status: 1,
      description: '',
      listPermission: []
    };

    this.getGroupUserInfo();
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

    //Kiểm tra để chek all
    var permissionsCheck = this.listPermission.filter(s => s.isChecked);
    if (this.listPermission.length > 0 && this.listPermission.length == permissionsCheck.length) {
      this.checkAll = true;
    } else {
      this.checkAll = false;
    }
  }

  //Chuyển trạng thái check all
  selectAll() {
    this.listPermission.forEach(itemFunc => {
      itemFunc.isChecked = this.checkAll;
    });

    var itemsChoose = this.listPermission.filter(a => a.isChecked);
    this.changeDataSub(this.model.listPermission, itemsChoose);

    //refresh tree
    this.treegrid.refresh();
    setTimeout(() => {
      this.treegrid.selectRow(this.groupSelectIndex);
    }, 100);
  }

  checkItem(permission:any) {
    var itemsChoose = this.listPermission.filter(a => a.isChecked);
    if (itemsChoose.length == this.listPermission.length)
      this.checkAll = true;
    else
      this.checkAll = false;

    this.changeDataSub(this.model.listPermission, itemsChoose);

    //refresh tree
    this.treegrid.refresh();
    setTimeout(() => {
      this.treegrid.selectRow(this.groupSelectIndex);
    }, 100);
  }

  changeDataSub(listSub: any[], permissionChoose: any[]) {
    listSub.forEach(item => {
      if (this.groupSelect.id == item.id)
      {
        item.checkCount = permissionChoose.length;
        item.permissions = this.listPermission;
      }

      if (item.children.length > 0)
        this.changeDataSub(item.children, permissionChoose);
    });
  }
}
