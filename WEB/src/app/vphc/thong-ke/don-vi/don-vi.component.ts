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

import { ThongKeService } from '../../service/thong-ke.service';

@Component({
  providers: [ContextMenuService, PageService],
  selector: 'app-thong-ke-don-vi',
  templateUrl: './don-vi.component.html',
  styleUrls: ['./don-vi.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class ThongKeDonViComponent implements OnInit {
  @ViewChild('element') tabObj?: TabComponent;
  public initialPage?: object;
  public contextMenuItems?: ContextMenuItemModel[];
  @ViewChild('grTraCuu')
  public grTraCuu?: GridComponent;

  constructor(
    public constant: Constants,
    private thongKeService: ThongKeService,
    private fileProcess: FileProcess,
    private messageService: MessageService,
    private router: Router,
    private searchGlobalService: SearchGlobalService,
    private translate: TranslateService,
    private lgService: LanguageService,
    private modalService: NgbModal,
    private ntsToolbarSearchService: NtsToolbarSearchService
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

  ngOnInit(): void {
    //Khởi tạo model tìm kiếm
    this.initModelSearch();

    this.ntsToolbarSearchService.onDataChanged
      .pipe(takeUntil(this._unsubscribeAll))
      .subscribe((data) => {
        if (data) {
          this.searchModel.namBaoCao = data.namBaoCao;
          this.searchModel.namSoSanh = data.namSoSanh;
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
      namBaoCao: new Date().getFullYear(),
      namSoSanh: new Date().getFullYear() - 1,
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
      isDashboard: true,
      searchModel: this.searchModel,
    };

    this.ntsToolbarSearchService.setConfig(meuOptions);
  }

  modelResult: any = {};
  search() {
    this.thongKeService.search(this.searchModel).subscribe({
      next: (result) => {
        this.modelResult = result.data;
        this.getChartDonVi(this.modelResult.namBaoCao,
          this.modelResult.namSoSanh,
          this.modelResult.listVuViecDonVi.map((s:any)=>s.donViNgan),
          this.modelResult.listVuViecDonVi.map((s:any)=>s.soVu),
          this.modelResult.listVuViecDonVi.map((s:any)=>s.soNguoiVP));
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
  }

  //#region -------------Xu hướng độ tuổi-------------
  public chartDonViOptions: any = {};
  getChartDonVi(
    namBC: any,
    namSS: any,
    listDonVi: any,
    listVu: any,
    listNguoi: any
  ) {
    this.chartDonViOptions = {
      series: [
        {
          name: namSS,
          type: 'line', // Dữ liệu dạng đường
          data: listVu,
        },
        {
          name: namBC,
          type: 'line', // Dữ liệu dạng cột
          data: listNguoi,
        },
        
      ],
      chart: {
        height: 500,
        type: 'line', // Chế độ mặc định là line nhưng kết hợp thêm column
        stacked: false,
        toolbar: {
          show: false,
        },
      },
      dataLabels: {
        enabled: true,
        formatter: function (val:number) {
          return val.toLocaleString().replace(/,/g, '.');  // Định dạng số với dấu phẩy hàng nghìn
      }
      },
      plotOptions: {
        bar: {
          horizontal: false, // Cột thẳng đứng
          borderWidth: 0,
        },
      },
      xaxis: {
        categories: listDonVi,
      },
      title: {
        text: '',
        align: 'left',
      },
      legend: {
        position: 'top', // Đặt vị trí legend trên cùng
        horizontalAlign: 'right',
      },
      colors: ['#198B4E', '#F6A953'],
    };
  }
  //#endregion
}
