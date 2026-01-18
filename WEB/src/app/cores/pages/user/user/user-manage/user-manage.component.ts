import {
  ChangeDetectorRef,
  Component,
  OnInit,
  ViewChild,
  ViewEncapsulation,
} from '@angular/core';
import { Router } from '@angular/router';
import { Observable, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { UserService } from '../../service/user.service';
import { TranslateService } from '@ngx-translate/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Constants } from 'src/app/cores/shared/common/constants';
import { MessageService } from 'src/app/cores/shared/services/message.service';
import { LanguageService } from 'src/app/cores/shared/services/language.service';
import { MenuOptions } from 'src/app/cores/shared/models/menu-options.model';
import { ChangePasswordComponent } from 'src/app/modules/auth/components/change-password/change-password.component';
import { ComboboxCoreService } from 'src/app/cores/shared/services/combobox-core.service';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import { NtsToolbarSearchService } from 'src/app/cores/partials/layout/drawers/nts-search-drawer/nts-search-drawer.service';

@Component({
  selector: 'app-user-manage',
  templateUrl: './user-manage.component.html',
  styleUrls: ['./user-manage.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class UserManageComponent implements OnInit {
  public initialPage?: object;
  constructor(
    public constant: Constants,
    private userService: UserService,
    private messageService: MessageService,
    private router: Router,
    private searchGlobalService: NtsToolbarSearchService,
    private translate: TranslateService,
    private lgService: LanguageService,
    private modalService: NgbModal
  ) {
    this._unsubscribeAll = new Subject();

    // Cấu hình phân trang
    this.pageSettings = { pageSize: 1 };
  }

  @ViewChild('usergrid') usergrid: GridComponent;
  users: any[] = [];
  totalRecords: any = 0; // Tổng số bản ghi
  pageSettings: any = { pageSize: 15, pageSizes: [15, 30, 40,50] }; // Kích thước trang mặc định
  startIndex = 1;
  _unsubscribeAll: Subject<any>;
  searchModel: any = null;

  userId: string;
  userType: number;
  keyCaheSearch: string;

  // pageSettings: PageSettingsModel;

  ngOnInit(): void {
    // this.initialPage = { pageSize: 1, totalRecordsCount: 3 };
    //Hứng sự kiện thay đổi ngôn ngữ để load lại Component
    // this.lgService.onLanguageChanged.pipe().subscribe((languageCode) => {
    //   if (languageCode) {
    //     this.translate.use(languageCode);
    //   }
    // });

    // this.user = JSON.parse(localStorage.getItem('CurrentUser')??"");
    // if (this.user) {
    //   this.userId = this.user.userId;
    //   // this.userType = this.user.type;
    // }
    //key cache search
    this.keyCaheSearch = this.router.url + '/' + this.userId;

    //Khởi tạo model tìm kiếm
    this.initModelSearch();

    let cacheSearch = localStorage.getItem(this.keyCaheSearch) ?? '';
    if (cacheSearch !== '') {
      this.searchModel = JSON.parse(cacheSearch);
    }

    this.searchGlobalService.onDataChanged
      .pipe(takeUntil(this._unsubscribeAll))
      .subscribe((data) => {
        if (data) {
          
          this.searchModel.userName = data.userName;
          this.searchModel.lockoutEnabled = data.lockoutEnabled;
          this.searchModel.pageNumber = data.pageNumber;
          this.searchModel.fullName = data.fullName;
          //Tài khoản phân theo loại và cấp hành chính thì mở lên
          // this.searchModel.tinhId = data.tinhId;
          // this.searchModel.huyenId = data.huyenId;
          // this.searchModel.xaId = data.xaId;
          // this.searchModel.type = data.type;
          this.search();
          localStorage.setItem(
            this.keyCaheSearch,
            JSON.stringify(this.searchModel)
          );
        }
      });

    this.searchGlobalService.onRefreshData
      .pipe(takeUntil(this._unsubscribeAll))
      .subscribe((data) => {
        if (data) {
          this.clearData();
        }
      });

    this.setToolbarConfig();

    this.pageSettings = {
      pageSize: 15,
      pageCount: 0,
      currentPage: 1,
      totalRecordsCount: 0,
    };

    this.search();
  }

  //Khởi tạo model tìm kiếm
  initModelSearch() {
    this.searchModel = {
      pageSize: 15,
      totalItems: 0,
      pageNumber: 1,
      userName: '',
      fullName: '',
      lockoutEnabled: 'false',
      orderBy: '',
      orderType: '',
    };
  }

  ngOnDestroy(): void {
    // Unsubscribe from all subscriptions
    this.searchGlobalService.setConfig(null);
    this._unsubscribeAll.next(null);
    this._unsubscribeAll.complete();
  }

  //cấu hình khởi tạo trình tìm kiếm
  setToolbarConfig() {
    let meuOptions: MenuOptions = {
      isExcel: false,
      isPDF: false,
      isSearch: true,
      searchModel: this.searchModel,
      searchOptions: {
        FieldContentName: 'userName',
        Placeholder: 'Tìm kiếm tên tài khoản',
        Items: [
          {
            FieldName: 'userName',
            Name: 'Tài khoản',
            Type: 'text',
            DisplayName: 'userName',
            Placeholder: 'Tài khoản',
            ValueName: 'userName',
          },
          {
            FieldName: 'fullName',
            Name: 'Họ và tên',
            Type: 'text',
            DisplayName: 'fullName',
            Placeholder: 'Họ và tên',
            ValueName: 'fullName',
          },
          {
            FieldName: 'description',
            Name: 'Mô tả',
            Type: 'text',
            DisplayName: 'description',
            Placeholder: 'Mô tả',
            ValueName: 'description',
          },
          // {
          //   FieldName: 'type',
          //   Name: 'Loại tài khoản',
          //   Type: 'ngselect',
          //   DisplayName: 'name',
          //   ValueName: 'id',
          //   Hidden: this.userType == this.constant.TypeUser.Ward,//Ẩn khi là tài khoản cấp xã
          //   GetData: (): Observable<any> => {
          //     return this.comboboxService.getLoaiTaiKhoan();
          //   }
          // },
          // {
          //   FieldName: 'tinhId',
          //   Name: 'Tỉnh/Thành phố',
          //   Type: 'ngselect',
          //   DisplayName: 'name',
          //   ValueName: 'id',
          //   Hidden: this.userType >= this.constant.TypeUser.Province,//Ẩn khi là tài khoản tỉnh đổ xuống
          //   IsRelation: true,
          //   RelationIndexTo: 3,
          //   GetData: (): Observable<any> => {
          //     return this.comboboxService.getTinhTP();
          //   }
          // },
          // {
          //   FieldName: 'huyenId',
          //   Name: 'Quận/Huyện',
          //   Type: 'ngselect',
          //   DisplayName: 'name',
          //   ValueName: 'id',
          //   Hidden: this.userType >= this.constant.TypeUser.District,//Ẩn khi là tài khoản cấp huyển xuống
          //   IsRelation: true,
          //   RelationIndexTo: 4,
          //   RelationIndexFrom: 2,
          //   GetData: (tinhId: any): Observable<any> => {
          //     return this.comboboxService.getQuanHuyen(tinhId);
          //   }
          // },
          // {
          //   FieldName: 'xaId',
          //   Name: 'Xã/Phường',
          //   Type: 'ngselect',
          //   DisplayName: 'name',
          //   ValueName: 'id',
          //   Hidden: this.userType == this.constant.TypeUser.Ward,//Ẩn khi là tai khoản cấp xã
          //   RelationIndexFrom: 3,
          //   GetData: (huyenId: any): Observable<any> => {
          //     return this.comboboxService.getXaPhuong(huyenId);
          //   }
          // },
          {
            FieldName: 'lockoutEnabled',
            Name: 'Tình trạng',
            Type: 'ngselect',
            DisplayName: 'Name',
            ValueName: 'Id',
            Data: this.constant.User_Status,
          },
        ],
      },
    };

    this.searchGlobalService.setConfig(meuOptions);
  }

  search() {
    this.userService.searchUser(this.searchModel).subscribe({
      next: (data) => {
        this.startIndex =
          (this.searchModel.pageNumber - 1) * this.searchModel.pageSize + 1;
        this.users = data.data.dataResults;
        this.searchModel.totalItems = data.data.totalItems;
        // this.pageSettings.totalRecordsCount = data.data.totalItems;
        // if (this.pageSettings.totalRecordsCount && this.pageSettings.pageSize) {
        //   this.pageSettings.pageCount = Math.ceil(
        //     this.pageSettings?.totalRecordsCount / this.pageSettings?.pageSize
        //   );
        // } else {
        //   this.pageSettings.pageCount = 0;
        // }
        this.totalRecords = data.data.totalItems; // Cập nhật tổng số bản ghi

        // Cập nhật lại số trang trong grid
        this.pageSettings = {
          pageSize: this.searchModel.pageSize,
          pageSizes: [15, 30, 40,50],
          totalRecords: this.totalRecords,
          totalDataRecordsCount:this.totalRecords,
          totalRecordsCount :this.totalRecords,
          enableQueryString: false,
          alwaysShow: false,
        };
        this.usergrid.pageSettings;
        this.usergrid.totalDataRecordsCount = this.totalRecords ;
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });
  }

  // Xử lý sự kiện actionBegin để lấy thông tin phân trang và sắp xếp
  actionBegin(args: any): void {
    if (args.requestType === 'sorting') {
      this.searchModel.orderBy = args.columnName;
      this.searchModel.orderType = args.direction;
      this.search();
    } else if (args.requestType === 'paging') {
      this.searchModel.pageSize = args.pageSize;
      this.search();
    }
  }

  // The custom function
  public sortComparer = (reference: string, comparer: string) => {
    
    if (reference < comparer) {
      return -1;
    }
    if (reference > comparer) {
      return 1;
    }
    return 0;
  };

  showUpdate(id: string) {
    this.router.navigate(['/nguoi-dung/tai-khoan/chinh-sua/' + id]);
  }

  showCreate() {
    this.router.navigate(['/nguoi-dung/tai-khoan/them-moi']);
  }

  showViewUser(id: string) {
    this.router.navigate(['/nguoi-dung/tai-khoan/xem-tai-khoan/' + id]);
  }

  showConfirmDelete(id: string) {
    this.messageService
      .showConfirm('Bạn có chắc muốn xoá tài khoản này không?')
      .then((data) => {
        this.deleteUser(id);
      });
  }

  deleteUser(id: string) {
    this.userService.deleteUser(id).subscribe(
      (data) => {
        if (data.isStatus) {
          this.messageService.showSuccess('Xóa tài khoản thành công!');
          this.search();
        }
      },
      (error) => {
        this.messageService.showError(error);
      }
    );
  }

  showConfirmLockUser(id: string) {
    this.messageService
      .showConfirm('Bạn có chắc muốn khóa tài khoản này không?')
      .then((data) => {
        this.lockUser(id);
      });
  }

  lockUser(id: string) {
    this.userService.userAdminLockOrUnlock(id, true).subscribe(
      (data) => {
        if (data.isStatus) {
          this.search();
          this.messageService.showSuccess('Khóa tài khoản thành công!');
        }
      },
      (error) => {
        this.messageService.showError(error);
      }
    );
  }

  showConfirmUnLockUser(id: string) {
    this.messageService
      .showConfirm('Bạn có chắc muốn mở khóa tài khoản này không?')
      .then((data) => {
        this.unLockUser(id);
      });
  }

  unLockUser(id: string) {
    this.userService.userAdminLockOrUnlock(id, false).subscribe(
      (data) => {
        if (data.isStatus) {
          this.search();
          this.messageService.showSuccess('Mở khóa tài khoản thành công!');
        }
      },
      (error) => {
        this.messageService.showError(error);
      }
    );
  }

  showConfirmResetPassword(id: string) {
    this.changePassword(id, true);
  }

  changePassword(idChange: string, ischange: boolean) {
    let activeModal = this.modalService.open(ChangePasswordComponent, {
      container: 'body',
    });
    activeModal.componentInstance.idChange = idChange;
    activeModal.componentInstance.ischange = ischange;
    activeModal.result.then(
      (result) => {},
      (reason) => {}
    );
  }

  clearData() {
    //Khởi tạo model tìm kiếm
    this.initModelSearch();
    localStorage.removeItem(this.keyCaheSearch);
    this.setToolbarConfig();
    this.search();
  }
}
