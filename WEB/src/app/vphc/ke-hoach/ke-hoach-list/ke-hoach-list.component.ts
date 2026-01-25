import { Component, OnInit, Output, ViewChild, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ContextMenuItemModel, ContextMenuService, GridComponent, PageService } from '@syncfusion/ej2-angular-grids';
import { TabComponent } from '@syncfusion/ej2-angular-navigations/src/tab/tab.component';
import { Subject, Observable, takeUntil, firstValueFrom } from 'rxjs';
import { NtsToolbarSearchService } from 'src/app/cores/partials/layout/drawers/nts-search-drawer/nts-search-drawer.service';
import { Constants, TrangThaiKHKTEnum } from 'src/app/cores/shared/common/constants';
import { FileProcess } from 'src/app/cores/shared/common/file-process';
import { ComboboxCoreService } from 'src/app/cores/shared/services/combobox-core.service';
import { LanguageService } from 'src/app/cores/shared/services/language.service';
import { MessageService } from 'src/app/cores/shared/services/message.service';
import { AuthService, UserType } from 'src/app/modules/auth';
import { MenuOptions } from 'src/app/cores/shared/models/menu-options.model';
import { KeHoachService } from '../../service/ke-hoach.service';


@Component({
  providers: [ContextMenuService, PageService],
  selector: 'app-ke-hoach-list',
  templateUrl: './ke-hoach-list.component.html',
  styleUrl: './ke-hoach-list.component.scss',
  encapsulation: ViewEncapsulation.None,
})
export class KeHoachListComponent implements OnInit {
  @ViewChild('element') tabObj?: TabComponent;
  public initialPage?: object;
  public contextMenuItems?: ContextMenuItemModel[];
  @ViewChild('grKeHoach')
  public grKeHoach?: GridComponent;

  constructor(
    public constant: Constants,
    private keHoachService: KeHoachService,
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
        text: 'Xóa',
        target: '.e-content',
        id: 'delete',
        iconCss: 'fas fa-trash-alt',
      },
      {
        text: 'Trình lãnh đạo cục',
        target: '.e-content',
        id: 'proccess',
        iconCss: 'fas fa-paper-plane',
      },
      {
        text: 'Phân giao triển khai',
        target: '.e-content',
        id: 'tasks',
        iconCss: 'fas fa-tasks',
      },
    ];
  }

  users: any[] = [];
  totalRecords: any = 0; // Tổng số bản ghi
  // tongNguoiVP: any = 0;
  // tongToChucVP: any = 0;
  // tongTienPhat: any = 0;
  _unsubscribeAll: Subject<any>;
  searchModel: any = null;

  userId: string;
  userType: number;
  user$: Observable<UserType>;
  lstYear: any;

  ngOnInit(): void {
    const currentYear = new Date().getFullYear();
    this.lstYear = Array.from({ length: 5 }, (_, i) => {
      const year = (currentYear - i).toString();
      return {
        id: year,
        name: year
      };
    });

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
      orderBy: 'NamThucHienKeHoach',
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
        FieldContentName: 'soQuyetDinhBanHanh',
        Placeholder: 'Tìm kiếm nhanh theo mã hồ sơ',
        Items: [
          {
            FieldName: 'soQuyetDinhBanHanh',
            Name: 'Số quyết định ban hành',
            Type: 'text',
            DisplayName: 'soQuyetDinhBanHanh',
            Placeholder: 'Số quyết định ban hành',
            ValueName: 'soQuyetDinhBanHanh',
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
            FieldName: 'canCu',
            Name: 'Căn cứ',
            Type: 'text',
            DisplayName: 'canCu',
            Placeholder: 'Căn cứ',
            ValueName: 'canCu',
          },
          {
            FieldName: 'mucDich',
            Name: 'Mục đích',
            Type: 'text',
            DisplayName: 'mucDich',
            Placeholder: 'Mục đích',
            ValueName: 'mucDich',
          },
          {
            FieldName: 'yeuCau',
            Name: 'Yêu cầu',
            Type: 'text',
            DisplayName: 'yeuCau',
            Placeholder: 'Yêu cầu',
            ValueName: 'yeuCau',
          },

        ],
      },
    };

    this.ntsToolbarSearchService.setConfig(meuOptions);
  }

  modelResult: any = {};
  search() {
    this.keHoachService.search(this.searchModel).subscribe({
      next: (result) => {
        this.modelResult = result.data;
        this.users = this.modelResult.dataResults;
        this.totalRecords = this.modelResult.totalItems;
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

  contextMenuClick(args: any): void {
    const id = args.rowInfo.rowData.id;
    if (args.item.id === 'edit') {
      this.showUpdate(id);
    } else if (args.item.id === 'detail') {
      this.showView(id);
    } else if (args.item.id === 'delete') {
      this.showConfirmDeleteSoft(id);
    } else if (args.item.id === 'proccess') {
      this.updateProcessing(id);
    }
  }

  showUpdate(id: string) {
    this.router.navigate(['/ke-hoach/chinh-sua/' + id]);
  }

  showCreate() {
    this.router.navigate(['/ke-hoach/them-moi']);
  }

  showView(id: string) {
    this.router.navigate(['/ke-hoach/xem/' + id]);
  }

  /**Xác nhận và xóa tạm vụ việc */
  showConfirmDeleteSoft(id: string) {
    this.messageService
      .showConfirm('Bạn có chắc muốn xoá vụ việc này không?')
      .then((data) => {
        this.keHoachService.deleteSoft(id).subscribe({
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
        this.keHoachService.deleteHard(id).subscribe({
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

  /**Trình lãnh đạo cục kế hoach soạn thảo */
  updateProcessing(id: string) {
    this.messageService
      .showConfirm('Bạn có chắc muốn trình kế hoặc lên lãnh đạo cục')
      .then((data) => {
        const payload = {
          id: id,
          trangThaiKeHoachKiemTra: TrangThaiKHKTEnum
            .DaTrinh
        }
        this.keHoachService.updateStatus(payload).subscribe({
          next: (data) => {
            if (data.isStatus) {
              this.messageService.showSuccess('Kế hoạch đã được trình lên lãnh đạo cục!');
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
    // this.keHoachService.exportExcel(this.searchModel).subscribe({
    //   next: (data) => {
    //     var blob = new Blob([data], { type: 'octet/stream' });
    //     var url = window.URL.createObjectURL(blob);
    //     this.fileProcess.downloadFileLink(url, 'VuViec.xlsx');
    //   },
    //   error: (error) => {
    //     const blb = new Blob([error.error], { type: 'text/plain' });
    //     const reader = new FileReader();

    //     reader.onload = () => {
    //       if (reader && reader.result)
    //         this.messageService.showMessage(
    //           reader.result.toString().replace('"', '').replace('"', '')
    //         );
    //     };
    //     // Start reading the blob as text.
    //     reader.readAsText(blb);
    //   },
    // });
  }

  clearData() {
    //Khởi tạo model tìm kiếm
    this.initModelSearch();
    this.setToolbarConfig();
    this.search();
  }

  public field: Object;
}
