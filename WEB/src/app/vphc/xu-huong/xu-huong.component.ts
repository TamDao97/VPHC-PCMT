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

import Highcharts from 'highcharts/highmaps';
import { XuHuongService } from '../service/xu-huong.service';

@Component({
  providers: [ContextMenuService, PageService],
  selector: 'app-xu-huong',
  templateUrl: './xu-huong.component.html',
  styleUrls: ['./xu-huong.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class XuHuongComponent implements OnInit {
  @ViewChild('element') tabObj?: TabComponent;
  public initialPage?: object;
  public contextMenuItems?: ContextMenuItemModel[];
  @ViewChild('grTraCuu')
  public grTraCuu?: GridComponent;

  constructor(
    public constant: Constants,
    private xuHuongService: XuHuongService,
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
    this.xuHuongService.search(this.searchModel).subscribe({
      next: (result) => {
        this.modelResult = result.data;
        this.getChart5Nam(
          this.modelResult.listNam,
          this.modelResult.listNamSoVu,
          this.modelResult.listNamTongTien,
          this.modelResult.listNamSoNguoi
        );

        this.getChartDonVi(this.modelResult.namBaoCao,
          this.modelResult.namSoSanh,
          this.modelResult.listVuViecDonVi.map((s:any)=>s.donViNgan),
          this.modelResult.listVuViecDonVi.map((s:any)=>s.soVu),
          this.modelResult.listVuViecDonVi.map((s:any)=>s.soNguoiVP))

        this.getChartDoTuoi(
          this.modelResult.namBaoCao,
          this.modelResult.namSoSanh,
          this.modelResult.listDoTuoi,
          this.modelResult.listDoTuoiBaoCao,
          this.modelResult.listDoTuoiSoSanh
        );

        this.getChartLinhVuc(
          this.modelResult.namBaoCao,
          this.modelResult.namSoSanh,
          this.modelResult.listLinhVuc,
          this.modelResult.listLinhVucBaoCao,
          this.modelResult.listLinhVucSoSanh
        );
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

  //#region -------------Biểu đồ 12 tháng-------------
  public chart5NamOptions: any = {};
  getChart5Nam(nam: any[], namVu: any[], namTien: any[], namNguoiViPham: any[]) {
    this.chart5NamOptions = {
      series: [
        {
          name: 'Số vụ',
          type: 'column', // Dữ liệu dạng cột
          data: namVu,
          yAxisIndex: 0
        },
        {
          name: 'Số người vi phạm',
          type: 'column', // Dữ liệu dạng cột
          data: namNguoiViPham, // Thêm dữ liệu số người vi phạm ở đây
          yAxisIndex: 0
        },
        {
          name: 'Tổng tiền xử phạt',
          type: 'line', // Dữ liệu dạng đường
          data: namTien,
          yAxisIndex: 1
        },
      ],
      chart: {
        height: 350,
        type: 'line',
        toolbar: {
          show: false,
        },
      },
      stroke: {
        width: [0, 0,4],
      },
      title: {
        text: '',
      },
      dataLabels: {
        enabled: true,
        formatter: function (val:number) {
          return val.toLocaleString().replace(/,/g, '.');  // Định dạng số với dấu phẩy hàng nghìn
      }
      },
      labels: nam,
      xaxis: {
        type: 'string',
      },
      yaxis: [
        {
          title: {
            text: 'Số vụ',
          },
        },
        {
          title: {
            text: 'Số người vi phạm',
          },
        },
        {
          opposite: true,
          title: {
            text: 'Tổng tiền xử phạt',
          },
        },
      ],
      colors: ['#198B4E', '#F6A953','#ED3237'],
      legend: {
        position: 'top', // Đặt vị trí legend trên cùng
        horizontalAlign: 'right',
      },
    };
  }
  //#endregion

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

  //#region -------------Xu hướng độ tuổi-------------
  public chartDoTuoiptions: any = {};
  getChartDoTuoi(
    namBC: any,
    namSS: any,
    listDoTuoi: any,
    listBaoCao: any,
    listSoSanh: any
  ) {
    this.chartDoTuoiptions = {
      series: [
        {
          name: namSS,
          type: 'line', // Dữ liệu dạng đường
          data: listSoSanh,
        },
        {
          name: namBC,
          type: 'line', // Dữ liệu dạng cột
          data: listBaoCao,
        },
        
      ],
      chart: {
        height: 400,
        type: 'line', // Chế độ mặc định là line nhưng kết hợp thêm column
        stacked: false,
        toolbar: {
          show: false,
        },
      },
      plotOptions: {
        bar: {
          horizontal: false, // Cột thẳng đứng
          borderWidth: 0,
        },
      },
      xaxis: {
        categories: listDoTuoi,
      },
      title: {
        text: 'Xu hướng theo độ tuổi vi phạm',
        align: 'left',
      },
      dataLabels: {
        enabled: true,
        formatter: function (val:number) {
          return val.toLocaleString().replace(/,/g, '.');  // Định dạng số với dấu phẩy hàng nghìn
      }
      },
      legend: {
        position: 'top', // Đặt vị trí legend trên cùng
        horizontalAlign: 'right',
      },
      colors: ['#198B4E', '#F6A953'],
    };
  }
  //#endregion

  //#region -------------Xu hướng theo lĩnh vực-------------
  public chartLinhVucOptions: any = {};
  getChartLinhVuc(
    namBC: any,
    namSS: any,
    listLinhVuc: any,
    listBaoCao: any,
    listSoSanh: any
  ) {
    this.chartLinhVucOptions = {
      series: [
        {
          name: namSS,
          type: 'column', // Dữ liệu dạng đường
          data: listSoSanh,
        },
        {
          name: namBC,
          type: 'column', // Dữ liệu dạng cột
          data: listBaoCao,
        },
      ],
      chart: {
        height: 400,
        type: 'line', // Chế độ mặc định là line nhưng kết hợp thêm column
        stacked: false,
        toolbar: {
          show: false,
        },
      },
      stroke: {
        width: [0, 0],
      },
      plotOptions: {
        bar: {
          horizontal: false, // Cột thẳng đứng
          borderWidth: 0,
        },
      },
      xaxis: {
        categories: listLinhVuc,
      },
      title: {
        text: 'Xu hướng theo lĩnh vực vi phạm',
        align: 'left',
      },
      dataLabels: {
        enabled: true,
        formatter: function (val:number) {
          return val.toLocaleString().replace(/,/g, '.');  // Định dạng số với dấu phẩy hàng nghìn
      }
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
