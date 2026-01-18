import {
  ChangeDetectorRef,
  Component,
  OnInit,
  ViewChild,
  ViewEncapsulation,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  NgbActiveModal,
  NgbDateStruct,
  NgbModal,
} from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { FileProcess } from 'src/app/cores/shared/common/file-process';
import { MessageService } from 'src/app/cores/shared/services/message.service';
import { Constants } from 'src/app/cores/shared/common/constants';
import { DateUtils } from 'src/app/cores/shared/common/date-utils';
import { FileService } from 'src/app/cores/shared/services/file.service';
import { LanguageService } from 'src/app/cores/shared/services/language.service';
import { ComboboxCoreService } from 'src/app/cores/shared/services/combobox-core.service';
import { v4 as uuidv4 } from 'uuid';
import {
  GridComponent,
  RowDeselectEventArgs,
  SelectionSettingsModel,
} from '@syncfusion/ej2-angular-grids';
import { QuyetDinhService } from 'src/app/vphc/service/quyet-dinh-service';

@Component({
  selector: 'app-danh-muc-qd-choose',
  templateUrl: './danh-muc-choose.component.html',
  styleUrls: ['./danh-muc-choose.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class DanhMucQuyetDinhComponent implements OnInit {
  constructor(
    public fileProcess: FileProcess,
    private routeA: ActivatedRoute,
    private messageService: MessageService,
    public constant: Constants,
    public dateUtils: DateUtils,
    private QuyetDinhService: QuyetDinhService,
    private router: Router,
    private fileService: FileService,
    private translate: TranslateService,
    private lgService: LanguageService,
    private activeModal: NgbActiveModal,
    private changeDetectorRef: ChangeDetectorRef,
    private comboboxService: ComboboxCoreService
  ) {
    this.translate.use(this.lgService.getLanguage());
  }

  @ViewChild('grDanhMuc')
  public grDanhMuc: GridComponent;
  public selectionOptions: SelectionSettingsModel = {
    type: 'Single',
    mode: 'Row',
  };

  //#region -----------Các sự kiện của modal--------------
  ngOnInit(): void {
    //Hứng sự kiện thay đổi ngôn ngữ để load lại Component
    this.lgService.onLanguageChanged.pipe().subscribe((languageCode) => {
      if (languageCode) {
        this.translate.use(languageCode);
      }
    });

    this.getDanhMuc();
  }

  save(isContinue: boolean = false) {
    if (!this.currentRowData) {
      this.messageService.showWarning(
        'Hãy lựa chọn một mẫu để tiến hành lập quyết định.'
      );
      return;
    }
    this.activeModal.close({
      success: true,
      data: this.currentRowData,
    });
  }

  close(result: false): void {
    this.activeModal.close(result); // Đóng modal
  }
  //#endregion

  //#region ----------------Danh sách cán bộ----------------
  public fields = { text: 'name', value: 'id' };
  public listDanhMuc: any[];
  getDanhMuc() {
    this.QuyetDinhService.getDanhMuc().subscribe({
      next: (result) => {
        if (result.isStatus) {
          this.listDanhMuc = result.data;
          this.grDanhMuc.selectedRowIndex = 1;
        }
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });
  }

  private selectedRowIndex: number | null = null;
  private currentRowData: any = null;

  // Lắng nghe sự kiện rowSelected để xử lý khi dòng được chọn
  onRowSelecting(args: any): void {
    this.currentRowData = args.data;
    this.selectedRowIndex = args.rowIndex;
  }

  // Lắng nghe sự kiện rowDeselected để xử lý khi dòng bị bỏ chọn
  onRowDeselected(args: any): void {
    if (this.selectedRowIndex == args.rowIndex) {
      this.grDanhMuc.selectRow(this.selectedRowIndex!);
    }
  }
  //#endregion

  //#endregion xử lý Check, Check all
  // Hàm xử lý khi checkbox trong từng dòng thay đổi
  public onCheckboxChange(event: any, updatedRecord: any): void {
    this.listDanhMuc.forEach((row) => {
      row.checked = row.id === updatedRecord.id;
    });
  }
  //#endregion
}
