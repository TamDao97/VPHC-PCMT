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
  selector: 'app-thong-ke-xu-ly',
  templateUrl: './xu-ly.component.html',
  styleUrls: ['./xu-ly.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class ThongKeXuLyComponent implements OnInit {
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
}
