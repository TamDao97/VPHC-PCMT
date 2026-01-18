import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Observable, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { GroupUserService } from '../../service/group-user.service';
import { GroupUserCreateComponent } from '../group-user-create/group-user-create.component';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'src/app/cores/shared/services/message.service';
import { Constants } from 'src/app/cores/shared/common/constants';
import { SearchGlobalService } from 'src/app/cores/shared/common/search-global.service';
import { LanguageService } from 'src/app/cores/shared/services/language.service';
import { ComboboxCoreService } from 'src/app/cores/shared/services/combobox-core.service';
import { MenuOptions } from 'src/app/cores/shared/models/menu-options.model';

@Component({
  selector: 'app-group-user-manage',
  templateUrl: './group-user-manage.component.html',
  styleUrls: ['./group-user-manage.component.scss']
})
export class GroupUserManageComponent implements OnInit {

  constructor(
    private messageService: MessageService,
    private modalService: NgbModal,
    private serviceGroupUser: GroupUserService,
    public constant: Constants,
    private searchGlobalService: SearchGlobalService,
    private translate: TranslateService,
    private lgService: LanguageService
  ) {
    this._unsubscribeAll = new Subject();
    this.translate.use(this.lgService.getLanguage());
  }

  _unsubscribeAll: Subject<any>;
  searchModel: any;

  ngOnInit(): void {
    //Hứng sự kiện thay đổi ngôn ngữ để load lại Component
    this.lgService.onLanguageChanged.pipe().subscribe(languageCode => {
      if (languageCode) {
        this.translate.use(languageCode);
      }
    });

    //Khởi tạo model search
    this.initModelSearch();

    this.searchGlobalService.onDataChanged.pipe(takeUntil(this._unsubscribeAll)).subscribe(data => {
      if (data) {
        this.searchModel.name = data.name;
        this.searchModel.type = data.type;
        this.searchModel.pageNumber = data.pageNumber;
        this.search();
      }
    });

    this.searchGlobalService.onRefreshData.pipe(takeUntil(this._unsubscribeAll)).subscribe(data => {
      if (data) {
        this.clearData();
      }
    });

    this.setToolbarConfig();
  }

  //Khởi tạo model tìm kiếm
  initModelSearch() {
    this.searchModel = {
      pageSize: 10,
      totalItems: 0,
      pageNumber: 1,
      orderBy: '',
      orderType: '',
      name: '',
      type: null
    }
  }

  ngOnDestroy(): void {
    // Unsubscribe from all subscriptions
    this.searchGlobalService.setConfig(null);
    this._unsubscribeAll.next(null);
    this._unsubscribeAll.complete();
  }

  setToolbarConfig() {
    let meuOptions: MenuOptions = {
      isExcel: false,
      isPDF: false,
      isSearch: true,
      searchModel: this.searchModel,
      searchOptions: {
        FieldContentName: 'name',
        Placeholder: 'Tìm kiếm tên nhóm',
        Items: [
        ]
      }
    };

    this.searchGlobalService.setConfig(meuOptions);
  }

  groups: any[] = [];
  startIndex = 0;
  clickOrderBy(orderBy: any) {
    this.searchModel.orderBy = orderBy;
    if (this.searchModel.orderType == "ASC") {
      this.searchModel.orderType = "DESC";
    } else {
      this.searchModel.orderType = "ASC";
    }
    this.search();
  }

  search() {
    this.serviceGroupUser.searchGroupUser(this.searchModel).subscribe((data: any) => {
      if (data.isStatus) {
        this.startIndex = ((this.searchModel.pageNumber - 1) * this.searchModel.pageSize + 1);
        this.groups = data.data.dataResults;
        this.searchModel.totalItems = data.data.totalItems;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  clearData() {
    //Khởi tạo model tìm kiếm
    this.initModelSearch();
    this.setToolbarConfig();
  }

  showConfirmDelete(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nhóm người dùng này không?").then(
      data => {
        this.delete(Id);
      }
    );
  }

  delete(Id: string) {
    this.serviceGroupUser.deleteGroupUser(Id).subscribe(
      data => {
        if (data.isStatus) {
          this.messageService.showSuccess('Xóa nhóm người dùng thành công!');
          this.search();
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdate(id: string) {
    let activeModal = this.modalService.open(GroupUserCreateComponent, { container: 'body', windowClass: 'group-user-create', backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.result.then((result) => {
      if (result) {
        this.search();
      }
    }, (reason) => {
    });
  }
}
