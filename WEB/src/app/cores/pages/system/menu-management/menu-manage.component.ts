import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { MenuCreateComponent } from '../menu-create/menu-create.component';
import { MenuService } from '../service/menu.service';
import { Router } from '@angular/router';
import {
  RowDDService,
  SelectionService,
  TreeGridComponent,
} from '@syncfusion/ej2-angular-treegrid';
import { Constants } from 'src/app/cores/shared/common/constants';
import { SearchGlobalService } from 'src/app/cores/shared/common/search-global.service';
import { LanguageService } from 'src/app/cores/shared/services/language.service';
import { MessageService } from 'src/app/cores/shared/services/message.service';
import { MenuOptions } from 'src/app/cores/shared/models/menu-options.model';

@Component({
  selector: 'app-menu-manage',
  templateUrl: './menu-manage.component.html',
  styleUrls: ['./menu-manage.component.scss'],
  encapsulation: ViewEncapsulation.None,
  providers: [RowDDService, SelectionService],
})
export class MenuManageComponent implements OnInit {
  @ViewChild('treegrid')
  public grid!: TreeGridComponent;
  constructor(
    private messageService: MessageService,
    private modalService: NgbModal,
    public constant: Constants,
    private searchGlobalService: SearchGlobalService,
    private translate: TranslateService,
    private lgService: LanguageService,
    private menuservice: MenuService,
    private router: Router
  ) {
    this._unsubscribeAll = new Subject();
    this.translate.use(this.lgService.getLanguage());
  }
  menulist: Object[] = [];
  // public selectOptions: Object;
  _unsubscribeAll: Subject<any>;
  startIndex = 1;
  height = 0;
  public searchSettingModel: Object = {};
  public toolbar: string[] = [];
  searchModel: any = {
    pageSize: 10,
    totalItems: 0,
    pageNumber: 1,
    titleKeyTranslate: '',
    titleDefault: '',
    icon: '',
    url: '',
    orderBy: '',
    orderType: '',
  };

  ngOnInit(): void {
    // this.selectOptions = { type: 'Multiple' };
    //Hứng sự kiện thay đổi ngôn ngữ để load lại Component
    this.lgService.onLanguageChanged.pipe().subscribe((languageCode) => {
      if (languageCode) {
        this.translate.use(languageCode);
      }
    });

    this.height = window.innerHeight - 400;

    this.searchGlobalService.onDataChanged
      .pipe(takeUntil(this._unsubscribeAll))
      .subscribe((data) => {
        if (data) {
          this.searchModel.pageNumber = data.pageNumber;
          this.searchModel.titleDefault = data.titleDefault;
          this.searchModel.titleKeyTranslate = data.titleKeyTranslate;
          this.searchModel.url = data.url;
          this.search();
        }
      });

    this.setToolbarConfig();

    this.search();
  }

  ngOnDestroy(): void {
    // Unsubscribe from all subscriptions
    this.searchGlobalService.setConfig(null);
    this._unsubscribeAll.next(null);
    this._unsubscribeAll.complete();
  }
  search() {
    this.menuservice.searchMenu(this.searchModel).subscribe({
      next: (data: any) => {
        if (data.isStatus) {
          setTimeout(() => {
            this.startIndex =
              (this.searchModel.pageNumber - 1) * this.searchModel.pageSize + 1;
            this.menulist = data.data.dataResults;
            this.searchModel.totalItems = data.data.totalItems;
          }, 200);
        }
      },
      error: (error: any) => {
        this.messageService.showError(error);
      },
    });
  }

  setToolbarConfig() {
    let meuOptions: MenuOptions = {
      isExcel: false,
      isPDF: false,
      isSearch: true,
      searchModel: this.searchModel,
      searchOptions: {
        FieldContentName: 'titleDefault',
        Placeholder: 'Tìm kiếm tên menu',
        Items: [
          {
            FieldName: 'titleDefault',
            Name: 'Tên menu',
            Type: 'text',
            Placeholder: '',
          },
          {
            FieldName: 'titleKeyTranslate',
            Name: 'Key dịch đa ngôn ngữ',
            Type: 'text',
            Placeholder: '',
          },
          {
            FieldName: 'url',
            Name: 'Url',
            Type: 'text',
            Placeholder: '',
          },
        ],
      },
    };

    this.searchGlobalService.setConfig(meuOptions);
  }
  clickOrderBy(orderBy: any) {
    this.searchModel.orderBy = orderBy;
    if (this.searchModel.orderType == 'ASC') {
      this.searchModel.orderType = 'DESC';
    } else {
      this.searchModel.orderType = 'ASC';
    }
    this.search();
  }

  showCreateUpdate(id: string, parentid: string = '') {
    let activeModal = this.modalService.open(MenuCreateComponent, {
      container: 'body',
      windowClass: 'menu-create',
      backdrop: 'static',
    });
    activeModal.componentInstance.id = id;
    activeModal.componentInstance.parentid = parentid;
    activeModal.result.then(
      (result) => {
        if (result) {
          this.search();
        }
      },
      (reason) => {}
    );
  }
  showConfirmDelete(id: string) {
    this.messageService
      .showConfirm('Bạn có chắc muốn xóa menu này không?')
      .then((data) => {
        this.deleteMenu(id);
      });
  }
  deleteMenu(id: string) {
    this.menuservice.deleteMenu(id).subscribe(
      (data) => {
        if (data.isStatus) {
          this.messageService.showSuccess('Xóa menu thành công!');
          this.search();
        }
      },
      (error) => {
        this.messageService.showError(error);
      }
    );
  }
  disableConfirm(id: string) {
    this.messageService
      .showConfirm('Bạn có chắc muốn ẩn/hiện menu này không?')
      .then((data) => {
        this.disable(id);
      });
  }
  disable(id: string) {
    this.menuservice.disableMenu(id).subscribe(
      (data) => {
        if (data.isStatus) {
          this.messageService.showSuccess('Ẩn/hiện thành công!');
          this.search();
        }
      },
      (error) => {
        this.messageService.showError(error);
      }
    );
  }
  clear() {
    this.searchModel = {
      pageSize: 10,
      totalItems: 0,
      pageNumber: 1,

      name: '',
      status: 1,
    };
  }

  onChange(event: any) {
    // = event.itemData.id;
  }
  rowSelected($event: any) {}

  updateIndex() {
    this.menuservice.updateIndex(this.grid.parentData).subscribe(
      (data) => {
        if (data.isStatus) {
          this.messageService.showSuccess(
            'Cập nhật thứ tự hiển thị menu thành công!'
          );
          this.search();
        }
      },
      (error) => {
        this.messageService.showError(error);
      }
    );
  }
}
