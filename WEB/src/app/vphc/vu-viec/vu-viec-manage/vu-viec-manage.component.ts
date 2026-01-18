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
import { VuViecService } from '../../service/vu-viec.service';
import {
  MenuEventArgs,
  TabComponent,
} from '@syncfusion/ej2-angular-navigations';
import { FileProcess } from 'src/app/cores/shared/common/file-process';
import { NtsToolbarSearchService } from 'src/app/cores/partials/layout/drawers/nts-search-drawer/nts-search-drawer.service';
import { AuthService, UserType } from 'src/app/modules/auth';

@Component({
  providers: [ContextMenuService, PageService],
  selector: 'app-vu-viec-manage',
  templateUrl: './vu-viec-manage.component.html',
  styleUrls: ['./vu-viec-manage.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class VuViecManageComponent implements OnInit {
  @ViewChild('element') tabObj?: TabComponent;
  public initialPage?: object;
  public contextMenuItems?: ContextMenuItemModel[];
  @ViewChild('grVuViec')
  public grVuViec?: GridComponent;

  constructor(
    public constant: Constants,
    private vuViecService: VuViecService,
    private fileProcess: FileProcess,
    private messageService: MessageService,
    private router: Router,
    private translate: TranslateService,
    private lgService: LanguageService,
    private modalService: NgbModal,
    private ntsToolbarSearchService: NtsToolbarSearchService,
    private comboboxService: ComboboxCoreService,
    private auth: AuthService
  ) {
    this._unsubscribeAll = new Subject();
    this.contextMenuItems = [
      {
        text: 'Xử lý',
        target: '.e-content',
        id: 'edit',
        iconCss: 'fa fa-edit',
      },
      {
        text: 'Xem chi tiết',
        target: '.e-content',
        id: 'detail',
        iconCss: 'fas fa-file-alt',
      },
      {
        text: 'Kết thúc vụ việc',
        target: '.e-content',
        id: 'finish',
        iconCss: 'fas fa-power-off',
      },
      {
        text: 'Xóa',
        target: '.e-content',
        id: 'delete',
        iconCss: 'fas fa-trash-alt',
      },
      {
        text: 'Lịch sử thay đổi',
        target: '.e-content',
        id: 'history',
        iconCss: 'fas fa-history',
      },
    ];
  }

  // @ViewChild('usergrid') usergrid: GridComponent;
  users: any[] = [];
  totalRecords: any = 0; // Tổng số bản ghi
  tongNguoiVP: any = 0;
  tongToChucVP: any = 0;
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
      orderBy: 'ThoiGianTiepNhan',
      orderType: 'Descending',
    };
  }

  ngOnDestroy(): void {
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
        FieldContentName: 'maHoSo',
        Placeholder: 'Tìm kiếm nhanh theo mã hồ sơ',
        Items: [
          {
            FieldName: 'maHoSo',
            Name: 'Mã hồ sơ',
            Type: 'text',
            DisplayName: 'maHoSo',
            Placeholder: 'Mã hồ sơ',
            ValueName: 'maHoSo',
          },
          {
            FieldName: 'idDonVi',
            Name: 'Đơn vị xử lý',
            Type: 'ngselect',
            DisplayName: 'name',
            ValueName: 'id',
            Data: (await firstValueFrom(this.comboboxService.getDonViByDonVi())).data,
          },
          {
            FieldName: 'idNguonPhatHien',
            Name: 'Nguồn phát hiện',
            Type: 'ngselect',
            DisplayName: 'name',
            ValueName: 'id',
            Data: (await firstValueFrom(this.comboboxService.getNguonTin()))
              .data,
          },
          {
            FieldName: 'thoiGianTiepNhan',
            Name: 'Ngày vi phạm',
            Type: 'date',
            DisplayName: 'thoiGianTiepNhan',
            ValueName: 'thoiGianTiepNhan',
            FieldNameTo: 'thoiGianTiepNhanTo',
            FieldNameFrom: 'thoiGianTiepNhanFrom',
          },
          {
            FieldName: 'diaDiemPhatHien',
            Name: 'Địa điểm phát hiện',
            Type: 'text',
            DisplayName: 'diaDiemPhatHien',
            Placeholder: 'Địa điểm phát hiện',
            ValueName: 'diaDiemPhatHien',
          },
          {
            FieldName: 'dienBien',
            Name: 'Diễn biến',
            Type: 'text',
            DisplayName: 'dienBien',
            Placeholder: 'Diễn biến',
            ValueName: 'dienBien',
          },
          {
            FieldName: 'phanLoai',
            Name: 'Phân loại',
            Type: 'ngselect',
            DisplayName: 'name',
            ValueName: 'id',
            Data: (await firstValueFrom(this.comboboxService.getPhanLoaiTin()))
              .data,
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
          {
            FieldName: 'IdXuLy',
            Name: 'Xử lý',
            Type: 'ngselect',
            DisplayName: 'name',
            ValueName: 'id',
            Data: (await firstValueFrom(this.comboboxService.getXuLyVPHC('1')))
              .data,
          },
        ],
      },
    };

    this.ntsToolbarSearchService.setConfig(meuOptions);
  }

  modelResult: any = {};
  search() {
    this.vuViecService.search(this.searchModel).subscribe({
      next: (result) => {
        this.modelResult = result.data;
        this.users = this.modelResult.dataResults;
        this.totalRecords = this.modelResult.totalItems;
        this.tongNguoiVP = this.modelResult.tongNguoiVP;
        this.tongToChucVP = this.modelResult.tongToChucVP;
        this.tongTienPhat = this.modelResult.tongTienPhat;
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });

    this.quickDashboard();
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

  contextMenuClick(args: any): void {
    const id = args.rowInfo.rowData.id;
    if (args.item.id === 'edit') {
      this.showUpdate(id);
    } else if (args.item.id === 'detail') {
      this.showView(id);
    } else if (args.item.id === 'finish') {
      this.showConfirmFinish(id);
    } else if (args.item.id === 'delete') {
      this.showConfirmDeleteSoft(id);
    } else if (args.item.id === 'history') {
      this.showHistory(id);
    }
  }

  showUpdate(id: string) {
    this.router.navigate(['/vphc/vu-viec/chinh-sua/' + id]);
  }

  showCreate() {
    this.router.navigate(['/vphc/them-moi']);
  }

  showView(id: string) {
    this.router.navigate(['/vu-viec/xem/' + id]);
  }

  showHistory(id: string) {
    this.router.navigate(['/vu-viec/lich-su/' + id]);
  }

  /**Xác nhận và xóa tạm vụ việc */
  showConfirmDeleteSoft(id: string) {
    this.messageService
      .showConfirm('Bạn có chắc muốn xoá vụ việc này không?')
      .then((data) => {
        this.vuViecService.deleteSoft(id).subscribe({
          next: (data) => {
            if (data.isStatus) {
              this.messageService.showSuccess('Xóa vụ việc thành công!');
              this.search();
            }
          },
          error: (error) => {
            this.messageService.showError(error);
          },
        });
      });
  }

  /**Xác nhận và xóa hản vụ việc */
  showConfirmDeleteHard(id: string) {
    this.messageService
      .showConfirm('Bạn có chắc muốn xoá vụ việc này không?')
      .then((data) => {
        this.vuViecService.deleteHard(id).subscribe({
          next: (data) => {
            if (data.isStatus) {
              this.messageService.showSuccess('Xóa vụ việc thành công!');
              this.search();
            }
          },
          error: (error) => {
            this.messageService.showError(error);
          },
        });
      });
  }

  /**Xác nhận và kết thúc vụ việc */
  showConfirmFinish(id: string) {
    this.messageService
      .showConfirm('Bạn có chắc muốn kết thúc vụ việc này không?')
      .then((data) => {
        this.vuViecService.finish(id).subscribe({
          next: (data) => {
            if (data.isStatus) {
              this.messageService.showSuccess('Kết thúc vụ việc thành công!');
              this.search();
            }
          },
          error: (error) => {
            this.messageService.showError(error);
          },
        });
      });
  }

  //Xuất excel
  exportExcel() {
    this.vuViecService.exportExcel(this.searchModel).subscribe({
      next: (data) => {
        var blob = new Blob([data], { type: 'octet/stream' });
        var url = window.URL.createObjectURL(blob);
        this.fileProcess.downloadFileLink(url, 'VuViec.xlsx');
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
    this.vuViecService.quickDashboard(this.searchModel).subscribe({
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
}
