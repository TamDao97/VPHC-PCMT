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
import topology from './vietnam.geo.json';
import { TrangChuService } from '../service/trang-chu.service';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';

@Component({
  providers: [ContextMenuService, PageService],
  selector: 'app-trang-chu',
  templateUrl: './trang-chu.component.html',
  styleUrls: ['./trang-chu.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class TrangChuComponent implements OnInit {
  @ViewChild('element') tabObj?: TabComponent;
  public initialPage?: object;
  public contextMenuItems?: ContextMenuItemModel[];
  @ViewChild('grTraCuu')
  public grTraCuu?: GridComponent;

  constructor(
    public constant: Constants,
    private trangChuService: TrangChuService,
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
  //Top tỉnh
  listTopTinh: any[];
  //Top tang vật
  listTopTangVat: any[];
  connection: signalR.HubConnection;

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

    this.connection = new signalR.HubConnectionBuilder()
        .withUrl(environment.apiUrl + 'signalr', {
          skipNegotiation: true,  // Bỏ qua quá trình đàm phán (tùy chọn, chỉ sử dụng WebSockets)
          transport: signalR.HttpTransportType.WebSockets,  // Sử dụng WebSockets cho kết nối
          withCredentials: true   // Cấu hình gửi cookie hoặc thông tin xác thực (credentials)
        })
        .build();
    
        this.connection
          .start()
          .then(() => console.log('Signalr Connection started!'))
          .catch(() => console.log('Error while establishing connection!'));
    
        this.connection.on(
          'NotifyVuViec',
          async (vuViecId: any, donvi: any, noidung: any) => {
            this.search();
          }
        );
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
    if (this.connection) {
      this.connection
        .stop()
        .then(() => console.log('SignalR Connection stopped!'))
        .catch((err) => console.log('Error while stopping connection:', err));
    }
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
    this.trangChuService.search(this.searchModel).subscribe({
      next: (result) => {
        this.modelResult = result.data;
        this.getChart12Thang(
          this.modelResult.listThang,
          this.modelResult.listThangSoVu,
          this.modelResult.listThangTongTien
        );

        this.getDuLieuMap(this.modelResult.listDataMap);

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

        this.listTopTinh = this.modelResult.listTopTinh;
        this.listTopTangVat = this.modelResult.listTopTangVat;

        this.getChartXuLy(
          this.modelResult.listLinhVucDonut,
          this.modelResult.listLinhVucDonutSoVu
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

  public field: Object;

  //#endregion-----------Biểu đồ tình hình xử lý--------
  public chartOptions: any = {};

  getChartXuLy(dataLabel: any[], dataSeries: any[]) {
    const seriesData = dataSeries;
    const total = seriesData.reduce((a, b) => a + b, 0);

    this.chartOptions = {
      series: seriesData,
      chart: {
        type: 'donut',
        height: 300,
      },
      labels: dataLabel,
      colors: [
        '#F8285A',
        '#F6C000',
        '#7239EA',
        '#1B84FF',
        '#1E2129',
        '#17C653',
      ],
      responsive: [
        {
          breakpoint: 480,
          options: {
            chart: {
              width: 300,
            },
            legend: {
              position: 'bottom',
            },
          },
        },
      ],
      dataLabels: {
        enabled: true,
        formatter: function (val: number) {
          return val.toFixed(1) + '%';
        },
      },
      plotOptions: {
        pie: {
          donut: {
            size: '40%',
            labels: {
              show: true,
              total: {
                show: true,
                label: 'Vụ',
                formatter: function () {
                  return total.toString();
                },
              },
            },
          },
        },
      },
      // title: {
      //   show: false,
      //   text: 'Donut Chart with Total',
      //   align: 'center',
      // },
      legend: {
        show: true,
        position: 'bottom', // Đặt labels ở dưới biểu đồ
        horizontalAlign: 'center',
      },
    };
  }
  //#endregion

  //#region -------------Biểu đồ 12 tháng-------------
  public chart12ThangOptions: any = {};
  getChart12Thang(thang: any[], thangVu: any[], thangTien: any[]) {
    this.chart12ThangOptions = {
      series: [
        {
          name: 'Số vụ',
          type: 'column', // Dữ liệu dạng cột
          data: thangVu,
        },
        {
          name: 'Tổng tiền xử phạt',
          type: 'line', // Dữ liệu dạng đường
          data: thangTien,
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
        width: [0, 4],
      },
      title: {
        text: '',
      },
      dataLabels: {
        enabled: true,
        formatter: function (val: number) {
          return val.toLocaleString().replace(/,/g, '.'); // Định dạng số với dấu phẩy hàng nghìn
        },
      },
      labels: thang,
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
          opposite: true,
          title: {
            text: 'Tổng tiền xử phạt',
          },
        },
      ],
      colors: ['#198B4E', '#F6A953'],
      legend: {
        position: 'top', // Đặt vị trí legend trên cùng
        horizontalAlign: 'right',
      },
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
        height: 350,
        type: 'line', // Chế độ mặc định là line nhưng kết hợp thêm column
        stacked: false,
        toolbar: {
          show: false,
        },
      },
      dataLabels: {
        enabled: true,
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
        height: 350,
        type: 'line', // Chế độ mặc định là line nhưng kết hợp thêm column
        stacked: false,
        toolbar: {
          show: false,
        },
      },
      stroke: {
        width: [0, 0],
      },
      dataLabels: {
        enabled: true,
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
      legend: {
        position: 'top', // Đặt vị trí legend trên cùng
        horizontalAlign: 'right',
      },
      colors: ['#198B4E', '#F6A953'],
    };
  }
  //#endregion

  //#region ---------------Xử lý bản đồ---------

  Highcharts: typeof Highcharts = Highcharts;
  chartConstructor = 'mapChart';
  updateFlag:boolean = false;
  oneToOneFlag:boolean = true;

  chartMapOptions: Highcharts.Options = {chart: {
    map: topology,
  },responsive: {
    rules: [], // Để trống nếu không cần tùy chỉnh
  },series:[]};

  getDuLieuMap(data: any) {
    this.chartMapOptions = {
      chart: {
        map: topology,
      },
      title: {
        text: '',
      },
      // subtitle: {
      //   text: 'Tiêu đề 2',
      // },
      mapNavigation: {
        enabled: true,
        buttonOptions: {
          alignTo: 'spacingBox',
        },
      },
      legend: {
        enabled: true,
      },
      colorAxis: {
        min: 0,
        max: 10, // Các giá trị lớn hơn bạn có thể chỉnh lại phù hợp với dữ liệu
        stops: [
          [0, '#f1f1f4'], // Màu sắc cho giá trị thấp
          [0.5, '#FF9999'], // Màu sắc cho giá trị trung bình
          [1, '#FF0000'], // Màu sắc cho giá trị cao
        ],
      },
      credits: {
        enabled: false, // Vô hiệu hóa phần credits
      },
      series: [
        {
          type: 'map',
          name: 'Vụ việc',
          states: {
            hover: {
              color: '#dfffea',
            },
          },
          dataLabels: {
            enabled: true,
            format: '{point.name}',
          },
          allAreas: false,
          data: data,
        },
      ],
      responsive: {
        rules: [], // Để trống nếu không cần tùy chỉnh
      },
    };
    // // Cập nhật biểu đồ sau khi có dữ liệu
    // Highcharts.mapChart('container', this.chartMapOptions);
  }
  //#endregion
}
