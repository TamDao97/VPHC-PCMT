import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  ElementRef,
  OnDestroy,
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
import { VuViecService } from '../../service/vu-viec.service';
import { TranslateService } from '@ngx-translate/core';
import { TreeGridComponent } from '@syncfusion/ej2-angular-treegrid';
import { FormControl, Validators } from '@angular/forms';
import { FileProcess } from 'src/app/cores/shared/common/file-process';
import { MessageService } from 'src/app/cores/shared/services/message.service';
import { Constants } from 'src/app/cores/shared/common/constants';
import { DateUtils } from 'src/app/cores/shared/common/date-utils';
import { FileService } from 'src/app/cores/shared/services/file.service';
import { LanguageService } from 'src/app/cores/shared/services/language.service';
import { ComboboxCoreService } from 'src/app/cores/shared/services/combobox-core.service';
import { environment } from 'src/environments/environment';
import { TabComponent } from '@syncfusion/ej2-angular-navigations';
import { DropDownListComponent } from '@syncfusion/ej2-angular-dropdowns';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { v4 as uuidv4 } from 'uuid';

@Component({
  selector: 'app-to-chuc-modify',
  templateUrl: './to-chuc-modify.component.html',
  styleUrls: ['./to-chuc-modify.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class ToChucModifyComponent implements OnInit {
  constructor(
    public fileProcess: FileProcess,
    private routeA: ActivatedRoute,
    private messageService: MessageService,
    public constant: Constants,
    public dateUtils: DateUtils,
    private vuViecService: VuViecService,
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

  id: any = '';
  model: any = {
    ten: '',
    diaChiTruSo: '',
    maSoDoanhNghiep: '',
    soDKKD: '',
    ngayCapDKKD: '',
    noiCapDKKD: '',
    hoTenPhapNhan: '',
    gioiTinh: '',
    chucVu: '',
  };
  listData: any[] = [];
  isEdit: boolean = false;

  ngOnInit(): void {
    if (!this.id) {
      this.model.idToChucVP = uuidv4();
    }else{
      this.isEdit = true;
    }
    //Hứng sự kiện thay đổi ngôn ngữ để load lại Component
    this.lgService.onLanguageChanged.pipe().subscribe((languageCode) => {
      if (languageCode) {
        this.translate.use(languageCode);
      }
    });

    this.getDataCombobox();
  }

  //#region ----------------Xử lý combobox----------------
  public fields = { text: 'name', value: 'id' };
  public listGioiTinh: any[];
  getDataCombobox() {
    this.comboboxService.getGioiTinh().subscribe({
      next: (result) => {
        if (result.isStatus) {
          this.listGioiTinh = result.data;
        }
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });
  }

  onSelect(event: any, type: any): void {
    if (type == 'GioiTinh') {
      this.model.tenGioiTinh = event.itemData.name;
    }
  }
  //#endregion

  save(isContinue: boolean = false) {
    if (this.id) {
      this.activeModal.close({ success: true, data: this.model });
    } else {
      this.listData.push(this.model);
      this.model = {};
      if (!isContinue) {
        this.activeModal.close({ success: true, data: this.listData });
      }
    }
  }

  close(result: false): void {
    this.activeModal.close(result); // Đóng modal
  }
}
