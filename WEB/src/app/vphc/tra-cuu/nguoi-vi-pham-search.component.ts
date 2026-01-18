import {
  ChangeDetectorRef,
  Component,
  OnInit,
  ViewChild,
  ViewEncapsulation,
} from '@angular/core';
import { Router } from '@angular/router';
import { firstValueFrom, Observable, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Constants } from 'src/app/cores/shared/common/constants';
import { MessageService } from 'src/app/cores/shared/services/message.service';
import { SearchGlobalService } from 'src/app/cores/shared/common/search-global.service';
import { LanguageService } from 'src/app/cores/shared/services/language.service';
import { MenuOptions } from 'src/app/cores/shared/models/menu-options.model';
import { ChangePasswordComponent } from 'src/app/modules/auth/components/change-password/change-password.component';
import { ComboboxCoreService } from 'src/app/cores/shared/services/combobox-core.service';
import {
  ContextMenuItemModel,
  ContextMenuService,
  GridComponent,
  PageService,
} from '@syncfusion/ej2-angular-grids';
import {
  MenuEventArgs,
  TabComponent,
} from '@syncfusion/ej2-angular-navigations';
import { FileProcess } from 'src/app/cores/shared/common/file-process';
import { NtsToolbarSearchService } from 'src/app/cores/partials/layout/drawers/nts-search-drawer/nts-search-drawer.service';
import { TraCuuService } from '../service/tra-cuu.service';
import { AuthService, UserType } from 'src/app/modules/auth';

@Component({
  providers: [ContextMenuService, PageService],
  selector: 'app-nguoi-vi-pham-search',
  templateUrl: './nguoi-vi-pham-search.component.html',
  styleUrls: ['./nguoi-vi-pham-search.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class NguoiViPhamSearchComponent implements OnInit {
  @ViewChild('element') tabObj?: TabComponent;
  public initialPage?: object;
  public contextMenuItems?: ContextMenuItemModel[];
  @ViewChild('grTraCuu')
  public grTraCuu?: GridComponent;

  constructor(
    public constant: Constants,
    private traCuuService: TraCuuService,
    private fileProcess: FileProcess,
    private messageService: MessageService,
    private router: Router,
    private searchGlobalService: SearchGlobalService,
    private translate: TranslateService,
    private lgService: LanguageService,
    private modalService: NgbModal,
    private ntsToolbarSearchService: NtsToolbarSearchService,
    private comboboxService: ComboboxCoreService, // private contextMenuService : ContextMenuService,
    private auth: AuthService
  ) {
    this._unsubscribeAll = new Subject();
  }

  // @ViewChild('usergrid') usergrid: GridComponent;
  users: any[] = [];
  totalRecords: any = 0; // Tổng số bản ghi
  tongLanVP: any = 0;
  tongTienPhat: any = 0;
  _unsubscribeAll: Subject<any>;
  searchModel: any = null;

  dataQuick: any;

  userId: string;
  userType: number;
    user$: Observable<UserType>;

  ngOnInit(): void {
    this.user$ = this.auth.currentUserSubject.asObservable();

    //Khởi tạo model tìm kiếm
    this.initModelSearch();

    this.user$.subscribe(async (user) => {
      if (user != null) {
        this.searchModel.idDonVi = user.idDonVi;
      }
    });

    this.ntsToolbarSearchService.onDataChanged
      .pipe(takeUntil(this._unsubscribeAll))
      .subscribe((data) => {
        if (data) {
          this.searchModel = data;
          this.search();
        }
      });

    this.ntsToolbarSearchService.onRefreshData.subscribe((data) => {
      if (data) {
        this.clearData();
      }
    });

    this.setToolbarConfig();

    this.search();

    this.quickDashboard();
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
      orderBy: 'NgayViPham',
      orderType: 'Descending',
    };
  }

  ngOnDestroy(): void {
    // Unsubscribe from all subscriptions
    this.searchGlobalService.setConfig(null);
    this._unsubscribeAll.next(null);
    this._unsubscribeAll.complete();
  }

  //cấu hình khởi tạo trình tìm kiếm
  async setToolbarConfig() {
    let meuOptions: MenuOptions = {
      isExcel: false,
      isPDF: false,
      isSearch: true,
      searchModel: this.searchModel,
      searchOptions: {
        FieldContentName: 'hoVaTen',
        Placeholder: 'Tìm kiếm nhanh theo họ và tên',
        Items: [
          {
            FieldName: 'hoVaTen',
            Name: 'Họ và tên',
            Type: 'text',
            DisplayName: 'hoVaTen',
            Placeholder: 'Họ và tên',
            ValueName: 'hoVaTen',
          },
          {
            FieldName: 'ngaySinh',
            Name: 'Ngày sinh',
            Type: 'date',
            DisplayName: 'ngaySinh',
            Placeholder: 'Ngày sinh',
            ValueName: 'ngaySinh',
            FieldNameTo: 'ngaySinhTo',
            FieldNameFrom: 'ngaySinhFrom',
          },
          {
            FieldName: 'gioiTinh',
            Name: 'Giới tính',
            Type: 'ngselect',
            DisplayName: 'Name',
            ValueName: 'Id',
            Data: this.constant.listGioiTinh,
          },
          {
            FieldName: 'queQuan',
            Name: 'Quê quán',
            Type: 'text',
            DisplayName: 'queQuan',
            Placeholder: 'Quê quán',
            ValueName: 'queQuan',
          },
          {
            FieldName: 'idDonVi',
            Name: 'Đơn vị xử lý',
            Type: 'ngselect',
            DisplayName: 'name',
            ValueName: 'id',
            Data: (await firstValueFrom(this.comboboxService.getDonVi())).data,
          },
          {
            FieldName: 'ngayViPham',
            Name: 'Ngày vi phạm',
            Type: 'date',
            DisplayName: 'ngayViPham',
            Placeholder: 'Ngày vi phạm',
            ValueName: 'ngayViPham',
            FieldNameTo: 'ngayViPhamTo',
            FieldNameFrom: 'ngayViPhamFrom',
          },
          {
            FieldName: 'linhVuc',
            Name: 'Lĩnh vực vi phạm',
            Type: 'ngselect',
            DisplayName: 'name',
            ValueName: 'id',
            Data: (await firstValueFrom(this.comboboxService.getLinhVucBCTH()))
              .data,
          },
          {
            FieldName: 'tienTrinh',
            Name: 'Tiến tình vụ việc',
            Type: 'ngselect',
            DisplayName: 'Name',
            ValueName: 'Id',
            Data: this.constant.VuViec_TienTrinh,
          },
        ],
      },
    };

    this.ntsToolbarSearchService.setConfig(meuOptions);
  }

  modelResult: any = {};
  search() {
    this.traCuuService.search(this.searchModel).subscribe({
      next: (result) => {
        this.modelResult = result.data;
        this.users = this.modelResult.dataResults;
        this.totalRecords = this.modelResult.totalItems;
        this.tongLanVP = this.modelResult.tongLanVP;
        this.tongTienPhat = this.modelResult.tongTienPhat;
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
      this.searchModel.pageNumber = args.page;
      this.search();
    } else if (args.requestType === 'pagesize') {
      this.searchModel.pageSize = args.pageSize;
      this.searchModel.pageNumber = 1; // Reset về trang 1
      this.search();
    }
  }

  //Xuất excel
  exportExcel() {
    this.traCuuService.exportExcel(this.searchModel).subscribe({
      next: (data) => {
        var blob = new Blob([data], { type: 'octet/stream' });
        var url = window.URL.createObjectURL(blob);
        this.fileProcess.downloadFileLink(url, 'TraCuu.xlsx');
      },
      error: (error) => {
        const blb = new Blob([error.error], { type: 'text/plain' });
        const reader = new FileReader();

        reader.onload = () => {
          if (reader && reader.result)
            this.messageService.showMessage(
              reader.result.toString().replace('"', '').replace('"', '')
            );
        };
        // Start reading the blob as text.
        reader.readAsText(blb);
      },
    });
  }

  //Thống kê nhanh
  quickDashboard() {
    this.traCuuService.quickDashboard(this.searchModel).subscribe({
      next: (data) => {
        this.dataQuick = data.data;
        this.field = {
          dataSource: data.data,
          id: 'id',
          text: 'text',
          child: 'child',
        };
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });
  }

  clearData() {
    //Khởi tạo model tìm kiếm
    this.initModelSearch();
    this.setToolbarConfig();
    this.search();
  }

  public field: Object;

  changeLinhVuc(type: any) {
    // if (this.searchModel.linhVuc !== type) this.searchModel.linhVuc = type;
    // else {
    //   this.searchModel.linhVuc = '';
    // }
    // this.search();
  }
}
