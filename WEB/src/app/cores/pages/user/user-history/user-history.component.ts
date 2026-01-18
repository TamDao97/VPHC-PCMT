import { Component, OnInit } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { UserHistoryService } from '../service/user-history.service';
import { TranslateService } from '@ngx-translate/core';
import { Constants } from 'src/app/cores/shared/common/constants';
import { DateUtils } from 'src/app/cores/shared/common/date-utils';
import { FileProcess } from 'src/app/cores/shared/common/file-process';
import { SearchGlobalService } from 'src/app/cores/shared/common/search-global.service';
import { LanguageService } from 'src/app/cores/shared/services/language.service';
import { MessageService } from 'src/app/cores/shared/services/message.service';
import { MenuOptions } from 'src/app/cores/shared/models/menu-options.model';
import { ComboboxCoreService } from 'src/app/cores/shared/services/combobox-core.service';

@Component({
  selector: 'app-user-history',
  templateUrl: './user-history.component.html',
  styleUrls: ['./user-history.component.scss']
})
export class UserHistoryComponent implements OnInit {

  constructor(
    public constant: Constants,
    private dateUtils: DateUtils,
    private fileProcess: FileProcess,
    private userHistoryService: UserHistoryService,
    private messageService: MessageService,
    private comboboxService: ComboboxCoreService,
    private searchGlobalService: SearchGlobalService,
    private translate: TranslateService,
    private lgService: LanguageService,
  ) {
    this._unsubscribeAll = new Subject();
    this.translate.use(this.lgService.getLanguage());
  }

  startIndex = 1;
  userHistories: any[] = [];
  dateFrom = null;
  dateTo = null;
  listUser: any[] = [];
  _unsubscribeAll: Subject<any>;

  searchModel: any;

  ngOnInit(): void {
    //Hứng sự kiện thay đổi ngôn ngữ để load lại Component
    this.lgService.onLanguageChanged.pipe().subscribe(languageCode => {
      if (languageCode) {
        this.translate.use(languageCode);
      }
    });

    //Khởi tạo model tìm kiếm
    this.initModelSearch();

    this.getListUser();
    
    this.searchGlobalService.onDataChanged.pipe(takeUntil(this._unsubscribeAll)).subscribe(data => {
      if (data) {
        this.searchModel.userId = data.userId;
        this.searchModel.type = data.type;
        this.searchModel.name = data.name;
        this.searchModel.dateToV = data.dateToV;
        this.searchModel.dateFromV = data.dateFromV;
        this.searchModel.pageNumber = data.pageNumber;

        this.search();
      }
    });

    this.searchGlobalService.onRefreshData.pipe(takeUntil(this._unsubscribeAll)).subscribe(data => {
      if (data) {
        this.clearData();
      }
    });

    // this.searchGlobalService.onExportExcel.pipe(takeUntil(this._unsubscribeAll)).subscribe(data => {
    //   if (data) {
    //     this.exportExcel();
    //   }
    // });

    // this.searchGlobalService.onExportPdf.pipe(takeUntil(this._unsubscribeAll)).subscribe(data => {
    //   if (data) {
    //     this.exportPdf();
    //   }
    // });

    this.setToolbarConfig();
  }

  //Khởi tạo model tìm kiếm
  initModelSearch() {
    this.searchModel = {
      pageNumber: 1,
      pageSize: 10,
      totalItems: 0,
      userId: null,
      name: '',
      type: null,
      dateToV: this.dateUtils.getDateNowToObject(),
      dateFromV: this.dateUtils.getDateNowToObject(),
      dateTo: null,
      dateFrom: null,
      orderBy: '',
      orderType: ''
    }
    this.dateFrom = null;
    this.dateTo = null;
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
        Placeholder: 'Tìm kiếm nội dung',
        Items: [
          {
            FieldName: 'userId',
            Name: 'Tên tài khoản',
            Type: 'text',
            DisplayName: 'name',
            Placeholder: 'Tìm kiếm theo tên tài khoản',
            ValueName: 'id',
            GetData: (): Observable<any> => {
              return this.comboboxService.getListUser();
            }
          },
          {
            FieldName: 'type',
            Name: 'Phân loại',
            Type: 'ngselect',
            DisplayName: 'Name',
            ValueName: 'Id',
            Data: this.constant.UserHistory_Type
          },
          {
            //FieldName: 'ngayMoHS',
            Name: 'Thời điểm',
            FieldNameTo: 'dateToV',
            FieldNameFrom: 'dateFromV',
            Type: 'date',
            GetData: null,
          },
        ]
      }
    };

    this.searchGlobalService.setConfig(meuOptions);
  }

  getListUser() {
    this.comboboxService.getListUser().subscribe(
      result => {
        if (result.isStatus) {
          this.listUser = result.data;
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }
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
    if (this.searchModel.dateFromV != null) {
      this.searchModel.dateFrom = this.dateUtils.convertObjectToDate(this.searchModel.dateFromV);
    } else {
      this.searchModel.dateFrom = null;
    }

    if (this.searchModel.dateToV != null) {
      this.searchModel.dateTo = this.dateUtils.convertObjectToDate(this.searchModel.dateToV);
    } else {
      this.searchModel.dateTo = null;
    }

    this.userHistoryService.searchUserHistory(this.searchModel).subscribe(
      (result: any) => {
        if (result.isStatus) {
          this.startIndex = ((this.searchModel.pageNumber - 1) * this.searchModel.pageSize + 1);
          this.userHistories = result.data.dataResults;
          this.searchModel.totalItems = result.data.totalItems;
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  exportExcel() {
    this.userHistoryService.exportExcel(this.searchModel).subscribe(data => {
      var blob = new Blob([data], { type: 'octet/stream' });
      var url = window.URL.createObjectURL(blob);
      this.fileProcess.downloadFileLink(url, "LichSuThaoTac.xlsx");
    }, error => {
      const blb = new Blob([error.error], { type: "text/plain" });
      const reader = new FileReader();

      reader.onload = () => {
        if (reader && reader.result)
          this.messageService.showMessage(reader.result.toString().replace('"', '').replace('"', ''));
      };
      // Start reading the blob as text.
      reader.readAsText(blb);
    });
  }

  exportPdf() {
    this.userHistoryService.exportPdf(this.searchModel).subscribe(data => {
      var blob = new Blob([data], { type: 'octet/stream' });
      var url = window.URL.createObjectURL(blob);
      this.fileProcess.downloadFileLink(url, "LichSuThaoTac.pdf");
    }, error => {
      const blb = new Blob([error.error], { type: "text/plain" });
      const reader = new FileReader();

      reader.onload = () => {
        if (reader && reader.result)
          this.messageService.showMessage(reader.result.toString().replace('"', '').replace('"', ''));
      };
      // Start reading the blob as text.
      reader.readAsText(blb);
    });
  }

  clearData() {
    //Khởi tạo model tìm kiếm
    this.initModelSearch();
    this.setToolbarConfig();
  }
}
